using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerTip : Debuff
{
    public string debuff_name { get; } = "û�Һ�";
    public string debuff_explain { get; } = "���� �� �ǹ����� ���?";
    public string debuff_effect { get; } = "û�Һΰ� û������ ���� �޽��ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.cleaner_pay_size = -30;
    }
}
