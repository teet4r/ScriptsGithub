using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;

public class Achievement
{
    public class Key
    {
        public class TypeInt
        {
            public const int type = -1;
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
            public const bool type = true;
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

    DatabaseReference achiv_reference = null;
    Dictionary<string, string> data_int;
    Dictionary<string, string> data_bool;

    public Achievement(DatabaseReference achiv_reference)
    {
        this.achiv_reference = achiv_reference;

        data_int = new Dictionary<string, string>()
            {
                { "game_connected_count", "1" },
                { "game_clear_count", "0" },
                { "stolen_gold_count", "0" },
                { "caught_thief_count", "0" },
                { "spend_gold_for_fix_elevator_count", "0" },
                { "illed_human_count", "0" },
                { "pay_count", "0" },
                { "convenience_store_earned_gold_count", "0" },
                { "total_transport_human_count", "0" },
            };
        data_bool = new Dictionary<string, string>()
            {
                { "korean_speed_whether", "false" },
                { "english_whether", "false" },
                { "higher_than_63_building_whether", "false" },
                { "total_transport_human_count", "false" },
                { "employ_cleaner_more_than_ten_whether", "false" },
                { "fool_player_whether", "false" },
                { "choose_no_effect_buff_whether", "false" },
                { "dont_employ_guard_whether", "false" },
                { "master_more_than_two_whether", "false" }
            };
    }
    // ����
    // �� �������� ���� �� �غ��غþ�
    // ��� ������ ������ ������ ������(�װų� Ŭ�����ϰų�) �����, �߰��� ���� x

    public void UpdateUser(string key, int add)
    {
        if (data_int.ContainsKey(key))
            data_int[key] = (int.Parse(data_int[key]) + add).ToString();
    }
    public void UpdateUser(string key, bool TorF)
    {
        if (data_bool.ContainsKey(key))
            data_bool[key] = TorF ? "true" : "false";
    }

    public bool Contains(string key, int type)
    {
        return data_int.ContainsKey(key);
    }
    public bool Contains(string key, bool type)
    {
        return data_bool.ContainsKey(key);
    }

    public int GetValue(string key, int type)
    {
        if (data_int.ContainsKey(key))
            return int.Parse(data_int[key]);
        return default;
    }
    public bool GetValue(string key, bool type)
    {
        if (data_bool.ContainsKey(key))
            return bool.Parse(data_bool[key]);
        return default;
    }

    public void Create()
    {
        achiv_reference.Child("int").SetValueAsync(data_int);
        achiv_reference.Child("bool").SetValueAsync(data_bool);
    }

    public void Read(DataSnapshot data_snapshot_from_result)
    {
        DataSnapshot ds;

        ds = data_snapshot_from_result.Child("achievement").Child("int");
        foreach (var d in ds.Children)
            UpdateUser(d.Key, int.Parse(d.Value.ToString()));

        ds = data_snapshot_from_result.Child("achievement").Child("bool");
        foreach (var d in ds.Children)
            UpdateUser(d.Key, bool.Parse(d.Value.ToString()));
    }

    public void Save(string key, int type)
    {
        if (data_int.ContainsKey(key))
            achiv_reference.Child("int").UpdateChildrenAsync(ToDictionary(key, data_int[key]));
    }
    public void Save(string key, bool type)
    {
        if (data_bool.ContainsKey(key))
            achiv_reference.Child("bool").UpdateChildrenAsync(ToDictionary(key, data_bool[key]));
    }

    public void SaveAll()
    {
        foreach (var d in data_int)
            achiv_reference.Child("int").UpdateChildrenAsync(ToDictionary(d.Key, d.Value));
        foreach (var d in data_bool)
            achiv_reference.Child("bool").UpdateChildrenAsync(ToDictionary(d.Key, d.Value));
    }

    Dictionary<string, object> ToDictionary(string key, object value)
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic[key] = value;
        return dic;
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