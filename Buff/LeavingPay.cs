using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingPay : Buff
{
    public string buff_name { get; } = "��ٺ�";
    public string buff_explain { get; } = "���� ����";
    public string buff_effect { get; } = "1������ ������ ������� ��带 �����մϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.go_to_home_pay_size += 10;
    }
}
