using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HongGilDong :  Debuff
{
    public string debuff_name { get; } = "ȫ�浿";
    public string debuff_explain { get; } = "���� ��½! ���� ��½! �� ���� ��½!";
    public string debuff_effect { get; } = "������ ���� ���� �߹����� ���İ��ϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.thief_proficiency_increase_rate += 0.15f;
    }
}
