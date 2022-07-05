using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Sick : Human, ActInterface
{
    public Vector2 appear_point;//ȯ�� ���� ��ġ
    // �Ĵ� ������ ����Ȯ���� ����
    // ������ ����
    // Ư¡ ����, ������ ����
    // <����> ���� ���� ������ �����ϸ� �ȵ�
    public override void Set(Floor currentfloor_script)
    {
        name = "sick";
        color = PID.SICK;
        dirty_size = Random.Range(20f, 30f);
        speed = Random.Range(0.2f, 0.4f);
        exp = Random.Range(1, 3);
        gold = 1;
        population = 5;
        animator.speed = speed / 3;

        this.currentfloor_script = currentfloor_script;

        destinationfloor_script = Gamemanager.Instance.buildgame.GetClosestFID(currentfloor_script.floor_level, FID.HOSPITAL);

        destination_floor = destinationfloor_script.floor_level;
        
        speech_bubble.gameObject.SetActive(true);
        speech_bubble.sprite = Gamemanager.Instance.buildgame.bubble[Bubble.EMPTY];
        des_bubble_text.text = destination_floor.ToString();

        spriteRenderer.sortingLayerName = "HumanAfter";
        speech_bubble.sortingLayerName = "HumanAfter";

        if (destination_floor > currentfloor_script.floor_level)
        {
            gameObject.layer = Mask.Up;
            StartMove(appear_point, true);
        }
        else
        {
            gameObject.layer = Mask.Down;
            StartMove(appear_point, true);
        }
        Gamemanager.Instance.buildgame.ChangeCurrentWaitingHumanCount(population);

    }
    public override void StartMove(Vector2 spawn_point, bool going_left)
    {
        layermask = 1 << gameObject.layer; // ���� ������Ʈ�� ���� ���̾�͸� �浹
        rigid.position = appear_point;

        if (gameObject.activeSelf)
            move_coroutine = StartCoroutine(MoveToEndLine());
    }
    public override void ReturnHuman()
    {
        StopAllCoroutines();
        Gamemanager.Instance.objectpool.ReturnSick(this);
    }

    public override IEnumerator ActAtHD(HospitalDoor hd)
    {
        Gamemanager.Instance.buildgame.PlusGoldFromHuman(this, hd.earn_things * 2);

        StopCoroutine(move_right);
        yield return StartCoroutine(Fadeout());
        yield return new WaitForSeconds(3f);

        destinationfloor_script.StartCoroutine(destinationfloor_script.MakeHuman());
        ReturnHuman();
    }
}