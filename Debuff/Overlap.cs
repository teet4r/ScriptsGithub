using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap :  Debuff
{
    public string debuff_name { get; } = "�ߺ�";
    public string debuff_explain { get; } = "���";
    public string debuff_effect { get; } = "�������� ������ ������� �ߺ��ؼ� ���� �� �ֽ��ϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_overlap_on = true;
    }
}
