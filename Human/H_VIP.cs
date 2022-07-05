using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_VIP : Human
{
    protected bool is_man;
    public override IEnumerator ActAtATM(ATM atm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, atm.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtVM(VendingMachine vm)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, vm.earn_things);
        // ¥�� ��÷ �� ���ߵ���
        if (Random.Range(0, 1f) <= vm.addict_threshold + Gamemanager.Instance.buffmanager.illed_human_rate && Gamemanager.Instance.buildgame.floors[FID.HOSPITAL].Count > 0)
        {
            // Sick��ȯ
            var sick = Gamemanager.Instance.objectpool.GetSick();

            //����ġ�� �̵�
            sick.transform.position = rigid.position;
            // ���� ����� ��ġ ��������Ʈ�� �̵�
            rigid.position = destinationfloor_script.spawn_point_up;

            //Sick �ʱ�ȭ
            sick.GetComponent<H_Sick>().Set(destinationfloor_script);
            sick.GetComponent<SpriteRenderer>().sprite = Gamemanager.Instance.objectpool.sick_sprites[color].sprite[is_man ? 0 : 1];

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

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, re.earn_things);
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
            sick.GetComponent<SpriteRenderer>().sprite = Gamemanager.Instance.objectpool.sick_sprites[color].sprite[is_man ? 0 : 1];

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
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, hd.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtSAD(SmokingAreaDoor sad) // Smoking Area Door
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtExit(Exit exit)
    {
        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtInfo(Infomation info)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, info.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtWP(WaterPurifier wp)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, wp.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtBS(BookShelf bs)
    {
        Gamemanager.Instance.buildgame.PlusHp(Gamemanager.Instance.buffmanager.healing_size);

        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, bs.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        yield return new WaitForSeconds(1f);
    }
    public override IEnumerator ActAtND(NormalDoor nd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, nd.earn_things * Gamemanager.Instance.buffmanager.vip_ticket);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
    public override IEnumerator ActAtFFExit(FirstFloorExit ffe)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, Gamemanager.Instance.buffmanager.go_to_home_pay_size * Gamemanager.Instance.buffmanager.vip_ticket);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        ReturnHuman();
    }
}