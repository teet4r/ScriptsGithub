using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Guard : Human
{
    public int id;
    public int pay; // ����Ҷ� å��
    public int caught_thief_count = 0; // ���ݲ� ���� ���� ��

    public Guard_Info guard_info;
    public override void Set(Floor currentfloor_script)
    {
        gameObject.name = "guard";
        animator.speed = speed / 3;
        population = 0;

        is_INF_human = true;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
            destinations,
            currentfloor_script.floor_level,
            // params
            FID.RED,
            FID.BLUE,
            FID.GREEN,
            FID.YELLOW,
            FID.BANK,
            FID.MASTER,
            FID.REST,
            FID.VIP,
            FID.HOSPITAL
            );
        if (destinationfloor_script == null)
        {
            ReturnHuman();
            return;
        }

        destination_floor = destinationfloor_script.floor_level;

        StartCoroutine(MoveAllFloors(new Vector2(4f, (currentfloor_script.floor_level - 1) * 2)));
    }

    /// <summary>
    /// ������ ���ƴٴ�
    /// </summary>
    IEnumerator MoveAllFloors(Vector2 start)
    {
        // ó�� �������� �� ����(1��)
        rigid.position = start;
        Vector2 end = new Vector2(-4f, (currentfloor_script.floor_level - 1) * 2);
        yield return StartCoroutine(MoveAtoB(rigid.position, end));

        while (true)
        {
            // ���� ��, ������ �� ����
            currentfloor_script = destinationfloor_script;
            destinationfloor_script = Gamemanager.Instance.buildgame.GetDestinationExceptCurrent(
                destinations,
                currentfloor_script.floor_level,
                // params
                FID.FIRST,
                FID.RED,
                FID.BLUE,
                FID.GREEN,
                FID.YELLOW,
                FID.BANK,
                FID.MASTER,
                FID.REST,
                FID.VIP,
                FID.HOSPITAL
                );
            if (destinationfloor_script == null)
            {
                ReturnHuman();
                yield break;
            }
            destination_floor = destinationfloor_script.floor_level;

            // �������� ���� ���� �ָ� ������ ���ϼ��� ���� ��ٷȴٰ� ��Ÿ��
            yield return new WaitForSeconds(Mathf.Abs(currentfloor_script.floor_level - destination_floor));

            if(!Gamemanager.Instance.employeemanager.guard_dic.ContainsKey(id)) // �̹� �ذ� �� ����̸�
            {
                ReturnHuman();
                yield break;
            }    

            guard_info.employee_state.text = "���� :" + destination_floor + "�� ������";

            // ���������� �̵�
            transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));  // �̵��Ϸ��� �������� ���Ƽ�
            yield return StartCoroutine(MoveAtoB(new Vector2(-4f, (destination_floor - 1) * 2), new Vector2(4f, (destination_floor - 1) * 2)));

            // �������� �̵�
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));  // �̵��Ϸ��� �������� ���Ƽ�
            yield return StartCoroutine(MoveAtoB(new Vector2(4f, (destination_floor - 1) * 2), new Vector2(-4f, (destination_floor - 1) * 2)));
        }
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnGuard(this);
    }
    /// <summary>
    /// // ���ϰ��� ��ȣ�ۿ�
    /// </summary>
    /// <returns></returns>
    public override IEnumerator ActWithThief(Human other)
    {
        guard_info.employee_caught_thief_count.text = "�˰�Ƚ�� : " + ++caught_thief_count + "��";
        yield return null;
    }

    /// <summary>
    /// ������ ������ ���� ���� �� �ö�
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    void OnTriggerEnter2D(Collider2D collision)
    {
        var tag = collision.tag;
        if (tag == "Human")
        {
            var human = collision.GetComponent<Human>();
            human.StartCoroutine(human.ActWithGuard(this));
        }
    }
}