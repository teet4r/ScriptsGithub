using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject menu, checkbox; // �޴���(����, ��������, �����), üũ�ڽ�(yes or no)
    public List<GameObject> child_panels; // �ڽ� �гε� �ڵ����� ����
    string dir; // ������ = Scene (Main or Ingame)
    public Text text;
    bool is_pause_pushed = false; // pause�� ���� ���¸� true
    public void Push()
    {
        if (!is_pause_pushed) // �� ���¿��� �ٽ� ������ ����
        {
            Gamemanager.Instance.uimanager.TimeStop();

            if (!Gamemanager.Instance.buffmanager.is_korean_speed)
                text.text = "��";

            is_pause_pushed = true;
            menu.SetActive(true);
        }
        else // �� ���¿��� �ٽ� ������ ���
        {
            Gamemanager.Instance.uimanager.TryTimeRestart();

            text.text = "I I";

            foreach(var cp in child_panels)
                cp.SetActive(false);

            is_pause_pushed = false;
            menu.SetActive(false);
        }
    }
    public void PushToMain()
    {
        dir = "Menu";
        checkbox.SetActive(true);
    }
    public void PushToRestart()
    {
        dir = "Ingame";
        checkbox.SetActive(true);
    }
    public void PushYes()
    {
        Gamemanager.Instance.buildgame.OnClickLoadScene(dir);
    }
    public void PushNo()
    {
        checkbox.SetActive(false);
    }
}