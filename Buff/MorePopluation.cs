using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorePopulation :  Buff
{
    public string buff_name { get; } = "���ö��̵���";
    public string buff_explain { get; } = "Additional supply depots required.";
    public string buff_effect { get; } = "3�ϸ��� �þ�� �α� �Ѱ�ġ�� �����մϴ�.";
    public void BuffOn()
    {
        Gamemanager.Instance.buffmanager.supply_size += 4;
    }
}
