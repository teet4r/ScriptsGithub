using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mechanic_Info : MonoBehaviour
{
    H_Mechanic mechanic; // ���� ������ ������ �ִ� ����� ��ũ��Ʈ

    public Text employee_name; // �̸�
    public Text employee_birth_place; // �����
    public Text employee_date_of_join; // �Ի糯¥
    public Text employee_career; // ���
    public int career;
    public Text employee_fix_elevator_count; // ���� Ƚ��
    public Text employee_proficiency; // ���õ�(�Ͽ� ����)
    public Text level; // ������ �ϴ��� ǥ��
    public Text promotion_gold; // ������ �ʿ��� ���
    public int promotion_gold_size;

    public Button fire_btn;         // �ذ��ư
    public Button promotion_btn;    // ������ư

    int id;

    public void SettingMechanicInfo(H_Mechanic mechanic) // ���� ����
    {
        // id �ο�
        this.id = mechanic.id;
        mechanic.mech_info = this;
        this.mechanic = mechanic; // ��ũ��Ʈ ����ȭ

        promotion_gold_size = 300;
        promotion_gold.text = promotion_gold_size + " G";
        
        // ���� �̸� ����
        employee_name.text = Gamemanager.Instance.employeemanager.first_name[Random.Range(0, Gamemanager.Instance.employeemanager.first_name.Length - 1)]
                     + " " + Gamemanager.Instance.employeemanager.second_name[Random.Range(0, Gamemanager.Instance.employeemanager.second_name.Length - 1)];

        // ���� ���� �ο�
        // �����
        employee_birth_place.text = "����� : " + Gamemanager.Instance.employeemanager.birth_place[Random.Range(0, Gamemanager.Instance.employeemanager.birth_place.Length - 1)];
        // �Ի糯
        employee_date_of_join.text = "�Ի��� : " + Gamemanager.Instance.buildgame.day + "��";
        // ���
        employee_career.text = "����ϼ� : " + career + "��";
        // û�Ҹ� �Ϸ��� �� ��
        employee_fix_elevator_count.text = "����Ƚ�� : " + mechanic.fix_elevator_count + "��";
        // ���õ�
        employee_proficiency.text = "���õ� : " + mechanic.fixing_speed;

        if (Gamemanager.Instance.buffmanager.is_iron_craw_on) // ����� ö������ ���� ���̸� �ذ��ư ��� ����
            fire_btn.enabled = false;
    }
    public void Promotion()
    {
        if (Gamemanager.Instance.buildgame.gold >= promotion_gold_size)
        {
            Gamemanager.Instance.buildgame.PlusGold(-promotion_gold_size);

            // pay 30% ����
            mechanic.pay = (int)(1.3 * mechanic.pay);

            // ���õ� 80% ����
            mechanic.fixing_speed = (int)(1.15f * mechanic.fixing_speed);
            employee_proficiency.text = "���õ� : " + Mathf.Floor(mechanic.fixing_speed * 100) * 0.01;

            // ���� ��� 50% ����
            promotion_gold_size = (int)(1.5 * promotion_gold_size);
            promotion_gold.text = promotion_gold_size + " G";
        }
    }
    public void PlusCareer()
    {
        employee_career.text = "����ϼ� : " + ++career + "��";
    }
    public void IronBowl()
    {
        mechanic.pay = (int)(1.5 * mechanic.pay);
        fire_btn.enabled = false;
    }
    public void Fire() // �� �ذ��!
    {
        if (Gamemanager.Instance.buffmanager.is_severance_pay_on)
            Gamemanager.Instance.buildgame.PlusGold(-100 * (career + 1) * (mechanic.fix_elevator_count+ 1));

        Gamemanager.Instance.employeemanager.FireMechanic(id);
        Destroy(gameObject);
    }
}
