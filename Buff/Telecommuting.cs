using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telecommuting : Buff
{
    public string buff_name { get; } = "���ñٹ�";
    public string buff_explain { get; } = "Ư) ���Ӱ���";
    public string buff_effect { get; } = "����ϴ� ����� ���� �پ��ϴ�";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.go_to_work_rate--; 
    }
}
