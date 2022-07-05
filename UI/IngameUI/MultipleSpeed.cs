using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleSpeed : MonoBehaviour
{
    public Text multiple_text;
    public void Push()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = (Time.timeScale + 1) % 4; // 1�� 2�� 3��
            if (Time.timeScale == 0) Time.timeScale = 1;

            multiple_text.text = "X" + Time.timeScale;

            Gamemanager.Instance.buildgame.timespeed = Time.timeScale;
        }
    }
}
