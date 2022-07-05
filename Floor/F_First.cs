using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_First : Floor
{
    public float thief_threshold = 0.1f; // 0 ~ 1�� ����� �̾��� �� ���ϳ��� �Ǵ� ���Ѽ� ��
    public float fat_threshold = 0.2f; // �� ���� Ȯ�� ���Ѽ� ��
    public float vip_threshold = 0.5f; // vip�� ���� ������ �����Ǵ� ���Ѽ� ��

    public int delivery_destination;

    Coroutine go_to_work_coroutine;

    public override void Set(int floor_level = 1, float min_respawntime = 1f, float max_respawntime = 2f, int max_wait_cnt = 5)
    {
        floor_color = FID.FIRST;
        dirty_index = 0;
        is_cleaner_called = false;

        this.floor_level = floor_level;
        floor_level_text.text = floor_level.ToString() + "F";
        this.min_respawntime = min_respawntime;
        this.max_respawntime = max_respawntime;
        this.max_wait_cnt = max_wait_cnt;
    }
    public IEnumerator MakeHumanInMorning() // ��ٿ�
    {
        while (Gamemanager.Instance.buildgame.is_go_to_work_time)  // ��ٽð� ���ȸ� ����
        {
            yield return new WaitForSeconds(Random.Range(1, 6));

            if (Random.Range(0f, 1f) < 0.05f)  // 5%
                Gamemanager.Instance.objectpool.GetVip(vip_threshold).GetComponent<Human>().Set(this);
            else //95%
                Gamemanager.Instance.objectpool.GetNormal(Random.Range(FID.RED, FID.YELLOW + 1), fat_threshold).GetComponent<Human>().Set(this);
        }
    }
    public IEnumerator MakeThief() // �� 22 ~ 7
    {
        while (Gamemanager.Instance.buildgame.is_night)  // ��ð� ���ȸ� ����
        {
            yield return new WaitForSeconds(Random.Range(1,3));

            if (Random.Range(0,1f) <= thief_threshold + Gamemanager.Instance.buffmanager.thief_appear_in_night_rate)
                Gamemanager.Instance.objectpool.GetThief().GetComponent<Human>().Set(this);
        }
    }
    public IEnumerator MakeDelivery(int event_object_kind, int event_object_id, int need_item,Floor des_floor) // ��޺� ����, �̺�Ʈ ������Ʈ�� �ִ� ��, �ʿ��� ������ 
    {

        yield return new WaitForSeconds(Random.Range(1, 3));
        var human = Gamemanager.Instance.objectpool.GetDelivery().GetComponent<Human>();
        (human as H_Delivery).SetItem(event_object_kind, event_object_id,need_item,des_floor);
        human.Set(this);
    }
    public IEnumerator MakeSanitation() // ���������� ����(���� ����? �ƴϸ� ���ߵ� ȯ�ڰ� ����� ����? 1������? ��������?)
    {
        while (true) 
        {
            yield return new WaitForSeconds(Random.Range(50, 60));

            Gamemanager.Instance.objectpool.GetSanitation().GetComponent<Human>().Set(this);
        }
    }
    public IEnumerator MakeHomeless() // ����� ���� 1������, �ƹ��볪
    {
        while (true)  
        {
            yield return new WaitForSeconds(Random.Range(30, 40));

            Gamemanager.Instance.objectpool.GetHomeless().GetComponent<Human>().Set(this);
        }
    }
    public override void SetDirtyRate(float dirty_size)
    {
        dirty_index += dirty_size;
        if (dirty_index > 100) dirty_index = 100;
        if (dirty_index < 0) dirty_index = 0;
        dirty_text.text = ((int)dirty_index).ToString();
        dirty_text_box.color = new Color(1, (100-dirty_index)/100, (100-dirty_index)/100);

        dirty_bar.transform.localScale = new Vector2(1, dirty_index / 100);
        if (dirty_index > Gamemanager.Instance.buildingmanager.cleaner_limit_size && !is_cleaner_called)
            StartCoroutine(CallClean());
    }
}
