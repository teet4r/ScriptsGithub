using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMoveUp :  Buff
{
    public string buff_name { get; } = "��ī";
    public string buff_explain { get; } = "��Ʃ�� ��ī �����.";
    public string buff_effect { get; } = "���������Ͱ� ������ �Ϸ� ���� �ӵ��� �������ϴ�.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_speed_rate_for_fix += 0.2f;
    }
}
