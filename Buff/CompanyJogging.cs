using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanyJogging :Buff
{
    public string buff_name { get; } = "�系����";
    public string buff_explain { get; } = "��ø��� �������� Ż�Կ�~";
    public string buff_effect { get; } = "������� �ӵ��� �������ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.human_speed_rate += 0.3f;
    }

}
