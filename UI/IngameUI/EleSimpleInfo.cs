using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EleSimpleInfo : MonoBehaviour
{
    public Text level_txt;
    public Text name_txt;
    public Text state_txt; // ������ �� �� �� �ش� ��ư�� �̿� ���Ѵٴ� ������ �����ִ� ��
    public Text auto_state_txt; // �ڵ� ���� ���̸� �ʷϻ�, �ƴϸ� ȸ��
    public Image waring_img;
    public Image durability_bar_img;
    public Image screen_img; // ������ �̹���, ��ư�� �̿�Ұ��� �� �����

    ElevatorClass elevatorclass_script;

    public void Init(ElevatorClass elevatorclass_script)
    {
        this.elevatorclass_script = elevatorclass_script;
        elevatorclass_script.elevator_info = this;

        level_txt.text = "Lv" + 1;
        name_txt.text = elevatorclass_script.gameObject.name; // �ִ� 6����
        screen_img.enabled = false;
        state_txt.text = "";
        auto_state_txt.color = Color.gray;
    }

    //======================================�ǽð����� ���ϴ� ����======================================================
    public void ChangeLevel()
    {
        // ������ �ö󰥶����� ����
        level_txt.text = "Lv" + elevatorclass_script.level;
    }
    public void ChangeName()
    {
        // �̸��� �ٲ𶧸��� ����
        name_txt.text = elevatorclass_script.gameObject.name; // �ִ� 5����(�ѱ۱���, ����� �� �� �ɵ�)
    }
    public void ChangeAuto()
    {
        // ���� ��ư�� ���������� ����
        auto_state_txt.color = (elevatorclass_script.is_automating) ? Color.green : Color.gray;
    }
    public void ShowIsFixing(bool is_fixing) // ������ �Ϸ� �����̴� ���ΰ�?
    {
        // ���������Ͱ� ������ư�� ���� �� ���� �����ϰ� �����ϱ� �������� ����
        if (is_fixing) // ���� ���������Ͱ�  ���� ����(���� ��ư Ŭ������ ���� �� ���ͱ���)��� ���� ���� �� �� ����
        {
            state_txt.text = "����";
            //button.enabled = false;
            screen_img.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); // ������ ȸ��
        }
        else
        {
            state_txt.text = "";
            //button.enabled = true;
            screen_img.color = new Color(0, 0, 0, 0); // ����
        }
    }
    public void ShowDurabilityProgress()
    {
        //����ҿ��� ���� ���϶� Ȥ�� �������� �Ҹ�ɶ� ������ ��ȯ ������
        durability_bar_img.fillAmount = (elevatorclass_script.cur_durability / elevatorclass_script.max_durability);

        if ((elevatorclass_script.cur_durability / elevatorclass_script.max_durability) > 0.5) // ������ 50% �̻�
            durability_bar_img.color = new Color(
                -2 * ((elevatorclass_script.cur_durability / elevatorclass_script.max_durability) - 1),
                1,
                0);
        else // ������ 50% �̸�
            durability_bar_img.color = new Color(
                1,
                2 * (elevatorclass_script.cur_durability / elevatorclass_script.max_durability),
                0);
    }
    public void ShowWarningState()
    {
        waring_img.color =
        (
            //1. ������
            elevatorclass_script.is_overload ||
            //2. ���� ž�� << ���� ����ȵ�
            elevatorclass_script.thief_cnt > 0 ||
            //3. ���������� �ļ�
            elevatorclass_script.cur_durability == 0
        //3. etc(�̱���)
        )
        ? Color.red : Color.gray;
    }

    public void Push()
    {
        Gamemanager.Instance.elevatormanager.managepanel.SetActive(true);
        Gamemanager.Instance.elevatormanager.managepanel.GetComponent<Management_detail>().Setting(elevatorclass_script);
        Camera.main.GetComponent<CameraMove>().SelectElevator(elevatorclass_script); // ī�޶� ����ٴ� ���������͸� �Ѱ���
    }
}
