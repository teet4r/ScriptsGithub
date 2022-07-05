using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventPoint
{
    public GameObject[] points;
}
[System.Serializable]
public class GameObjects
{
    public GameObject[] gameObjects;
}

public class Floor : MonoBehaviour
{
    // �⺻ ��
    public int floor_color { get; protected set; }             // ���� ����
    public int floor_level { get; protected set; }             // �ش� ���� �� ������
    public float dirty_index;           // ���� ������ ����
    public bool is_cleaner_called;      // ���� cleaner�� ȣ���ߴٸ� true          
    protected float min_respawntime;       // ��� ���� �ּ� �ð�
    protected float max_respawntime;       // ��� ���� �ִ� �ð�
    public int max_wait_cnt;            // ��⿭�� ������ �� �ִ� �ִ� ��� ��

    // ����� �����Ǵ� ����
    [SerializeField]
    private GameObject _spawn_point_up;
    [SerializeField]
    private GameObject _spawn_point_down;
    public Vector2 spawn_point_up { get; private set; }
    public Vector2 spawn_point_down { get; private set; }

    /// �̺�Ʈ ������Ʈ
    /// �̺�Ʈ�� �ִ� ������ ����
    [SerializeField]
    protected EventPoint[] event_points; // 2���� �迭
    protected EventObject[][] event_objects; // event_points�� event_object ��ũ��Ʈ ����
    [SerializeField]
    protected EventPoint[] exit_points; // �� ������ �ݵ�� �����ؾ���
    protected EventObject[][] exit_eventobject; // ���������� ���� �� �� �� �ϳ�

    // ȭ�鿡 ���̴� ������ ������
    public GameObject dirty_bar;
    public TextMesh dirty_text; // ������ ������ ��ġǥ��
    public SpriteRenderer dirty_text_box; // ������ ������ �ڽ� ������ ������ ���� ������ �뵵
    public float auto_clean_per_second; // �ʴ� �ڵ� û��
    public Coroutine auto_clean_coroutine;

    [SerializeField]
    protected TextMesh floor_level_text; // ���� �� ��ġǥ��

    public Human up_first;
    public bool is_using_up { get; protected set; }
    public Human down_first;
    public bool is_using_down { get; protected set; }

    protected Coroutine make_human_coroutine;
    protected bool being_on_the_elevator = false; // ����� �¿�� �ִ� ���̸� true

    // ���� �ִ� ���������� ��: �ν����Ϳ��� ����
    // ���ʺ��� idx: 0, 1, 2, ...
    [SerializeField]
    protected GameObjects[] elevator_doors;
    private Vector3[,] original_door_pos; // ���������� ���� ���� ��ġ
    public bool[] is_elevatordoor_opened { get; protected set; }

    protected void Awake()
    {
        is_using_up = false;
        is_using_down = false;

        is_elevatordoor_opened = new bool[elevator_doors.Length];
        for (int i = 0; i < elevator_doors.Length; i++)
            is_elevatordoor_opened[i] = false;
        original_door_pos = new Vector3[elevator_doors.Length, 2];
        for (int i = 0; i < elevator_doors.Length; i++)
        {
            original_door_pos[i, 0] = elevator_doors[i].gameObjects[1].transform.localPosition;
            original_door_pos[i, 1] = elevator_doors[i].gameObjects[2].transform.localPosition;
        }
    }
    protected void Start()
    {
        spawn_point_up = (Vector2)_spawn_point_up.transform.position;
        spawn_point_down = (Vector2)_spawn_point_down.transform.position;

        // event_objects �ʱ�ȭ
        event_objects = new EventObject[event_points.Length][];
        for (int i = 0; i < event_points.Length; i++)
            event_objects[i] = new EventObject[event_points[i].points.Length];
        for (int i = 0; i < event_points.Length; i++)
            for (int j = 0; j < event_points[i].points.Length; j++)
            {
                event_objects[i][j] = event_points[i].points[j].GetComponent<EventObject>();
                event_objects[i][j].cur_floor = this;
                event_objects[i][j].kind_of = i;
                event_objects[i][j].id = j;
            }
        // exit_eventobject �ʱ�ȭ
        exit_eventobject = new EventObject[exit_points.Length][];
        for (int i = 0; i < exit_points.Length; i++)
            exit_eventobject[i] = new EventObject[exit_points[i].points.Length];
        for (int i = 0; i < exit_points.Length; i++)
            for (int j = 0; j < exit_points[i].points.Length; j++)
                exit_eventobject[i][j] = exit_points[i].points[j].GetComponent<EventObject>();

        if (dirty_text != null)
            dirty_text.text = "0";

        try
        {
            MeshRenderer floor_level_mesh = floor_level_text.GetComponent<MeshRenderer>();
            floor_level_mesh.sortingLayerName = "Floor";
            floor_level_mesh.sortingOrder = 1;

            MeshRenderer dirty_mesh = dirty_text.GetComponent<MeshRenderer>();
            dirty_mesh.sortingLayerName = "Floor";
            dirty_mesh.sortingOrder = 1;
        }
        catch { }

        for (int i = 0; i < 3; i++)
            StartCoroutine(ElevatorDoorsSelfFix(i));

        auto_clean_coroutine = StartCoroutine(AutoClean());
    }

    public virtual void Set(int floor_level, float min_respawntime = 5f, float max_respawntime = 15f, int max_wait_cnt = 5) { }
    public virtual void SetDirtyRate(float dirty_size) { }
    public virtual IEnumerator MakeHuman() { yield return null; }
    public virtual IEnumerator MakeRush() { yield return null; }
    protected virtual void Act() { }
    public void StartUseFloor(int elevator_layer)
    {
        if (!is_using_up && elevator_layer == ElevatorClass.MovingStateForLayer.ASCENDING)
            is_using_up = true;
        if (!is_using_down && elevator_layer == ElevatorClass.MovingStateForLayer.DESCENDING)
            is_using_down = true;
    }
    public void FinishUseFloor(int elevator_layer)
    {
        if (elevator_layer == ElevatorClass.MovingStateForLayer.ASCENDING)
            is_using_up = false;
        else
            is_using_down = false;
    }
    public void ReadyToGetElevator(Human human_script)
    {
        if (human_script.destination_floor > floor_level)
            up_first = human_script;
        else
            down_first = human_script;
    }
    public IEnumerator CallClean()
    {
        while (!is_cleaner_called)
        {
            (Gamemanager.Instance.buildgame.floors[FID.CLEAN][0] as F_Clean).Call_Clean(floor_level);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public IEnumerator GetEventPoints(Queue<EventObject> q)
    {
        int idx = -1, idx2 = -1;
        for (int i = 0; i < event_objects.Length; i++)
        {
            idx = Random.Range(-1, event_objects[i].Length);
            if (idx >= 0)
                q.Enqueue(event_objects[i][idx]);
        }

        // ������ ������ �ؾ��ϹǷ� �������� �߰�
        // ������� ������ ������ �ϳ�
        idx = Random.Range(0, exit_eventobject.Length);
        idx2 = Random.Range(0, exit_eventobject[idx].Length);
        q.Enqueue(exit_eventobject[idx][idx2]);
        yield return null;
    }
    public IEnumerator GetSpecificationEventPoints(Queue<EventObject> q, int event_object_kind, int event_object_id) // Ư�� eventobject�� �޾ƿ�
    {
        q.Enqueue(event_objects[event_object_kind][event_object_id]);

        int idx = -1, idx2 = -1;
        idx = Random.Range(0, exit_eventobject.Length);
        idx2 = Random.Range(0, exit_eventobject[idx].Length);
        q.Enqueue(exit_eventobject[idx][idx2]);

        yield return null;
    }

    public IEnumerator OpenDoors(int line)
    {
        if (is_elevatordoor_opened[line])
            yield break;
        is_elevatordoor_opened[line] = true;
        // ����: light, leftdoor, rightdoor
        GameObjects elevator_door = elevator_doors[line];
        // �Һ� ����: ������ ����
        elevator_door.gameObjects[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
        // ������
        var leftdoor_pos = elevator_door.gameObjects[1].transform.localPosition;
        var rightdoor_pos = elevator_door.gameObjects[2].transform.localPosition;
        for (float i = 0; i <= 0.2; i += 0.02f)
        {
            elevator_door.gameObjects[1].transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            elevator_door.gameObjects[2].transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        yield return null;
    }
    public IEnumerator CloseDoors(int line)
    {
        if (!is_elevatordoor_opened[line])
            yield break;
        is_elevatordoor_opened[line] = false;
        // ����: light, leftdoor, rightdoor
        GameObjects elevator_door = elevator_doors[line];
        // ������
        var leftdoor_pos = elevator_door.gameObjects[1].transform.localPosition;
        var rightdoor_pos = elevator_door.gameObjects[2].transform.localPosition;
        for (float i = 0; i <= 0.2; i += 0.02f)
        {
            elevator_door.gameObjects[1].transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            elevator_door.gameObjects[2].transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        // �Һ� ����: ������ ����
        elevator_door.gameObjects[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0);
        yield return null;
    }

    // �������� ���� �� �ȴ����� ���������� ���� ���·� ������
    IEnumerator ElevatorDoorsSelfFix(int line)
    {
        while (true)
        {
            if (!is_elevatordoor_opened[line])
            {
                elevator_doors[line].gameObjects[1].transform.localPosition = original_door_pos[line, 0];
                elevator_doors[line].gameObjects[2].transform.localPosition = original_door_pos[line, 1];
            }
            yield return new WaitForSeconds(1f);
        }
    }

    // ���������� ���� ���� �ѱ�
    public void TurnOnLight(int line)
    {
        GameObjects elevator_door = elevator_doors[line];
        elevator_door.gameObjects[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1);
    }
    // ���������� ���� ���� ����
    public void TurnOffLight(int line)
    {
        GameObjects elevator_door = elevator_doors[line];
        elevator_door.gameObjects[0].GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0);
    }

    // �ڵ����� ���� û��
    // �ּڰ��� 0.3
    IEnumerator AutoClean()
    {
        while (!Gamemanager.Instance.buildgame.is_gameover)
        {
            SetDirtyRate(-Mathf.Max(0.3f, auto_clean_per_second));

            yield return new WaitForSeconds(1f);
        }
    }
}