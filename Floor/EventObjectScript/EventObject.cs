using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    // tag = EventObject
    // Layer = AfterHuman(11��) = ���� ���� ������� �浹


    // �ʿ����� ���� �ϴ� �ƹ��ų� �����
    public Floor cur_floor;
    public int kind_of; // object ���� (������ 0���� ����)
    public int id; // object ��ȣ (�������� 0���� ����)
    public int use_count; // ��� ���� Ƚ��
    public int spend_things; // �Ҹ� ��ȭ
    public int earn_things; // ��� ��ȭ
    public bool delivery_called = false; // �̹� ��޺θ� �ҷ����� true, �ߺ� ȣ�� ����
    protected F_First FF;

    private void Start()
    {
        FF = Gamemanager.Instance.buildgame.floors[FID.FIRST][0] as F_First;
    }
    // ��ü�� �ൿ
    // Human ���� Ư�� Move�� ���������� ���Ǻη�(�浹 ��) ȣ��� ����
    public virtual IEnumerator EventOn(Human human) 
    {
        yield return null;
    }
}
