using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateRush :  Buff
{
    public string buff_name { get; } = "���峭 �ڸ���";
    public string buff_explain { get; } = "�ð��� ������ ���� �� ������?";
    public string buff_effect { get; } = "���� ������ �����մϴ�.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.rush_distance_size += 0.5f;
    }
}
