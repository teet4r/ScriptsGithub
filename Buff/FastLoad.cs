using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastLoad :  Buff
{
    public string buff_name { get; } = "���� ž��";
    public string buff_explain { get; } = "�Ϸ� ����� ſ!";
    public string buff_effect { get; } = "������� ���������Ϳ� Ÿ�� �ӵ��� �����մϴ�.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_on_speed_rate += 0.2f;
    }
}
