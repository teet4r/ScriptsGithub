using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acrophobia : Debuff
{
    public string debuff_name { get; } = "��Ұ�����";
    public string debuff_explain { get; } = "���� ū ������ �����ٴ� 11���� ��¼��...";
    public string debuff_effect { get; } = "�ǹ� �����̻� �ö󰡸� ���������Ͱ� �������ϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.acrophobia_speed_rate -= 0.4f;
    }
}
