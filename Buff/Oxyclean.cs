using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxyclean : Buff
{
    public string buff_name { get; } = "�����Ͻ� ��Xũ��";
    public string buff_explain { get; } = "������, ������, �⸧���� �ѹ濡!";
    public string buff_effect { get; } = "û�Һ��� û�� ȿ���� �����մϴ�";
    public void BuffOn()
    {
         Gamemanager.Instance.buffmanager.cleaning_rate += 0.2f;
    }
}
