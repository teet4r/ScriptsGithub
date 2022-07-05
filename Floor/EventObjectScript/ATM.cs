using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATM : EventObject
{
    // ���� ���� ����
    // ���� ��ȭ ȹ�� or ���� ��ȭ ����

    public void Awake()
    {
        earn_things = 20;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        // �ϰ� ���� �ൿ ����
        yield return human.StartCoroutine(human.ActAtATM(this));
        human.animator.SetBool("isWaiting", false);
    }
}
