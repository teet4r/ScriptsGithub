using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoldWall :Debuff
{
    public string debuff_name { get; } = "�����̺���";
    public string debuff_explain { get; } = "��� ������ ����!!!";
    public string debuff_effect { get; } = "���ϰ� �������� ���� ������� �̿��Ҷ����� ���� �ҽ��ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buffmanager.mold_pay_size = -10;
    }
}
