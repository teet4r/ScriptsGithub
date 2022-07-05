using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : MonoBehaviour, ActInterface
{
    public static class Mask
    {
        public const int Up = 9;
        public const int Down = 10;
        public const int After = 11;  // ���� �� 
    }

    public int color { get; set; } // �ڽ��� ������ ���� ������ �� �� ����
    public float dirty_size; // �� ����� ���� �������� �⿩�ϴ� ����
    public int destination_floor; // ������
    public int exp;
    public float speed;
    public int gold;

    // ���º���

    public int state { get; set; }
    public bool is_first { get; set; } // �� ����� ���� �� ������
    public bool is_INF_human { get; set; } = false; // �� ����� ������ ��ٸ� �� �ִ� ������� (û�Һ�, ����, �����, ������, ȸ��)

    // ���� ���ʰ� �� �� �ִ���
    [SerializeField]
    protected float time_who_can_wait;

    protected int layermask;
    public int population { get; protected set; } // �� ������ ����&����� ���� ���� ����
    public Floor currentfloor_script { get; protected set; }
    public Floor destinationfloor_script { get; protected set; }

    [SerializeField]
    protected SpriteRenderer speech_bubble;    //���� ��ǳ��
    [SerializeField]
    protected TextMesh des_bubble_text;    //������ ����

    [SerializeField]
    protected GameObject gold_image; // ����� �� �������� �����ٰ� (x���� �Һ� -0.3y�� 0.45���� ���� 0.9���� �ö󰬴ٰ� 0.8���� ������ �׸��� ������ �����)
    [SerializeField]
    protected TextMesh gold_text; // ����� �� �������� ���� ����

    public Rigidbody2D rigid { get; private set; }
    public BoxCollider2D coll { get; private set; }

    // �ڷ�ƾ
    protected Coroutine move_to_end_line = null;
    protected Coroutine move_coroutine = null;
    protected Coroutine move_right = null;
    protected Coroutine angry_timer = null;

    protected SpriteRenderer spriteRenderer;

    public Animator animator { get; private set; }

    protected float r;
    protected float g;
    protected float b;

    protected List<Floor> destinations { get; private set; } // ������ ������ ���� �迭, Awake()���� �Ҵ�

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        destinations = new List<Floor>();
        points = new Queue<EventObject>();

        r = spriteRenderer.color.r;
        g = spriteRenderer.color.g;
        b = spriteRenderer.color.b;

        gold_image.GetComponent<SpriteRenderer>().sortingLayerName = "HumanAfter";

        MeshRenderer gt_mesh = gold_text.GetComponent<MeshRenderer>();
        gt_mesh.sortingLayerName = "HumanAfter";
        gt_mesh.sortingOrder = 1;
    }
    protected void OnEnable()
    {
        state = HumanState.WAITING;
        spriteRenderer.color = new Color(r, g, b, 1);
        is_first = false;
        gold_image.SetActive(false);
    }

    public virtual void ReturnHuman() { } // �ڽ� Ŭ�������� overriding
    public virtual void Set(Floor currentfloor_script) { }
    public virtual void StartMove(Vector2 spawn_point, bool going_left)
    {
        layermask = 1 << gameObject.layer; // ���� ������Ʈ�� ���� ���̾�͸� �浹
        rigid.position = new Vector2(Random.Range(4f, 8f), spawn_point.y);

        if (gameObject.activeSelf)
            move_to_end_line = StartCoroutine(MoveToEndLine());
    }
    public virtual void ChangeSpriteLayer(string layername)
    {
        spriteRenderer.sortingLayerName = layername;
        speech_bubble.sortingLayerName = layername;
        MeshRenderer mesh = des_bubble_text.GetComponent<MeshRenderer>();
        mesh.sortingLayerName = layername;
        mesh.sortingOrder = 1;
    }
    protected IEnumerator MoveToEndLine()
    {
        spriteRenderer.flipX = true;

        Debug.DrawRay(rigid.position + Vector2.down * 0.3f, Vector2.left * 0.3f, Color.red);
        RaycastHit2D[] hit = Physics2D.RaycastAll(rigid.position + Vector2.down * 0.3f, new Vector2(-1, 0), 1, layermask);

        while (hit.Length > 1) // ���� ���� �浹 = �ٸ� ��ü�� �浹�ߴٴ� �� = ���ٷ� ��������
        {
            Debug.DrawRay(rigid.position + Vector2.down * 0.3f, Vector2.left * 0.3f, Color.red);
            hit = Physics2D.RaycastAll(rigid.position + Vector2.down * 0.3f, Vector2.left, 1f, layermask);
            // ���� ���� ������Ʈ�� ���� ���̾ �˻��ϰ� ��

            rigid.velocity = Vector2.right * speed; // move

            yield return null;
        }

        rigid.velocity = Vector2.zero;

        spriteRenderer.flipX = false;

        // ���������� ���� �������� ���
        if (destination_floor > currentfloor_script.floor_level) // �������� �̵�
        {
            ChangeSpriteLayer("HumanUp");
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(rigid.position.x, currentfloor_script.spawn_point_up.y)));
        }
        else
        {
            ChangeSpriteLayer("HumanDown");
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(rigid.position.x, currentfloor_script.spawn_point_down.y)));
        }


        move_coroutine = StartCoroutine(Move());
        angry_timer = StartCoroutine(AngryTimer(time_who_can_wait));
    }
    protected virtual IEnumerator Move()
    {
        while (true)
        {
            Debug.DrawRay(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left * 0.3f, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.3f, 0, 0), Vector2.left, 0.3f, layermask);
            // ���� ���� ������Ʈ�� ���� ���̾ �˻��ϰ� ��

            if (hit.collider != null)
            { // ��� Ȥ�� ���� �浹�� �������
                rigid.velocity = Vector2.zero;

                if (hit.collider.tag == "Endline") // ���� �浹
                {
                    animator.SetBool("isWaiting", true);
                    currentfloor_script.ReadyToGetElevator(this);
                    is_first = true;
                    yield break;
                }
                else
                    animator.SetBool("isWaiting", rigid.velocity == Vector2.zero);
            }
            else // �浹�� ���� ���� 
            {
                rigid.velocity = Vector2.left * speed; // move
                animator.SetBool("isWaiting", false);
            }

            yield return null;
        }
    }

    protected Queue<EventObject> points;
    protected virtual IEnumerator MoveRight(Vector2 start) // ��޺� ��ũ��Ʈ���� override
    {
        animator.SetBool("isWaiting", false);
        points.Clear();
        rigid.position = start;
        // �湮�� ���� �޾ƿ� ������ ��ٸ�
        yield return destinationfloor_script.StartCoroutine(destinationfloor_script.GetEventPoints(points));

        // ���� �湮
        while (points.Count != 0)
        {
            EventObject point = points.Dequeue();
            Vector2 point_pos = point.gameObject.transform.position;
            spriteRenderer.flipX = rigid.position.x < point_pos.x;  // �̵��Ϸ��� �������� ���Ƽ�
            while (rigid.position != point_pos)
            {
                rigid.position = Vector2.MoveTowards(rigid.position, point_pos, speed * Time.deltaTime);
                yield return null;
            }
            //yield return StartCoroutine(StopMoveRightWhile()); // �ش� ������ ���� ����

            //StopCoroutine(move_right);
            yield return StartCoroutine(point.EventOn(this));
            //move_right = StartCoroutine(MoveRight(rigid.position));
        }
    }

    protected virtual IEnumerator MoveAtoB(Vector2 start, Vector2 end)
    {
        rigid.position = start;
        animator.SetBool("isWaiting", false);

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime);
            yield return null;
        }
    }

    // elevator�� Ÿ�� ���� �̵��ϴ� ����
    public virtual IEnumerator MovetoElevator(Vector2 start, Vector2 end)
    {
        rigid.position = start;
        animator.SetBool("isWaiting", false);

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime * (1 + Gamemanager.Instance.buffmanager.elevator_on_speed_rate));
            yield return null;
        }
    }

    public IEnumerator Fadeout()
    {
        for (float a = 1; a >= 0; a -= 0.05f)
        {
            spriteRenderer.color = new Color(r, g, b, a);
            yield return null;
        }
    }

    /// <summary>
    /// Ư���ð� ��ٷȴٰ� �׶����� ž�� ���ϸ� �����ļ� �����
    /// </summary>
    /// <param name="patience_guage"></param>
    /// <returns></returns>
    protected IEnumerator AngryTimer(float patience_sec)
    {
        if (is_INF_human)
            yield break;

        yield return new WaitForSeconds(patience_sec);

        // ������ ��ٸ��� ���̸�
        if (state == HumanState.WAITING)
        {
            if (gameObject.layer == Mask.Up && is_first) // ���� ���ΰ��� �ٿ��� ���� ���ڸ����� ��� ���̿��ٸ�
                currentfloor_script.up_first = null;
            else if (gameObject.layer == Mask.Down && is_first)
                currentfloor_script.down_first = null;
            spriteRenderer.flipX = false;
            state = HumanState.WALKING;
            gameObject.layer = LayerMask.GetMask("HumanAfter");
            if (move_to_end_line != null)
                StopCoroutine(move_to_end_line);
            if (move_coroutine != null)
                StopCoroutine(move_coroutine);
            rigid.velocity = Vector2.zero;
            Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(-population);
            speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.ANGRY];
            des_bubble_text.text = "";
            currentfloor_script.SetDirtyRate(2 * dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));
            yield return StartCoroutine(MoveAtoB(rigid.position, new Vector2(-5, 2 * (currentfloor_script.floor_level - 1))));
            yield return StartCoroutine(Fadeout());
            ReturnHuman();
        }
    }

    /// <summary>
    /// Default Functions of ActInterface
    /// </summary>
    /// <param name="elevator"></param>
    public virtual IEnumerator ActAtATM(ATM atm)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, atm.earn_things);
        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtVM(VendingMachine vm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, vm.earn_things);
        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtHD(HospitalDoor hd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, hd.earn_things);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtExit(Exit exit)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtInfo(Infomation info)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, info.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtWP(WaterPurifier wp)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, wp.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtBS(BookShelf bs)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, bs.earn_things);

        yield return new WaitForSeconds(1f);
    }
    public virtual IEnumerator ActAtND(NormalDoor nd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, nd.earn_things);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtFFExit(FirstFloorExit ffe)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.go_to_home_pay_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtRestExit(RestExit re)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public virtual IEnumerator ActAtCounter(Counter c)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, c.earn_things);

        yield return new WaitForSeconds(3f);
    }





    /// <summary>
    /// Default Functions of reacting with other humans
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator ActWithGuard(Human other)
    {
        yield return null;
    }
    public virtual IEnumerator ActWithThief(Human other)
    {
        yield return null;
    }





    /// <summary>
    /// ActOff/ActIn
    /// </summary>
    /// <param name="elevator"></param>
    public virtual void ActOffElevator(ElevatorClass elevator)
    {
        // �� ������
        destinationfloor_script.SetDirtyRate(dirty_size * (1 + Gamemanager.Instance.buffmanager.dirty_increase_rate));

        // ��� �߰� ���� �ʹ� ������� ���� �����
        if (destinationfloor_script.dirty_index < 90)
            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, gold);
        else // �ʹ� ������� ���� ������ ����� ����� ��( ������ ���� )
            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.mold_pay_size);

        speech_bubble.gameObject.SetActive(false); // ��ǳ�� ����
        des_bubble_text.text = "";
        gameObject.layer = Mask.After;
        spriteRenderer.sortingLayerName = "HumanAfter";

        // ���������� �����̱� ����
        move_right = StartCoroutine(MoveRight(elevator.rigid.position));
    }

    public virtual void ActInElevator(ElevatorClass elevator)
    {
        //��� �α��� ����
        Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(-population);
        StopCoroutine(move_coroutine);
    }

    public IEnumerator CoinEffect(int gold) // �ݾ�
    {
        gold_text.text = (gold >= 0 ? "+" : "") + gold.ToString();
        gold_image.transform.position = gameObject.transform.position + new Vector3(-0.2f, 0.45f);
        gold_image.SetActive(true);

        while (gold_image.transform.position.y < gameObject.transform.position.y + 0.9)
        {
            gold_image.transform.position += Vector3.up * 0.01f;
            yield return null;
        }
        while (gold_image.transform.position.y > gameObject.transform.position.y + 0.8)
        {
            gold_image.transform.position += Vector3.down * 0.01f;
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        gold_image.SetActive(false);
    }
}