using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyElevator : Buff
{
    public string buff_name { get; } = "������ ����������";
    public string buff_explain { get; } = "�� ���� ������ �ٴϴ� �����̴� ���� ����";
    public string buff_effect { get; } = "���������Ϳ� ����� ������ �ӵ��� ���� �����մϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.empty_elevator_speed_rate += 1f;
    }
}
