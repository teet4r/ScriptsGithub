using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddhaThief :  Debuff
{
    public string debuff_name { get; } = "��ó ����";
    public string debuff_explain { get; } = "߲Ҵ��ޫ����ٰ";
    public string debuff_effect { get; } = "������ �� �����ð� ��ٸ� �� �ֽ��ϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.thief_patience_rate += 1;
    }
}
