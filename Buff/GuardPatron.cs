using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardPatron : Buff
{
    int count = 1;
    public string buff_name { get; } = "���� 5����";
    public string buff_explain { get; } = "�����ϰ� �˽η�������";
    public string buff_effect { get; } = "������ �̵��ӵ��� �����մϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.patron_speed_rate += Mathf.Pow(0.5f, count++);
        Gamemanager.Instance.employeemanager.PatronSpeedUp();
    }
}
