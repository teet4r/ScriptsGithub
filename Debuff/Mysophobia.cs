using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mysophobia :  Debuff
{
    public string debuff_name { get; } = "�Ắ��";
    public string debuff_explain { get; } = "�ҵ� ���հ�, ���� ���հ�, �ƹ�ư ���հ�";
    public string debuff_effect { get; } = "�������� �� �Ĳ��� ���ϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.sanitation_check_size -= 20;
        if (Gamemanager.Instance.buffmanager.sanitation_check_size < 20)
            Gamemanager.Instance.buffmanager.sanitation_check_size = 0;
    }
}
