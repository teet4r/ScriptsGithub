using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EleVIRItor : Debuff
{
    public string debuff_name { get; } = "���� \"��\" ��";
    public string debuff_explain { get; } = "���� ���������͸� ������ �������?";
    public string debuff_effect { get; } = "������������ �������� ������ ���� �����մϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.durability_decrease_rate = 4f;
    }
}
