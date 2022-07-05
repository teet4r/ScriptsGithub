using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Mechanic : Human
{
    /// <<����>>
    /// ������ ���ϴ� ���������Ͱ� ���� ���� ����
    /// �������� �����ʿ��� ����
    /// �ش� ���������ͱ��� �̵�
    /// ������ �ٸ� ���� ���� ��Ȳ ����
    /// ������ �Ϸ�Ǹ� ���������� ��� �� ������ ���������� ����

    public int id;
    public int pay;
    public GameObject fixing_image; // ���� ������ �����ִ� �̹���
    public ElevatorClass elevator;  // �����ؾ��ϴ� ����������
    public Vector2 end_point;
    public int fixing_speed;// ���� �ӵ� (���õ�)
    public int fix_elevator_count; // ���ݱ��� ������ ���������� ��
    public bool is_working = false;

    public Mechanic_Info mech_info;

    public override void Set(Floor currentfloor_script)
    {
        spriteRenderer.flipX = false;
        population = 0;
        animator.speed = speed / 3;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        StartMove(currentfloor_script.spawn_point_up, true);
    }
    public override void StartMove(Vector2 spawn_point, bool going_left) // ������ ���� �̵� ����
    {
        rigid.position = spawn_point;
        end_point = (Vector2)(currentfloor_script as F_Repairshop).repair_point[elevator.line].transform.position;

        if (gameObject.activeSelf)
            move_coroutine = StartCoroutine(Move());
    }
    protected override IEnumerator Move() // ���� ��
    {
        while (end_point != rigid.position)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end_point, speed * Time.deltaTime); // move
            yield return null;
        }

        StartCoroutine(FixElevator()); // ���� ����
    }

    IEnumerator FixElevator() // ���� ����
    {
        fixing_image.SetActive(true);
        animator.SetBool("isFixing", true);

        for (int i = 0; i < 100; i++)  //  ���õ��� ���� ���� ���� ����, �ѹ��� ������ 1 ����
        {
            if (elevator.cur_durability >= elevator.max_durability || Gamemanager.Instance.buildgame.gold < 1)
                break;

            elevator.AddDurability(fixing_speed, false);

            if (Random.Range(0,100) < Gamemanager.Instance.buffmanager.reinforce_fail_threshold) // ��ȭ ���� ����� (���� ����)
            {
                //Gamemanager.Instance.buildgame.SetAlarm(elevator.name + " ���� ����!!!");
                elevator.AddDurability(-30, false);

                if (elevator.cur_durability < 10)
                    elevator.cur_durability = 10;

                break; //���� ����
            }

            Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, -1);

            yield return new WaitForSeconds(1f);
        }

        // ���� ��
        elevator.FinishFix();

        mech_info.employee_fix_elevator_count.text = "����Ƚ�� : " + ++fix_elevator_count + "��";
        fixing_image.SetActive(false);
        spriteRenderer.flipX = true;
        animator.SetBool("isFixing", false);

        yield return StartCoroutine(MoveAtoB(rigid.position,currentfloor_script.spawn_point_down));

        Gamemanager.Instance.employeemanager.remain_mechanic_count++;
        ReturnHuman();
    }
    protected override IEnumerator MoveAtoB(Vector2 start, Vector2 end) // ���� ��
    {
        rigid.position = start;

        while (rigid.position != end)
        {
            rigid.position = Vector2.MoveTowards(rigid.position, end, speed * Time.deltaTime);
            yield return null;
        }
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.employeemanager.ReturnMechanic(id);
    }
}