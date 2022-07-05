using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoreanSpeed :Debuff
{
    public string debuff_name { get; } = "�ѱ��� �ӵ�";
    public string debuff_explain { get; } = "���� �̰� �а� ���� �ð��� �����ٵ���?";
    public string debuff_effect { get; } = "���� �ӵ��� 3��� �����ǰ� �Ͻ������� ������ϴ�";
    public void DebuffOn()
    {
        Gamemanager.Instance.buildgame.timespeed = 3;
        Gamemanager.Instance.buffmanager.is_korean_speed = true;
        Gamemanager.Instance.uimanager.KoreanSpeedOn();
        Time.timeScale = Gamemanager.Instance.buildgame.timespeed;
    }
}
