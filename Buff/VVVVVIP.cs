using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VVVVVIP :  Buff
{
    public string buff_name { get; } = "VVVVVIP";
    public string buff_explain { get; } = "��...���������Ͱ� �� ���� �� ����?";
    public string buff_effect { get; } = "ȸ���� �¿�� �ӵ��� �ް��ϰ� �������ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.vvvvvip_speed_rate = 3f;
    }
}
