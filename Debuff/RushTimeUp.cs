using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushTimeUp :  Debuff
{
    public string debuff_name { get; } = "������Ʈ ����";
    public string debuff_explain { get; } = "�׸� �� ����";
    public string debuff_effect { get; } = "���� ���ӽð��� �����մϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.rush_time_size += 0.5f;
    }
}
