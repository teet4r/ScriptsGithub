using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyDelivery :  Debuff
{
    public string debuff_name { get; } = "���ſ� �ù�";
    public string debuff_explain { get; } = "��ټ� 20�� ��Ű�� ��";
    public string debuff_effect { get; } = "��޺��� �̵��ӵ��� �����մϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.delivery_speed_rate -= 0.5f;
    }
}
