using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkOvertime :  Debuff
{
    public string debuff_name { get; } = "�̾� �߱��̴�";
    public string debuff_explain { get; } = "�� ���ο� ¥����";
    public string debuff_effect { get; } = "�� �ð��� ������� �� �̻� �پ���� �ʽ��ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.human_create_in_night_rate = 1;
    }
}
