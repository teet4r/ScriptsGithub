using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darktempler : Debuff
{
    public string debuff_name { get; } = "��ũ���÷�";
    public string debuff_explain { get; } = "�Ƶ� �丮�ٽ�";
    public string debuff_effect { get; } = "�㿡 ������ ���� ���� �����˴ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.thief_appear_in_night_rate += 0.15f;
    }
}
