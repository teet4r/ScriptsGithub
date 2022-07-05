using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached on Main Camera
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;    //ĵ����

    [SerializeField]
    Button multiple_btn; //��ӹ�ư
    [SerializeField]
    Button pause_btn; // �Ͻ�������ư

    [SerializeField]
    GameObject explain_panel;
    public int stop_panels_count { get; set; }

    ExplainText explain_text;

    public bool is_explain_on; // �ΰ��� ���� ��� ���� , true��� ����â�� ����
    public bool is_bgm_on; 
    public bool is_effectsound_on;

    void Awake()
    {
        stop_panels_count = 0;
    }
    void Start()
    {
        explain_text = new ExplainText();
    }
    public void KoreanSpeedOn()
    {
        multiple_btn.GetComponent<MultipleSpeed>().multiple_text.text = "X3";
        multiple_btn.enabled = false;
    }
    public void TryTimeRestart() // ��� ������ε� ���� �ӵ��� �����·� ������
    {
        stop_panels_count--;
        if (stop_panels_count == 0)
            Time.timeScale = Gamemanager.Instance.buildgame.timespeed;
    }
    public void TimeStop() // ���� �ӵ� ����
    {
        if (!Gamemanager.Instance.buffmanager.is_korean_speed)
        {
            stop_panels_count++;
            Time.timeScale = 0f;
        }
    }
    public void ShowExplain(ref bool is_appear, int explain_idx) // ���� ������ ������ ������ �ִ���, ���� �ε���
    {
        if (!is_appear && is_explain_on) // Ÿ�̸� ����â ����
        {
            TimeStop();
            GameObject ex_panel = Instantiate(explain_panel, canvas.transform);
            ex_panel.GetComponent<ExplainPanel>().Set(explain_text.explain_arr[explain_idx]);
            is_appear = true;
        }
    }
}
public class ExplainText
{
    public int man_size = 15;
    public string[][] explain_arr;

    public ExplainText()
    {
        explain_arr = new string[man_size][];

        for (int i = 0; i < man_size; i++) 
            explain_arr[i] = new string[3];

        explain_arr[PID.THIEF][0] = "����";
        explain_arr[PID.THIEF][1] = "����� ������ �ٴ��� ���ƴٴ±���.";
        explain_arr[PID.THIEF][2] = "�������� �����ϸ� ������ ��带 �����ϴ�. ���࿡�� �� ���� ���� �ҽ��ϴ�. ��� ��ġ�Ͻʽÿ�. �㿡 ����մϴ�.";

        explain_arr[1][0] = "����";
        explain_arr[1][1] = "ȿ�������� ���������͸� ��ġ�ϼ���!";
        explain_arr[1][2] = "Ÿ�̸��� �ð��� �ٵǸ� �ش� ������ ��� ������ ����� ���� �����˴ϴ�.";

        explain_arr[PID.SANITATION][0] = "������";
        explain_arr[PID.SANITATION][1] = "�Ʒü� ���� ����̶� �ϳ׿�.";
        explain_arr[PID.SANITATION][2] = "�ǹ��� �̵��ϸ鼭 �����˻縦 �մϴ�. ������ ������ �����Ͻʽÿ�.";

        explain_arr[PID.HOMELESS][0] = "�����";
        explain_arr[PID.HOMELESS][1] = "������ �븮�� �ٴ��� ��ȣ�մϴ�.";
        explain_arr[PID.HOMELESS][2] = "���� �ִ� ���� �ֱ������� �������� �߰��մϴ�. �ൿ�� �� ���� ���� ���� �������� �߰��մϴ�. ������ �ڱ⵵ �մϴ�.";

        explain_arr[PID.SICK][0] = "���ߵ�";
        explain_arr[PID.SICK][1] = "�̷� ����� ����� ����׿�";
        explain_arr[PID.SICK][2] = "Ư�� ��ȣ��� �� ���� Ȯ���� ��Ż�� �߻��մϴ�. �������� �̼��ϼ���.";
    }
}
