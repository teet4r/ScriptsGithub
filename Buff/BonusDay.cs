using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDay :  Buff
{
    public string buff_name { get; } = "������ ���ʽ� �޴� ��";
    public string buff_explain { get; } = "�������������� ���޵� �ֳ�?";
    public string buff_effect { get; } = "��� ���������Ϳ��� ����Ʈ�� 3���� �߰��˴ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.elevatormanager.PlusPoint();
    }
}
