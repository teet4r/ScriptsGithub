using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : Buff
{
    int count = 1;
    public string buff_name { get; } = "�߷�";
    public string buff_explain { get; } = "���������͵� �߷��� ����!";
    public string buff_effect { get; } = "���������Ͱ� �ö� ���� ���������� ������ ���� �������ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.gravity_elevator_rate += Mathf.Pow(0.5f, count++);
    }
}
