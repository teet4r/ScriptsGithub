using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayHealing : Buff
{
    public string buff_name { get; } = "�޽� = ����";
    public string buff_explain { get; } = "����...��...";
    public string buff_effect { get; } = "�޽�Ȱ���� �ϸ� ü���� ���� �� ȸ���˴ϴ�.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.healing_size += 3; 
    }
}
