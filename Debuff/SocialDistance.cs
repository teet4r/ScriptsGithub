using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialDistance :  Debuff
{
    public string debuff_name { get; } = "��ȸ�� �Ÿ��α�";
    public string debuff_explain { get; } = "Good~ Good~ Stay right there";
    public string debuff_effect { get; } = "���������� ���뷮�� �������� �����մϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.elevator_volume_half = 2;
        Gamemanager.Instance.elevatormanager.HalfVolume();
    }
}
