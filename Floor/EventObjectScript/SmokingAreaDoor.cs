using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokingAreaDoor : EventObject
{
    // �Ĵ� ���� ����
    // ���� �� �������⸸�ұ�
    // ���ޱ� �� ���ѵ�
    public void Awake()
    {
        earn_things = 0;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);

        yield return human.StartCoroutine(human.ActAtSAD(this));
    }
}
