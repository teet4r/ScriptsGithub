using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsKim :Buff
{
    public string buff_name { get; } = "�迩��";
    public string buff_explain { get; } = "�ƴ� ���� ���������Ϳ� �ڵ��� �޾Ƴ���?";
    public string buff_effect { get; } = "ž�� ���� vip���� ���� ����Ͽ� �ӵ��� ���� �����մϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.vip_female_speed_rate += 0.05f;
    }
}
