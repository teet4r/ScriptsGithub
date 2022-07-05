using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestExit : EventObject
{
    public float addict_threshold = 0.1f;
    public void Awake()
    {
        earn_things = 30;
        spend_things = 0;
    }
    public override IEnumerator EventOn(Human human)
    {
        human.animator.SetBool("isWaiting", true);
        // �ϰ� ���� �ൿ ����
        yield return human.StartCoroutine(human.ActAtRestExit(this));
    }
}