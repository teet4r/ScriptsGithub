using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archievement : MonoBehaviour
{

    // 업적
    // 뭘 좋아할지 몰라 다 준비해봤어
    // 모든 업적은 게임이 완전히 끝나고(죽거나 클리어하거나) 저장됨, 중간에 저장 x


    //*************************int*******************************
    // 접속 횟수
    public int game_connected_count;

    // 게임 클리어 횟수
    // 미정
    public int game_clear_count;

    // 도둑질 당한 골드 수
    // 고블린
    public int stolen_gold_count;

    // 잡은 도둑 수
    // 포돌이
    public int caught_thief_count;

    // 수리하는데 소모한 골드 수
    // 수리 효율 증가 20%
    public int spend_gold_for_fix_elevator_count;

    // 식중독에 걸린 환자 수
    // 병원 급료 증가 20%
    public int illed_human_count;

    // 지금까지 지급한 월급 수    
    // 월급 인하 10%
    public int pay_count;

    // 편의점에서 얻은 골드 수
    // 편의점 골드 +5 
    public int convenience_store_earned_gold_count;

    // 총 수송 성공한 인원 수(도둑부터 마스터까지 싹다, 그냥 내린 횟수)
    // 엘리베이터 기본 적재량 + 3
    public int total_transport_human_count;

    //*************************int*******************************

    //*************************bool*******************************

    // 한국인 속도를 가지고 클리어 여부
    // 페이커 (이미지 無)
    public bool korean_speed_whether;

    // 영어판으로 클리어 여부
    // 트럼프 (이미지 無)
    public bool english_whether;

    // 최대 높이가 63빌딩보다 높게 갔는가 여부
    // 미정
    public bool higher_than_63_building_whether;
   
    // 최대 높이가 잠실층수 보다 높게 갔는가 여부(123)
    // 미정
    public bool higher_than_jamsil_tower_whether;

    // 한 게임에서 청소부 15명 이상 고용했는가 여부
    // 로봇청소기 (이미지 無)
    public bool employ_cleaner_more_than_ten_whether;

    // 20일이 되기 전에 죽었는가 여부
    // 침착맨
    public bool fool_player_whether;

    // 아무 효과도 없는 버프를 선택했는가 여부
    // 달걀귀신 (이미지 無)
    public bool choose_no_effect_buff_whether;

    // 경비를 고용하지 않고 클리어 했는가 여부
    // 터미네이터
    public bool dont_employ_guard_whether;

    // 회장을 둘 이상 데리고 클리어 했는가 여부
    // 원숭이
    public bool master_more_than_two_whether;
    //*************************bool*******************************

}
