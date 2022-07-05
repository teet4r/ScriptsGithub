using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuffManager : MonoBehaviour
{
    private List<Buff> Buffs;
    private List<Debuff> Debuffs;

    [SerializeField]
    private GameObject buff_panel;
    [SerializeField]
    private Buff_Choice_UI[] buff; // ���� ���� ��ư

    [SerializeField]
    private GameObject debuff_panel;
    [SerializeField]
    private Buff_Choice_UI debuff; // ����� ���� ��ư

    private Buff[] pickedbuff;

    public void Start()
    {
        Buffs = new List<Buff>();
        Debuffs = new List<Debuff>();


        Buffs.Add(new Oxyclean());          // ����ũ��
        Buffs.Add(new EmptyElevator());     // ������ ����������
        Buffs.Add(new Ghost());             // ��üȭ
        Buffs.Add(new MsKim());             // �迩��
        Buffs.Add(new CompanyJogging());    // �系����
        Buffs.Add(new Telecommuting());     // ���ñٹ�
        Buffs.Add(new VVVVVIP());           // VVVVVIP
        Buffs.Add(new BonusDay());          // ���ʽ� �޴� ��
        Buffs.Add(new LeavingPay());        // ��ٺ�
        Buffs.Add(new GuardPatron());       // ����5����
        Buffs.Add(new Nothing());           // nothing
        Buffs.Add(new Gravity());           // �߷�
        Buffs.Add(new DaddyAwake());        // �ƺ� ���ܴ�
        Buffs.Add(new FastLoad());          // ���� ž��
        Buffs.Add(new FixMoveUp());         // ��ī
        Buffs.Add(new LateRush());          // ���峭 �ڸ���
        Buffs.Add(new LateRush());          // �޽� = ����



        Debuffs.Add(new Surfeit());         // ����
        Debuffs.Add(new Overload());        // ������� ����� ����
        Debuffs.Add(new FoodPoisoning());   // ���ߵ�
        Debuffs.Add(new BinMissing());      // ���������� ����?
        Debuffs.Add(new CleanerTip());      // û�Һ�
        Debuffs.Add(new HongGilDong());     // ȫ�浿
        Debuffs.Add(new SocialDistance());  // ��ȸ�� �Ÿ��α�
        Debuffs.Add(new LeavingWork());     // Į ��
        Debuffs.Add(new Darktempler());     // ��ũ���÷�
        Debuffs.Add(new KoreanSpeed());     // �ѱ��μӵ�
        Debuffs.Add(new Acrophobia());      // ��Ұ�����
        Debuffs.Add(new DruckReinforce());  // ���ּ���
        Debuffs.Add(new IronBowl());        // ö����
        Debuffs.Add(new CriticalPatient()); // ��ȯ��
        Debuffs.Add(new MoldWall());        // ������ ����
        Debuffs.Add(new EleVIRItor());      // ����"��"��
        Debuffs.Add(new VIPTicket());       // VIPƼ��
        Debuffs.Add(new OverTimeMeal());    // �߽�����
        Debuffs.Add(new WorkOvertime());    // �̾� �߱��̴�
        Debuffs.Add(new Arsenal());         // �ƽ���
        Debuffs.Add(new HeatWave());        // ����
        Debuffs.Add(new LoanShark());       // ��ä����
        Debuffs.Add(new Mysophobia());      // �Ắ��
        Debuffs.Add(new Overlap());         // �ߺ�
        Debuffs.Add(new RushTimeUp());      // ������Ʈ ����
        Debuffs.Add(new UnLuckyDay());      // ��� ���� ��

        //Debuffs.Add(new BodyGuard());// ���𰡵�

        pickedbuff = new Buff[3];
    }

    public void PickAndShowBuff() // ������ ������ ������ ����
    {
        for (int i = 0; i < 3; i++) // ������ ������ ������ ���� �� ����
        {
            int random_idx = Random.Range(0, Buffs.Count);
            pickedbuff[i] = Buffs[random_idx];
            Buffs.RemoveAt(random_idx);
        }

        buff[0].Setting(pickedbuff[0], 0);
        buff[1].Setting(pickedbuff[1], 1);
        buff[2].Setting(pickedbuff[2], 2);

        buff_panel.SetActive(true);
    }
    int debuff_count;

    public void ChooseBuff() // ��ư Ŭ�� �� ����Ǵ� �׼Ǹ����� �Լ�(���� ����)
    {
        int choosen_buff_idx = EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<Buff_Choice_UI>().id;
        // ������ ��ư�� �ε����� ������

        for (int i = 0; i < 3; i++)
            if(i != choosen_buff_idx)
                Buffs.Add(pickedbuff[i]); // ���ù��� ���� �������� �ٽ� �־��

        debuff_count = unlucky_day_count;
        pickedbuff[choosen_buff_idx].BuffOn(); // ������ ���� ����
        buff_panel.SetActive(false);
        ApplyRandomDebuff();
    }

    void ApplyRandomDebuff() // ������� �ƹ� ������� �����ϰ� ����
    {
        debuff_panel.SetActive(true);
        int random_idx = Random.Range(0, Debuffs.Count);

        Debuffs[random_idx].DebuffOn();

        debuff_count--;

        debuff.Setting(Debuffs[random_idx]);

        if (!is_overlap_on)
            Debuffs.RemoveAt(random_idx); // ���� �� ����Ʈ���� ����
    }
    public void DebuffCheckPush() // ������� Ȯ���ϰ� Ȯ�ι�ư�� ����
    {
        debuff_panel.SetActive(false);

        if (debuff_count > 0)
            ApplyRandomDebuff();
        else
            Gamemanager.Instance.uimanager.TryTimeRestart();
    }

    ////////////////////////////////////////////����////////////////////////////////////////////

    public float cleaning_rate { get; set; } = 0f;
    public float empty_elevator_speed_rate { get; set; } = 0f;
    public bool is_ghost_elevator { get; set; } = false;
    public int human_create_in_night_rate { get; set; } = 0; // 0 or 1
    public float vip_female_speed_rate { get; set; } = 0;
    public bool is_there_night_rest { get; set; } = false;
    public float human_speed_rate { get; set; } = 0f;
    public int go_to_work_rate { get; set; } = 5;
    public float vvvvvip_speed_rate { get; set; } = 0f;

    // ���ʽ� �޴� ���� ������ ����
    public int go_to_home_pay_size { get; set; } = 0;
    public float patron_speed_rate { get; set; } = 0; // �����ɶ� + ���� ���Ǿ��ִ� ����� ��� ����
    public bool nothing { get; set; } = false;
    public float gravity_elevator_rate { get; set; } = 0;
    public float elevator_on_speed_rate { get; set; } = 0;
    public float elevator_speed_rate_for_fix { get; set; } = 0;
    public float rush_distance_size { get; set; } = 0;
    public int healing_size { get; set; } = 1;
    public int supply_size { get; set; } = 10;

    ////////////////////////////////////////////����////////////////////////////////////////////
    //.
    //.
    //.
    //.
    //.
    ////////////////////////////////////////////�����////////////////////////////////////////////
    public float fat_human_appear_rate { get; set; } = 0f;
    public float overload_speed_rate { get; set; } = 0f;
    public float illed_human_rate { get; set; } = 0;
    public float dirty_increase_rate { get; set; } = 0f;
    public int cleaner_pay_size { get; set; } = 0;
    public float thief_proficiency_increase_rate { get; set; } = 0;
    public int elevator_volume_half { get; set; } = 1;
    public float go_to_home_rate { get; set; } = 1f;
    public float thief_appear_in_night_rate { get; set; } = 0f;
    public bool is_korean_speed { get; set; } = false;
    public float acrophobia_speed_rate { get; set; } = 0f;
    public int reinforce_fail_threshold { get; set; } = 0;
    public bool is_iron_craw_on { get; set; } = false;
    public float pay_increase_rate { get; set; } = 0f;
    public float patient_speed_rate { get; set; } = 0f;
    public int mold_pay_size { get; set; } = 0;
    public float durability_decrease_rate { get; set; } = 0f;
    public int vip_ticket { get; set; } = 1; // üũ�Ǹ� 0
    public int plus_floor_size { get; set; } = 3;
    public float thief_patience_rate { get; set; } = 1;
    public int use_size_of_bevarage { get; set; } = 1;
    public float delivery_speed_rate { get; set; } = 0;
    public int sanitation_check_size { get; set; } = 0;
    public bool is_overlap_on { get; set; } = false;
    public float rush_time_size { get; set; } = 0;
    public bool is_severance_pay_on { get; set; } = false;
    public int unlucky_day_count { get; set; } = 1;

    // �̸� ���𰡵�
    // ���� ��Ʈ���� ����̾� ����
    // ȿ�� ȸ��� ������ ������ �� �����ϴ�. ���������Ϳ� ȸ���� Ÿ�� ��� ���i���ϴ�.

    ////////////////////////////////////////////�����////////////////////////////////////////////
}

// �� ���� ����� ���� ������ ����
// ���� ����� ����� ����ð� ����