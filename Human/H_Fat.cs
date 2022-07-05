using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Fat : Human, ActInterface
{
    [SerializeField]
    protected int kind;
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, vm.earn_things * 3);
        // ¥�� ��÷ �� ���ߵ���(���� ���� �ִٸ� ����)
        if (Random.Range(0, 1f) <= vm.addict_threshold + Gamemanager.Instance.buffmanager.illed_human_rate && Gamemanager.Instance.buildgame.floors[FID.HOSPITAL].Count > 0)
        {
            //��ȯ
            var sick = Gamemanager.Instance.objectpool.GetSick();
            sick.GetComponent<H_Sick>().appear_point = rigid.position;

            //Sick �ʱ�ȭ
            sick.GetComponent<H_Sick>().Set(destinationfloor_script);
            sick.GetComponent<SpriteRenderer>().sprite = Gamemanager.Instance.objectpool.sick_sprites[color].sprite[kind * 2 + 1];

            // ��� �ൿ �ߴ�
            StopCoroutine(move_right);
            StopCoroutine(move_coroutine);
            rigid.velocity = Vector2.zero;
            // ����
            ReturnHuman();
        }
        yield return null;
    }

    public override IEnumerator ActAtRestExit(RestExit re)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, re.earn_things * 3);
        // ¥�� ��÷ �� ���ߵ���
        if (Random.Range(0, 1f) <= re.addict_threshold + Gamemanager.Instance.buffmanager.illed_human_rate && Gamemanager.Instance.buildgame.floors[FID.HOSPITAL].Count > 0)
        {
            // Sick��ȯ
            var sick = Gamemanager.Instance.objectpool.GetSick();

            //����ġ�� �̵�
            sick.transform.position = rigid.position;
            // ���� ����� ��ġ ��������Ʈ�� �̵�
            rigid.position = destinationfloor_script.spawn_point_up;

            //Sick �ʱ�ȭ
            sick.GetComponent<H_Sick>().Set(destinationfloor_script);
            sick.GetComponent<SpriteRenderer>().sprite = Gamemanager.Instance.objectpool.sick_sprites[color].sprite[kind * 2 + 1];

            // ��� �ൿ �ߴ�
            StopCoroutine(move_right);
            StopCoroutine(move_coroutine);
            rigid.velocity = Vector2.zero;
            // ����
            ReturnHuman();
        }
        yield return null;
    }
    public override IEnumerator ActAtHD(HospitalDoor hd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, hd.earn_things * 3);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtND(NormalDoor nd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, nd.earn_things);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtFFExit(FirstFloorExit ffe)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.go_to_home_pay_size / 2);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
}