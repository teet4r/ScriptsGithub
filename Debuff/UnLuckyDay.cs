using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnLuckyDay :  Debuff
{
    public string debuff_name { get; } = "��� ���� ��";
    public string debuff_explain { get; } = "�������� �� ��Ծ�!";
    public string debuff_effect { get; } = "������ ������� �ϳ� �� �����մϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.unlucky_day_count++;
    }
}
