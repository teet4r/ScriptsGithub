using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleaner_Info : MonoBehaviour
{
    H_Cleaner cleaner; // ���� ������ ������ �ִ� ����� ��ũ��Ʈ

    public Text employee_name; // �̸�
    public Text employee_birth_place; // �����
    public Text employee_state; // ������
    public Text employee_career; // ���
    public int career;
    public Text employee_cleanup_floors_count; // �����
    public Text employee_proficiency; // ���õ�(�Ͽ� ����)
    public Text level; // ������ �ϴ��� ǥ��
    public Text promotion_gold; // ������ �ʿ��� ���
    public int promotion_gold_size;

    public Button fire_btn;         // �ذ��ư
    public Button promotion_btn;    // ������ư

    int id;

    public void SettingCleanerInfo(H_Cleaner cleaner) // ���� ����
    {
        // id �ο�
        this.id = cleaner.id;
        cleaner.cleaner_info = this;
        this.cleaner = cleaner; // ��ũ��Ʈ ����ȭ

        promotion_gold_size = 200;
        promotion_gold.text = promotion_gold_size + " G";

        // ���� �̸� ����
        employee_name.text = Gamemanager.Instance.employeemanager.first_name[Random.Range(0, Gamemanager.Instance.employeemanager.first_name.Length - 1)] 
                     + " " + Gamemanager.Instance.employeemanager.second_name[Random.Range(0, Gamemanager.Instance.employeemanager.second_name.Length - 1)];

        // ���� ���� �ο�
        // �����
        employee_birth_place.text = "����� : " + Gamemanager.Instance.employeemanager.birth_place[Random.Range(0, Gamemanager.Instance.employeemanager.birth_place.Length - 1)];
        // �Ի糯
        employee_state.text = "������ : �����";
        // ���
        employee_career.text = "����ϼ� : " + career + "��";
        // û�Ҹ� �Ϸ��� �� ��
        employee_cleanup_floors_count.text = "û��Ƚ�� : " + cleaner.cleanup_floors_count + "��";
        // ���õ�
        employee_proficiency.text = "���õ� : " + Mathf.Floor(cleaner.cleaning_speed * 100) * 0.01;

        if (Gamemanager.Instance.buffmanager.is_iron_craw_on) // ����� ö������ ���� ���̸� �ذ��ư ��� ����
            fire_btn.enabled = false;
    }
    public void Promotion()
    {
        if (Gamemanager.Instance.buildgame.gold >= promotion_gold_size)
        {
            Gamemanager.Instance.buildgame.PlusGold(-promotion_gold_size);

            // pay 30% ����
            cleaner.pay = (int)(1.3 * cleaner.pay);

            // ���õ� 70% ����
            cleaner.cleaning_speed *= 1.7f;
            employee_proficiency.text = "���õ� : " + Mathf.Floor(cleaner.cleaning_speed * 100) * 0.01;

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
        cleaner.pay = (int)(1.5 * cleaner.pay);
        fire_btn.enabled = false;
    }
    public void Fire() // �� �ذ��!
    {
        if (Gamemanager.Instance.buffmanager.is_severance_pay_on)
            Gamemanager.Instance.buildgame.PlusGold(-100 * (career + 1) * (cleaner.cleanup_floors_count + 1));

        Gamemanager.Instance.employeemanager.FireCleaner(id);
        Destroy(gameObject);
    }
}
