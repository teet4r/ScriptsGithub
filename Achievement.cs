using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement : MonoBehaviour
{
    public Dictionary<string, int> data_int { get; private set; }
    public Dictionary<string, bool> data_bool { get; private set; }

    public Achievement()
    {
        data_int = new Dictionary<string, int>()
        {
            { "game_connected_count", 1 },
            { "game_clear_count", 0 },
            { "stolen_gold_count", 0 },
            { "caught_thief_count", 0 },
            { "spend_gold_for_fix_elevator_count", 0 },
            { "illed_human_count", 0 },
            { "pay_count", 0 },
            { "convenience_store_earned_gold_count", 0 },
            { "total_transport_human_count", 0 },
        };
        data_bool = new Dictionary<string, bool>()
        {
            { "korean_speed_whether", false },
            { "english_whether", false },
            { "higher_than_63_building_whether", false },
            { "total_transport_human_count", false },
            { "employ_cleaner_more_than_ten_whether", false },
            { "fool_player_whether", false },
            { "choose_no_effect_buff_whether", false },
            { "dont_employ_guard_whether", false },
            { "master_more_than_two_whether", false }
        };
    }
    // ����
    // �� �������� ���� �� �غ��غþ�
    // ��� ������ ������ ������ ������(�װų� Ŭ�����ϰų�) �����, �߰��� ���� x

    public class Key
    {
        public class TypeInt
        {
            public const string game_connected_count = "game_connected_count";
            public const string game_clear_count = "game_clear_count";
            public const string stolen_gold_count = "stolen_gold_count";
            public const string caught_thief_count = "caught_thief_count";
            public const string spend_gold_for_fix_elevator_count = "spend_gold_for_fix_elevator_count";
            public const string illed_human_count = "illed_human_count";
            public const string pay_count = "pay_count";
            public const string convenience_store_earned_gold_count = "convenience_store_earned_gold_count";
            public const string total_transport_human_count = "total_transport_human_count";
        }
        public class TypeBool
        {
            public const string korean_speed_whether = "korean_speed_whether";
            public const string english_whether = "english_whether";
            public const string higher_than_63_building_whether = "higher_than_63_building_whether";
            public const string total_transport_human_count = "total_transport_human_count";
            public const string employ_cleaner_more_than_ten_whether = "employ_cleaner_more_than_ten_whether";
            public const string fool_player_whether = "fool_player_whether";
            public const string choose_no_effect_buff_whether = "choose_no_effect_buff_whether";
            public const string dont_employ_guard_whether = "dont_employ_guard_whether";
            public const string master_more_than_two_whether = "master_more_than_two_whether";
        }
    }

    public void UpdateUser(string key, int add)
    {
        if (data_int.ContainsKey(key))
            data_int[key] += add;
    }
    public void UpdateUser(string key, bool TorF)
    {
        if (data_bool.ContainsKey(key))
            data_bool[key] = TorF;
    }

    //*************************int*******************************
    // ���� Ƚ��
    //public int game_connected_count = 0;

    // ���� Ŭ���� Ƚ��
    // ����
    //public int game_clear_count = 0;

    // ������ ���� ��� ��
    // ���
    //public int stolen_gold_count = 0;

    // ���� ���� ��
    // ������
    //public int caught_thief_count = 0;

    // �����ϴµ� �Ҹ��� ��� ��
    // ���� ȿ�� ���� 20%
    //public int spend_gold_for_fix_elevator_count = 0;

    // ���ߵ��� �ɸ� ȯ�� ��
    // ���� �޷� ���� 20%
    //public int illed_human_count = 0;

    // ���ݱ��� ������ ���� ��    
    // ���� ���� 10%
    //public int pay_count = 0;

    // ���������� ���� ��� ��
    // ������ ��� +5 
    //public int convenience_store_earned_gold_count = 0;

    // �� ���� ������ �ο� ��(���Ϻ��� �����ͱ��� �ϴ�, �׳� ���� Ƚ��)
    // ���������� �⺻ ���緮 + 3
    //public int total_transport_human_count = 0;

    //*************************int*******************************

    //*************************bool*******************************

    // �ѱ��� �ӵ��� ������ Ŭ���� ����
    // ����Ŀ (�̹��� ��)
    //public bool korean_speed_whether = false;

    // ���������� Ŭ���� ����
    // Ʈ���� (�̹��� ��)
    //public bool english_whether = false;

    // �ִ� ���̰� 63�������� ���� ���°� ����
    // ����
    //public bool higher_than_63_building_whether = false;

    // �ִ� ���̰� ������� ���� ���� ���°� ����(123)
    // ����
    //public bool higher_than_jamsil_tower_whether = false;

    // �� ���ӿ��� û�Һ� 15�� �̻� ����ߴ°� ����
    // �κ�û�ұ� (�̹��� ��)
    //public bool employ_cleaner_more_than_ten_whether = false;

    // 20���� �Ǳ� ���� �׾��°� ����
    // ħ����
    //public bool fool_player_whether = false;

    // �ƹ� ȿ���� ���� ������ �����ߴ°� ����
    // �ް��ͽ� (�̹��� ��)
    //public bool choose_no_effect_buff_whether = false;

    // ��� ������� �ʰ� Ŭ���� �ߴ°� ����
    // �͹̳�����
    //public bool dont_employ_guard_whether = false;

    // ȸ���� �� �̻� ������ Ŭ���� �ߴ°� ����
    // ������
    //public bool master_more_than_two_whether = false;
    //*************************bool*******************************
}
