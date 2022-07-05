using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infomation : EventObject
{
    // 1���� ����
    // ���� ��ȣ�ۿ��� ��Ȯ�� ��ü�� ����
    // 

    void Awake()
    {
        earn_things = 5;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        yield return human.StartCoroutine(human.ActAtInfo(this));
        human.animator.SetBool("isWaiting", false);
    }
}
