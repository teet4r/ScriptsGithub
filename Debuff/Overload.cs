using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overload :Debuff
{
    public string debuff_name { get; } = "������� ����� ����";
    public string debuff_explain { get; } = "�� �����ڸ��� �̵��Ͻð� �� ��������";
    public string debuff_effect { get; } = "������ �� �ӵ��� ���� �����մϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.overload_speed_rate -= 0.3f;
    }
}
