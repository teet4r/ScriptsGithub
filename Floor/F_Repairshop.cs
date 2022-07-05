using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_Repairshop : Floor
{
    public List<bool> is_line_repair_using; // �� ���ο��� ���ÿ� ���� �� �� �ִ� ���������ʹ� �� �� ����
                                            // �ش� ������ ���� ���̶�� true

    public List<GameObject> repair_point; // �� ���δ� ���� ��ġ
    public override void Set(int floor_level, float min_respawntime = 10, float max_respawntime = 15, int max_wait_cnt = 5)
    {
        floor_color = FID.REPAIRSHOP;
        dirty_index = 0;
        is_cleaner_called = false;

        is_line_repair_using = new List<bool>(3) { false, false, false };

        this.floor_level = floor_level;
        this.min_respawntime = min_respawntime;
        this.max_respawntime = max_respawntime;
        this.max_wait_cnt = max_wait_cnt;
    }
    public void Call_Mechanic(ElevatorClass elevator, H_Mechanic mechanic_script) // �����ؾ��ϴ� ������������ ��ü
    {
        mechanic_script.elevator = elevator;
        mechanic_script.gameObject.SetActive(true);
        mechanic_script.Set(this);
    }
}
