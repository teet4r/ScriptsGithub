using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalDoor : EventObject
{
    // �������� ����
    // ���� ��ȭ ȹ��
    // H_Sick�� ���� ��� H_Patient Ȯ�� ����
    // ��Ÿ �ٸ� ����� ���ð�� > ??
    // ������ ���� ��� > ??
    public void Awake()
    {
        earn_things = 30;
        spend_things = -100;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        yield return human.StartCoroutine(human.ActAtHD(this));
    }
}
