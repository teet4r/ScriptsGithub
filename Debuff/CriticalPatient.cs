using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalPatient :Debuff
{
    public string debuff_name { get; } = "��ȯ��";
    public string debuff_explain { get; } = "�׷��� ��å�� ������ ����";
    public string debuff_effect { get; } = "ȯ���� �̵��ӵ��� ������ �������ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.patient_speed_rate = -0.5f;
    }
}
