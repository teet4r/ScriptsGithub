using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinMissing : Debuff
{
    public string debuff_name { get; } = "���������� ����";
    public string debuff_explain { get; } = "���ϳ� ���̾�~";
    public string debuff_effect { get; } = "���� ���� ������ ���������ϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.dirty_increase_rate += 0.3f;
    }
}
