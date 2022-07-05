using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorClass : MonoBehaviour
{
    // gameObject layer
    public static class MovingStateForLayer
    {
        public const int ASCENDING = 12; // UpElevator
        public const int DESCENDING = 13; // DownElevator
        public const int FIXING = 8;  // ���� ���� 
    }

    public int line;                    // index: 0, 1, 2
    public int max_volume;                    // �¿� �� �ִ� �ִ� �ο� ��
    public int cur_volume;                    // ���� �¿�� �ִ� �ο� ��
    public int cur_floor;                    // ���� ���������Ͱ� �����ִ� ��
    public int available_top_floor;                    // ���� �����س��� ������ �� �ְ���
    public int available_bottom_floor;                    // ���� �����س��� ������ �� ������

    public Dictionary<string, int> skill_dict;

    public int vip_female_cnt;      // ���� Ÿ�� �ִ� �迩�� ��
    public int master_cnt;          // ���� Ÿ�� �ִ� ���� ��
    public int thief_cnt;           // ���� Ÿ�� �ִ� ���� ��

    public int save_moving_state;        // ���� �ϱ� ������ ����
    public Vector2 save_position;        // ���� �ϱ� ������ ����

    public int level;                    // ���������� ����
    public int cur_point;                    // ��� ������ ����Ʈ
    public float cur_exp;                   // ���� ����ġ
    public float cur_speed;                   // ���� ���� �ӵ�
    public float max_exp;                  // �������ϱ����� �ִ� ����ġ
    public float on_and_off_time;                   // ����� Ÿ�� ������ �ð� ����(������, ���� ������θ� ����)

    //3�� ��� ������
    public float crash_speed_rate;                // �� ���ο��� �� �� �̻��� ���������Ͱ� �浹���� �� �����ϴ� �ӵ� ���� -0.5
    public float overload_speed_rate;                // max_weight�� �Ѱ����� �پ��� �ӵ� ����  -0.5
    public float broken_speed_rate;             // �������� 0�̵Ǿ� �ı��Ǿ����� �پ��� �ӵ� ����  -0.5

    /// <������ ����>
    /// 0�� �Ǹ� �̵� �Ұ�
    /// ����� Ÿ�� ������ ������(������� ���̸� ���� ����) ����
    /// ������ ��������
    /// ������ �������� ��� �Ҹ�, �Ҹ��� ����/�ִ� ������ ����
    public float cur_durability;                // ���� ������ 
    public float max_durability;                // �ִ� ������ ���� ������ ���� ����
    public float temp_durability_size;  //�ӽð�
    public float auto_fix_per_second;
    public Coroutine auto_fix_coroutine;

    public GameObject selected_point; // ���������� ���� ǥ�� ������Ʈ

    public bool is_loading;                 // ����� �¿�� ������ false
    public bool is_overload;                // �ִ� ���뷮�� 80%�� ������ true
    public bool is_waiting;                 // �� ������ ������� �¿�ų� �������� true
    public bool is_crashing;                // ���������ͳ��� �浹�ϸ� true
    public bool is_fixing;                  // ���������Ͱ� �������̸� true
    public bool is_automating = false;      // ���������Ͱ� �ڵ� ���� ���̸� true

    SpriteRenderer sprite;
    public Sprite[] elevator_sprite;              // ���� �°��� ���� ���� �̹��� 0% 33% 66% 100%(�̻�)

    public GameObject thief_bubble;     // �ش� ���������Ϳ� ������ �ִٴ� ���� �����ִ� �̹���
    public GameObject master_bubble;    // �ش� ���������Ϳ� ȸ���� �ִٴ� ���� �����ִ� �̹���

    protected Floor floor_script;

    public EleSimpleInfo elevator_info; // ���� ������������ ���¸� �����ϰ� �����ִ� ��ư , ���������� �Ŵ������� ��������

    protected Coroutine move_coroutine;
    protected Coroutine wait_coroutine;         // ������� �¿�ų� ������ ����
    protected Coroutine auto_coroutine;
    public int save_top_floor, save_bottom_floor; // ���� ���� ���� ����

    public Vector2 end_point;
    Vector2 save_end_point;

    public Rigidbody2D rigid { get; protected set; }
    protected Dictionary<int, Queue<GameObject>> humans_in_elevator;          // ���� ���������Ͱ� �¿�� �ִ� ����� ����Ʈ

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        humans_in_elevator = new Dictionary<int, Queue<GameObject>>();
        sprite = GetComponent<SpriteRenderer>();
        floor_script = null;
    }

    protected void Start()
    {
        auto_fix_coroutine = StartCoroutine(AutoFix());
    }

    public void Set(int line, int building_bottom_floor, int building_top_floor)
    {
        this.line = line;
        end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

        float durability_color_int = (255 - (100 - (int)((cur_durability / max_durability) * 100)) * 2) / 255;
        sprite.color = new Color(durability_color_int, durability_color_int, durability_color_int);

        save_top_floor = available_top_floor = building_top_floor;
        save_bottom_floor = available_bottom_floor = building_bottom_floor;

        max_volume = 5 / Gamemanager.Instance.buffmanager.elevator_volume_half;

        skill_dict = new Dictionary<string, int>()
        {
            { "speed", 0 },
            { "volume", 0 },
            { "durability", 0 }
        };

        selected_point.SetActive(true);

        // �������ڸ��� ������
        move_coroutine = StartCoroutine(Move());
    }

    public void Reset(int setting_top_floor, int setting_bottom_floor) // ���� ������ ��
    {
        end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

        save_top_floor = available_top_floor = setting_top_floor;
        save_bottom_floor = available_bottom_floor = setting_bottom_floor;
    }

    //---------------------------------------------------------------------------
    IEnumerator Move()
    {
        while (!is_fixing)
        {
            yield return null;

            rigid.position = Vector2.MoveTowards(
                rigid.position,
                end_point,
                // Buff & Debuff
                cur_speed *                   
                // ����ӵ�
                overload_speed_rate *   
                // ������ �ӵ� ����
                crash_speed_rate *      
                // �浹 �ӵ� ����
                ((cur_volume == 0) ? 1 + Gamemanager.Instance.buffmanager.empty_elevator_speed_rate : 1) * 
                // �� ���������� �ӵ� ���� 
                ((cur_floor >= Gamemanager.Instance.buildgame.building_top_floor / 2) ? 1 + Gamemanager.Instance.buffmanager.acrophobia_speed_rate : 1) *
                // ��Ұ����� �ӵ� ����
                (1 + vip_female_cnt * Gamemanager.Instance.buffmanager.vip_female_speed_rate) *            
                // �迩�� �ӵ� ����
                (1 + master_cnt * Gamemanager.Instance.buffmanager.vvvvvip_speed_rate) *                  
                // ���� �ӵ� ����
                ((end_point.y > rigid.position.y) ? 1 - Gamemanager.Instance.buffmanager.gravity_elevator_rate : 1 + Gamemanager.Instance.buffmanager.gravity_elevator_rate) * 
                // �߷� �ӵ� ������
                Time.deltaTime
                );
        }
    }
    // �浹 ó�� ��ũ��Ʈ
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������ ��
        if (collision.gameObject.tag == "CheckBox")
        {
            floor_script = collision.gameObject.GetComponentInParent<Floor>();
            cur_floor = floor_script.floor_level;

            if (available_bottom_floor <= cur_floor && cur_floor <= available_top_floor) // ���� ���� ����� �ִ°�
            {
                if (move_coroutine != null) StopCoroutine(move_coroutine); // ���ʽ���� ���ʿ��� move_coroutine�� null
                wait_coroutine = StartCoroutine(Wait());
            }
            else
            {
                if (cur_floor >= available_top_floor)
                    gameObject.layer = MovingStateForLayer.DESCENDING;
                if (cur_floor <= available_bottom_floor)
                    gameObject.layer = MovingStateForLayer.ASCENDING;

                end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;
            }
        }
        else if (collision.gameObject.tag == "Elevator" && !Gamemanager.Instance.buffmanager.is_ghost_elevator && !is_crashing) // ���������� �� �浹
        {
            is_crashing = true;
            crash_speed_rate = 0.5f;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Elevator") // ���������� �� �浹 ����
        {
            is_crashing = false;
            crash_speed_rate = 1f;
        }
    }
    // �浹 ó�� ��ũ��Ʈ
    IEnumerator Wait() // ����� ������ Ÿ�� ���� �ڷ�ƾ
    {
        if (!humans_in_elevator.ContainsKey(cur_floor)) // ����
            humans_in_elevator.Add(cur_floor, new Queue<GameObject>());

        while (humans_in_elevator[cur_floor].Count > 0)
        {
            yield return floor_script.StartCoroutine(floor_script.OpenDoors(line));

            Human hs = humans_in_elevator[cur_floor].Dequeue().GetComponent<Human>();

            AddDurability(-temp_durability_size, false); // ������ ��ȭ
            AddVolume(-hs.population);

            // ����ġ �߰�
            cur_exp += hs.exp;
            if (cur_exp >= max_exp)
            {
                level++;
                elevator_info.ChangeLevel();
                cur_exp -= max_exp;
                max_exp *= 1.2f; // �ִ� ����ġ�� ����
                cur_point++;
            }

            hs.state = HumanState.WALKING;
            hs.ActOffElevator(this);

            yield return new WaitForSeconds(on_and_off_time); // off time
        }

        // ���� �ִ� ��� �¿�
        if ((!floor_script.is_using_up && gameObject.layer == MovingStateForLayer.ASCENDING) ||
            (!floor_script.is_using_down && gameObject.layer == MovingStateForLayer.DESCENDING)) // ��밡��
        {
            floor_script.StartUseFloor(gameObject.layer); // ���� �� ����Ѵٰ� �˸�

            Human waiter = (gameObject.layer == MovingStateForLayer.ASCENDING) ? floor_script.up_first : floor_script.down_first;
            // �˸´� ����� ������

            while (waiter != null && waiter.state == HumanState.WAITING && cur_volume + waiter.population <= max_volume) // ������� ��� ����
            {
                if (available_bottom_floor <= waiter.destination_floor && waiter.destination_floor <= available_top_floor)
                {
                    waiter.state = HumanState.BOARDING;

                    yield return floor_script.StartCoroutine(floor_script.OpenDoors(line));

                    //1. ���������� ���డ���Ѱ�, 2. ���������Ϳ� ���� �ڸ��� �ִ°�
                    /// ********** ����� ���������Ϳ� Ž
                    /// ********** ���������Ϳ� Ž���ν� �ؾ��� �ൿ ����

                    // ������������ ���¿� ���� �ʱ�ȭ�� ù ����ڸ� ����
                    if (gameObject.layer == MovingStateForLayer.ASCENDING)
                    {
                        if (floor_script.up_first == null) // �� ������ ������ ����� ȭ���� �����ȴٸ�
                            break;
                        floor_script.up_first = null;
                    }
                    else
                    {
                        if (floor_script.down_first == null)
                            break;
                        floor_script.down_first = null;
                    }

                    // ���������͸� ���� �̵�
                    // ������ Ż������ ���

                    if (waiter.gameObject.activeSelf) // ����� �����ϸ�(�߰��� ������� ������) �¿�
                    {
                        is_loading = true;
                        yield return waiter.StartCoroutine(waiter.MovetoElevator(waiter.transform.position, rigid.position));
                        is_loading = false;

                        if (!humans_in_elevator.ContainsKey(waiter.destination_floor)) // ����
                            humans_in_elevator.Add(waiter.destination_floor, new Queue<GameObject>());

                        humans_in_elevator[waiter.destination_floor].Enqueue(waiter.gameObject);

                        waiter.ActInElevator(this); // ���������� ���� �� �ൿ ����
                        waiter.transform.position = Gamemanager.Instance.objectpool.waiting_room.transform.position;

                        AddDurability(-temp_durability_size, false); // ������ ��ȭ
                        AddVolume(waiter.population);
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.3f); // ��� �������� �ٷ� üũ�ϸ� ���� ȥ�� �� �������״� ������ ��ٷ�����
                        break; // �¿�ٰ� �� ����� ������� break
                    }
                }
                else break; // �¿� ����� ������ break

                waiter = (gameObject.layer == MovingStateForLayer.ASCENDING) ? floor_script.up_first : floor_script.down_first;
                // ���ο� ����ڸ� �ҷ���
            }
        }
        yield return floor_script.StartCoroutine(floor_script.CloseDoors(line));
        floor_script.FinishUseFloor(gameObject.layer); // �� ����� �����ٰ� �˸�

        if (cur_durability <= 0)
        {
            broken_speed_rate = 0f; // �̵� �Ұ�
            sprite.color = Color.gray;
            elevator_info.ShowWarningState();
        }
        else
        {
            if (cur_floor >= available_top_floor)
                gameObject.layer = MovingStateForLayer.DESCENDING;
            if (cur_floor <= available_bottom_floor)
                gameObject.layer = MovingStateForLayer.ASCENDING;

            end_point = (gameObject.layer == MovingStateForLayer.ASCENDING) ? rigid.position + Vector2.up * 2 : rigid.position + Vector2.down * 2;

            move_coroutine = StartCoroutine(Move());
        }
    }

    // ���º�ȭ ��ũ��Ʈ
    public void AddDurability(float durability_size, bool is_auto_fix)
    {
        //������ ���� �� �̹��� ����
        cur_durability += durability_size + (is_auto_fix ? 0 : Gamemanager.Instance.buffmanager.durability_decrease_rate);
        {
            if (cur_durability < 0)
                cur_durability = 0;
            else if (cur_durability > max_durability)
                cur_durability = max_durability;

            float durability_color_int = (255 - (100 - cur_durability / max_durability * 100) * 2) / 255;
            sprite.color = new Color(durability_color_int, durability_color_int, durability_color_int);

            elevator_info.ShowDurabilityProgress();
        } // �̹��� ����
    } 
    public void AddVolume(int population)
    {
        cur_volume += population;

        if (cur_volume <= max_volume * 0.75f && is_overload)
        {
            is_overload = false;
            overload_speed_rate = 1f;
            elevator_info.ShowWarningState();
        } // ������ üũ
        else if (cur_volume > max_volume * 0.75f && !is_overload)
        {
            is_overload = true;
            overload_speed_rate = 0.5f - Gamemanager.Instance.buffmanager.overload_speed_rate;
            elevator_info.ShowWarningState();
        } // ������ üũ

        sprite.sprite = elevator_sprite[Mathf.FloorToInt((float)cur_volume / max_volume * 100) / 25];
    }
    // ���º�ȭ ��ũ��Ʈ










    // ���� ��ũ��Ʈ
    public void GoToFix(H_Mechanic mechanic_script) // ������ �Ϸ� �̵�
    {
        is_fixing = true;

        if (wait_coroutine != null) StopCoroutine(wait_coroutine);

        sprite.color = Color.gray;

        save_end_point = end_point;
        save_position = rigid.position; // ���� ���� ���¸� ����
        save_moving_state = gameObject.layer;

        gameObject.layer = MovingStateForLayer.FIXING; // up, down�� �浹�����ʴ� �������� ���̾�

        elevator_info.ShowIsFixing(true); // ���� ���� ������ �˷���

        F_Repairshop repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop; // �������� ����Ұ� �� �ϳ�
        repairshop.is_line_repair_using[line] = true; // �ش� ���ο��� �� ���������͸� ����
        // ���� ����

        StartCoroutine(MoveForFix(mechanic_script));
    }
    IEnumerator MoveForFix(H_Mechanic mechanic_script) // ���� ���ķ� �̵��ϱ� ���� �ڷ�ƾ
    {
        var repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop;

        Vector2 fix_end_point = repairshop.repair_point[line].transform.position;
        // ���������� �̵�
        while (rigid.position != fix_end_point)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, fix_end_point, 1.5f * Time.deltaTime); // move �������� �̵�
            yield return null;
        }

        // ����� ȣ���ϱ� �� �ش� �� ���ѱ�
        repairshop.TurnOnLight(line);
        repairshop.Call_Mechanic(this, mechanic_script); // ����� ȣ��
    }
    public void FinishFix() // ���� ��
    {
        broken_speed_rate = 1;
        Vector2 fix_end_point = save_position; // ���� �ڸ��� ���ư�

        F_Repairshop repairshop = Gamemanager.Instance.buildgame.floors[FID.REPAIRSHOP][0] as F_Repairshop;
        repairshop.is_line_repair_using[line] = false; // �ش� ���� �ٸ� ���������Ͱ� �̿� ����

        // �ڸ��� �����ϱ� �� �ش� �� �Ҳ���
        repairshop.TurnOffLight(line);
        StartCoroutine(MoveForReturn(fix_end_point)); // ���� ���� �ڸ��� ����
    }
    IEnumerator MoveForReturn(Vector2 fix_end_point) // ���� ���ķ� �̵��ϱ� ���� �ڷ�ƾ
    {
        // ���������� �̵�
        while (rigid.position != fix_end_point)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, fix_end_point, 1.5f * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_speed_rate_for_fix)); // move �������� �̵�
            yield return null;
        }

        // ���� ���·� ����
        sprite.color = Color.white;
        is_fixing = false;
        elevator_info.ShowIsFixing(false);
        gameObject.layer = save_moving_state;
        end_point = save_end_point;
        move_coroutine = StartCoroutine(Move());
    }
    // ���� ��ũ��Ʈ

    public void StartAuto()
    {
        is_automating = true;
        save_top_floor = available_top_floor;
        save_bottom_floor = available_bottom_floor; // ���� ����
        auto_coroutine = StartCoroutine(Auto());
        elevator_info.ChangeAuto();
    }
    public void StopAuto()
    {
        StopCoroutine(auto_coroutine);
        is_automating = false;
        available_top_floor = save_top_floor;
        available_bottom_floor = save_bottom_floor;
        elevator_info.ChangeAuto();
    }
    // ���� ��ũ��Ʈ
    IEnumerator Auto()
    {
        while(true)
        {
            available_top_floor = Gamemanager.Instance.buildgame.building_top_floor;
            available_bottom_floor = Gamemanager.Instance.buildgame.building_bottom_floor;
            yield return null;
        }
    }

    // �ڵ����� ���� ������ ����
    // �ּڰ��� 0.3
    IEnumerator AutoFix()
    {
        while (!Gamemanager.Instance.buildgame.is_gameover)
        {
            AddDurability(Mathf.Max(0.3f, auto_fix_per_second), true);

            yield return new WaitForSeconds(1f);
        }
    }
}