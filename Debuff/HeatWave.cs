using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : Debuff
{
    public string debuff_name { get; } = "����";
    public string debuff_explain { get; } = "100�� ���� �־��� ���� ��¼��";
    public string debuff_effect { get; } = "���Ǳ� �� �������� �Ҹ��� �����մϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.use_size_of_bevarage += 1;
    }
}
