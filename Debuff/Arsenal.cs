using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsenal :  Debuff
{
    public string debuff_name { get; } = "Arsenal";
    public string debuff_explain { get; } = "�౸���� ������ ������ �� ������, �׳� �׷� ���� �����Ѵ�.";
    public string debuff_effect { get; } = "�� ���� ���� 3������ 4���� �����մϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.plus_floor_size += 1;
    }
}
