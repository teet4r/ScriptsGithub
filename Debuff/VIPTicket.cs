using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VIPTicket : Debuff
{
    public string debuff_name { get; } = "VIP Ƽ��";
    public string debuff_explain { get; } = "���� �� ���� �����̶� ��";
    public string debuff_effect { get; } = "VIP���� �ü��� �̿�� ����� ���� �ʽ��ϴ�. ȸ���� ���� �ȳ��ϴ�. �װ��� ȸ���̴ϱ�.";

    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.vip_ticket = 0;
    }
}
