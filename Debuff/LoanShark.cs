using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoanShark :  Debuff
{
    public string debuff_name { get; } = "��ä����";
    public string debuff_explain { get; } = "���� �������� �̸��̾�";
    public string debuff_effect { get; } = "3000G�� ������ �޽��ϴ�. 20�ϰ� 300��徿 �ڵ����� �����ϴ�. ���� �� ���ٸ� ���ӿ��� �˴ϴ�.";
    public void DebuffOn()
    {
        Gamemanager.Instance.buildgame.StartDebuffLoanShark();
    }
}
