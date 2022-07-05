using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBowl : Debuff
{
    public string debuff_name { get; } = "ö����";
    public string debuff_explain { get; } = "�� ���� ö����";
    public string debuff_effect { get; } = "��� �뵿���� �ӱ��� �����ϸ� �ذ� �� �� �������ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_iron_craw_on = true;
        Gamemanager.Instance.buffmanager.pay_increase_rate = 0.3f;
        Gamemanager.Instance.employeemanager.EmployeesPayUp();
    }
}
