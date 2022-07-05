using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guard_Info : MonoBehaviour
{
    H_Guard guard; // ���� ������ ������ �ִ� ����� ��ũ��Ʈ

    public Text employee_name; // �̸�
    public Text employee_birth_place; // �����
    public Text employee_state; // ����
    public Text employee_career; // ���
    public int career;
    public Text employee_caught_thief_count; // ���� ���� ��
    public Text employee_proficiency; // ���õ�(�Ͽ� ����)
    public Text level; // ������ �ϴ��� ǥ��
    public Text promotion_gold; // ������ �ʿ��� ���
    public int promotion_gold_size;


    public Button fire_btn;         // �ذ��ư
    public Button promotion_btn;    // ������ư

    int id;
    public void SettingGuardInfo(H_Guard guard) // ���� ����
    {
        // id �ο�
        this.id = guard.id;
        guard.guard_info = this;
        this.guard = guard; // ��ũ��Ʈ ����ȭ

        promotion_gold_size = 300;
        promotion_gold.text = promotion_gold_size + " G";

        // ���� �̸� ����
        employee_name.text = Gamemanager.Instance.employeemanager.first_name[Random.Range(0, Gamemanager.Instance.employeemanager.first_name.Length - 1)]
                     + " " + Gamemanager.Instance.employeemanager.second_name[Random.Range(0, Gamemanager.Instance.employeemanager.second_name.Length - 1)];

        // ���� ���� �ο�
        // �����
        employee_birth_place.text = "����� : " + Gamemanager.Instance.employeemanager.birth_place[Random.Range(0, Gamemanager.Instance.employeemanager.birth_place.Length - 1)];
        // �Ի糯
        employee_state.text = "���� : 1�� ������";
        // ���
        employee_career.text = "����ϼ� : " + career + "��";
        // û�Ҹ� �Ϸ��� �� ��
        employee_caught_thief_count.text = "�˰�Ƚ�� : " + guard.caught_thief_count + "��";
        // ���õ�
        employee_proficiency.text = "���õ� : " + Mathf.Floor(guard.speed * 100) * 0.01; // �ι�° �ڸ������� �ݿø�

        if (Gamemanager.Instance.buffmanager.is_iron_craw_on) // ����� ö������ ���� ���̸� �ذ��ư ��� ����
            fire_btn.enabled = false;
    }
    public void Promotion()
    {
        if (Gamemanager.Instance.buildgame.gold >= promotion_gold_size)
        {
            Gamemanager.Instance.buildgame.PlusGold(-promotion_gold_size);

            // pay 30% ����
            guard.pay = (int)(1.3 * guard.pay);

            // ���õ� 20% ����
            guard.speed *= 1.2f;
            employee_proficiency.text = "���õ� : " + Mathf.Floor(guard.speed * 100) * 0.01;

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
        guard.pay = (int)(1.5 * guard.pay);
        fire_btn.enabled = false;
    }
    public void PatronUp()
    {
        guard.speed *= (1 + Gamemanager.Instance.buffmanager.patron_speed_rate);
    }
    public void Fire() // �� �ذ��!
    {
        if (Gamemanager.Instance.buffmanager.is_severance_pay_on)
            Gamemanager.Instance.buildgame.PlusGold(-100 * (career + 1) * (guard.caught_thief_count + 1));

        Gamemanager.Instance.employeemanager.FireGuard(id);
        Destroy(gameObject);
    }
}
