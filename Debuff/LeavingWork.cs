using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingWork : Debuff
{
    public string debuff_name { get; } = "Į!��!";
    public string debuff_explain { get; } = "������ �� ���� ����";
    public string debuff_effect { get; } = "��� �ð��� ������� ���� ���� ����ϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.go_to_home_rate += 1.5f;
    }
}
