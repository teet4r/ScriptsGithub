using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookShelf : EventObject
{
    // ���� ���� ����
    // ���� ��ȭ ȹ�� or ���� ��ȭ ����

    public void Awake()
    {
        earn_things = 0;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        // �ϰ� ���� �ൿ ����
        yield return human.StartCoroutine(human.ActAtBS(this));
        human.animator.SetBool("isWaiting", false);
    }
}
