using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPoisoning :  Debuff
{
    public string debuff_name { get; } = "���ߵ�";
    public string debuff_explain { get; } = "������ �����⿡ ���� Ǯ����!";
    public string debuff_effect { get; } = "�Ĵ翡�� �谡 ���»���� ���� ���� ����ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.illed_human_rate += 0.3f;
    }
}
