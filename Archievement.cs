using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archievement : MonoBehaviour
{

    // ����
    // �� �������� ���� �� �غ��غþ�
    // ��� ������ ������ ������ ������(�װų� Ŭ�����ϰų�) �����, �߰��� ���� x


    //*************************int*******************************
    // ���� Ƚ��
    public int game_connected_count;

    // ���� Ŭ���� Ƚ��
    // ����
    public int game_clear_count;

    // ������ ���� ��� ��
    // ���
    public int stolen_gold_count;

    // ���� ���� ��
    // ������
    public int caught_thief_count;

    // �����ϴµ� �Ҹ��� ��� ��
    // ���� ȿ�� ���� 20%
    public int spend_gold_for_fix_elevator_count;

    // ���ߵ��� �ɸ� ȯ�� ��
    // ���� �޷� ���� 20%
    public int illed_human_count;

    // ���ݱ��� ������ ���� ��    
    // ���� ���� 10%
    public int pay_count;

    // ���������� ���� ��� ��
    // ������ ��� +5 
    public int convenience_store_earned_gold_count;

    // �� ���� ������ �ο� ��(���Ϻ��� �����ͱ��� �ϴ�, �׳� ���� Ƚ��)
    // ���������� �⺻ ���緮 + 3
    public int total_transport_human_count;

    //*************************int*******************************

    //*************************bool*******************************

    // �ѱ��� �ӵ��� ������ Ŭ���� ����
    // ����Ŀ (�̹��� ��)
    public bool korean_speed_whether;

    // ���������� Ŭ���� ����
    // Ʈ���� (�̹��� ��)
    public bool english_whether;

    // �ִ� ���̰� 63�������� ���� ���°� ����
    // ����
    public bool higher_than_63_building_whether;
   
    // �ִ� ���̰� ������� ���� ���� ���°� ����(123)
    // ����
    public bool higher_than_jamsil_tower_whether;

    // �� ���ӿ��� û�Һ� 15�� �̻� ����ߴ°� ����
    // �κ�û�ұ� (�̹��� ��)
    public bool employ_cleaner_more_than_ten_whether;

    // 20���� �Ǳ� ���� �׾��°� ����
    // ħ����
    public bool fool_player_whether;

    // �ƹ� ȿ���� ���� ������ �����ߴ°� ����
    // �ް��ͽ� (�̹��� ��)
    public bool choose_no_effect_buff_whether;

    // ��� ������� �ʰ� Ŭ���� �ߴ°� ����
    // �͹̳�����
    public bool dont_employ_guard_whether;

    // ȸ���� �� �̻� ������ Ŭ���� �ߴ°� ����
    // ������
    public bool master_more_than_two_whether;
    //*************************bool*******************************

}
