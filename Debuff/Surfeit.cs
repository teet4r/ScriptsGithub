using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Surfeit :  Debuff
{
    public string debuff_name { get; } = "����";
    public string debuff_explain { get; } = "�� �� ��ġ ���ϳ�";
    public string debuff_effect { get; } = "���� ����� ������ Ȯ���� �����մϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.fat_human_appear_rate += 0.2f;
    }
}
