using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaddyAwake :  Buff
{
    public string buff_name { get; } = "�ƺ� ���ܴ�";
    public string buff_explain { get; } = "�и� ���� ���� �����ٵ�?";
    public string buff_effect { get; } = "���尡 �ڷε� �� �� �ֽ��ϴ�. //���� ���� X//";
    public void BuffOn()
    {
        //Gamemanager.Instance.buffmanager.vvvvvip_speed_rate = 3f;
    }
}
