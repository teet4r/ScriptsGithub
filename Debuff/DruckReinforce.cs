using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DruckReinforce :  Debuff
{
    public string debuff_name { get; } = "���� ����";
    public string debuff_explain { get; } = "���� ��������~";
    public string debuff_effect { get; } = "���� ���� ������ ���߰� �������� ������ �� �ֽ��ϴ�(10���� �۾����� �ʽ��ϴ�)";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.reinforce_fail_threshold = 1;
    }
}
