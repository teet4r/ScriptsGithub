using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeverancePay :  Debuff
{
    public string debuff_name { get; } = "������";
    public string debuff_explain { get; } = "���� ��ŭ �޾ƾ���";
    public string debuff_effect { get; } = "�ذ� �� �� Ƚ��, ��� ��¥�� ���� ���� �����մϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.is_severance_pay_on = true;
    }
}
