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
    // 업적
    // 뭘 좋아할지 몰라 다 준비해봤어
    // 모든 업적은 게임이 완전히 끝나고(죽거나 클리어하거나) 저장됨, 중간에 저장 x

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
    // 접속 횟수
    //public int game_connected_count = 0;

    // 게임 클리어 횟수
    // 미정
    //public int game_clear_count = 0;

    // 도둑질 당한 골드 수
    // 고블린
    //public int stolen_gold_count = 0;

    // 잡은 도둑 수
    // 포돌이
    //public int caught_thief_count = 0;

    // 수리하는데 소모한 골드 수
    // 수리 효율 증가 20%
    //public int spend_gold_for_fix_elevator_count = 0;

    // 식중독에 걸린 환자 수
    // 병원 급료 증가 20%
    //public int illed_human_count = 0;

    // 지금까지 지급한 월급 수    
    // 월급 인하 10%
    //public int pay_count = 0;

    // 편의점에서 얻은 골드 수
    // 편의점 골드 +5 
    //public int convenience_store_earned_gold_count = 0;

    // 총 수송 성공한 인원 수(도둑부터 마스터까지 싹다, 그냥 내린 횟수)
    // 엘리베이터 기본 적재량 + 3
    //public int total_transport_human_count = 0;

    //*************************int*******************************

    //*************************bool*******************************

    // 한국인 속도를 가지고 클리어 여부
    // 페이커 (이미지 無)
    //public bool korean_speed_whether = false;

    // 영어판으로 클리어 여부
    // 트럼프 (이미지 無)
    //public bool english_whether = false;

    // 최대 높이가 63빌딩보다 높게 갔는가 여부
    // 미정
    //public bool higher_than_63_building_whether = false;

    // 최대 높이가 잠실층수 보다 높게 갔는가 여부(123)
    // 미정
    //public bool higher_than_jamsil_tower_whether = false;

    // 한 게임에서 청소부 15명 이상 고용했는가 여부
    // 로봇청소기 (이미지 無)
    //public bool employ_cleaner_more_than_ten_whether = false;

    // 20일이 되기 전에 죽었는가 여부
    // 침착맨
    //public bool fool_player_whether = false;

    // 아무 효과도 없는 버프를 선택했는가 여부
    // 달걀귀신 (이미지 無)
    //public bool choose_no_effect_buff_whether = false;

    // 경비를 고용하지 않고 클리어 했는가 여부
    // 터미네이터
    //public bool dont_employ_guard_whether = false;

    // 회장을 둘 이상 데리고 클리어 했는가 여부
    // 원숭이
    //public bool master_more_than_two_whether = false;
    //*************************bool*******************************
}