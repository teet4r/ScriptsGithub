using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverTimeMeal :Debuff
{
    public string debuff_name { get; } = "�߽�����";
    public string debuff_explain { get; } = "�߽� ���� ���� ���";
    public string debuff_effect { get; } = "24�ú��� 3�ñ��� �Ĵ��̿��� ���������ϴ�";
    public void DebuffOn()
    {
       Gamemanager.Instance.buffmanager.is_there_night_rest = true;
    }
}
