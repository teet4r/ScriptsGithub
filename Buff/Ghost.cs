using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost :Buff
{
    public string buff_name { get; } = "��üȭ";
    public string buff_explain { get; } = "�ȿ� �ִ� ����� ��� �Ǵ°���?";
    public string buff_effect { get; } = "���������͵��� ���̻� ���� �浹���� �ʽ��ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.is_ghost_elevator = true;
    }
}
