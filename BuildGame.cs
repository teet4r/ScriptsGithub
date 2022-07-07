using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;

// ***********************
// attached in Main Camera
// ***********************

public class BuildGame : MonoBehaviour
{
    public float timespeed; // ���� ��ӿ� ����
    public bool is_gameover { get; private set; }

    // -----------------�����-----------------

    public bool is_attendance_appear;
    public bool is_thief_appear;
    public bool is_sanitation_appear;
    public bool is_homeless_appear;
    public bool is_rush_appear;
    public bool is_sick_appear;

    //--------------------------------------

    public int day;
    public int hour;

    // �� Ư�� �� ���� ��¥
    public int sanitation_day;
    public int homeless_day;
    public int[] rushtimer_days;
    public int delivery_day;

    public int gold { get; private set; } // ����� ������ ������ �����ϴ� ��
    public int one_hour { get; private set; } = 2; // �ΰ��� 1�ð��� ���� �ð�

    public int night_regen; // �� ��� ������
    public bool is_go_to_work_time; // ��� �ð��̸� true 7 ~ 9
    public bool is_lunch_time;      // ����� �ð��̸� true 11 ~ 13
    public bool is_go_to_home;      // ��� �ð��̸� true 18 ~ 22
    public bool is_night;           // �� �ð��̸� ������ ����� �� ����

    // <player life ���� ����> ************************************************************************************
    // �ν����Ϳ��� ���� ���
    [SerializeField]
    private float cur_life;
    [SerializeField]
    private float max_life;
    public int waiting_cnt { get; set; } = 0;
    public int waiting_threshold;
    public float life_reduction_interval;

    public float auto_heal_per_second;
    public Coroutine auto_heal_coroutine;
    // </player life ���� ����> ***********************************************************************************

    public int building_top_floor { get; private set; } = -1; // ���ϴ� �����
    public int building_bottom_floor { get; private set; } = 1;

    public bool is_scenechange_ad_ready = true; // ȭ�� ��ȯ ���� �غ�Ǿ�����
    public string next_scene_name; // ���� �޾Ƽ� ��¿ �� ���� �߰�

    public Sprite[] bubble; // �������⼭ ���̴� ����
    /// <summary>
    /// �� ��ǳ��
    /// ��Ȳ
    /// ü��
    /// �ų�
    /// �˻�1
    /// �˻�2
    /// �˻�3
    /// ȭ��
    /// </summary>

    // <prefabs> *************************************************************************************************
    // �ν����Ϳ����� ���� ����
    [SerializeField]
    private GameObject[] N_floors; // normal floors
    [SerializeField]
    private GameObject F_floor;
    [SerializeField]
    private GameObject R_floor;
    [SerializeField]
    private GameObject C_floor;
    [SerializeField]
    private GameObject V_floor;
    [SerializeField]
    private GameObject M_floor;
    [SerializeField]
    private GameObject B_floor;
    [SerializeField]
    private GameObject hospital_floor;
    [SerializeField]
    private GameObject repairshop_floor;
    [SerializeField]
    private GameObject counter_floor;
    [SerializeField]
    private GameObject under;
    [SerializeField]
    private GameObject roof;
    [SerializeField]
    private GameObject floorSet;
    [SerializeField]
    private Sprite[] roofs;      // ���� �̹��� ���� 
    SpriteRenderer _roof; // << ��� �ϳ����̶� �� ����ٰ� ������
    // </prefabs> ************************************************************************************************

    public List<List<Floor>> floors { get; protected set; } // FID�� �ε��� ���� >> �ش� ���� ��ũ��Ʈ ������
    public List<Floor> floor_of { get; protected set; } // FloorLevel�� �ε��� ���� >> �ش� ���� ��ũ��Ʈ ������

    public GameObject plus_Ele;

    // <UI> ******************************************************************************************************
    public Text day_txt, hour_txt, gold_txt;
    public Text cur_waiting_human_state_txt;// ���� ��� �ο� / ��� �Ӱ����� ȭ�鿡 ǥ��
    public Text alarm_txt; // Ư�� ����� �Ͼ�� ȭ�鿡 ǥ��(�� �� �߰�, �̺�Ʈ �߻� ���)
    public Queue<string> alarm_queue; // Ư�� ��ǵ��� ��� �ִ� ť ���ʷ� ���
    public Image Hp;

    public GameObject gameover_panel;

    [SerializeField]
    GameObject leftdoor, rightdoor;

    // </UI> *****************************************************************************************************

    public GameObject spawn_point;

    ElevatorManager elevator_manager_script;
    EmployeeManager employee_manager_script;
    BuffManager buff_manager_script;

    Coroutine time_coroutine;
    Coroutine life_coroutine;

    public RushTimer[] timer_arr;
    List<int> timer_idx;

    protected void Awake()
    {
        SoundManager.Instance.PlayBgm("Menu1");

        StartCoroutine(OpenDoors());

        is_gameover = false;
        Gamemanager.Instance.Set();

        timer_idx = new List<int>() { 0, 1, 2, 3, 4 };

        gameover_panel.SetActive(false);

        //Camera.main.orthographicSize = 0; //�����
        timespeed = Time.timeScale = 1;
        day = 1;
        hour = 3;
        gold = 10000;
        night_regen = 1;
        elevator_manager_script = GetComponent<ElevatorManager>();
        employee_manager_script = GetComponent<EmployeeManager>();
        buff_manager_script = GetComponent<BuffManager>();

        alarm_queue = new Queue<string>();

        // ������ ��� ������ ���⿡ ����
        floors = new List<List<Floor>>();
        for (int i = FID.INDEX_BEGIN; i <= FID.INDEX_END; i++) // FID�ε����� �ٷ� ������ �� �ְԲ�
            floors.Add(new List<Floor>());
        floor_of = new List<Floor>();

        // ����
        Instantiate(under).transform.position = new Vector2(0, -4);
        Instantiate(under).transform.position = new Vector2(0, -6);

        // ���� 1��(0��)
        GameObject f_temp = Instantiate(repairshop_floor);
        f_temp.name = repairshop_floor.name; // �̸� ����ȭ, ���ϸ� �ڿ� (clone) ����
        Floor f_temp_f = f_temp.GetComponent<Floor>();
        f_temp.transform.position = new Vector2(0, 2 * building_top_floor); // �� ��ġ ����
        f_temp_f.Set(++building_top_floor); // �� �ʱ�ȭ
        floors[FID.REPAIRSHOP].Add(f_temp_f); // floors�迭���� ��ũ��Ʈ ����
        floor_of.Add(f_temp_f);


        //************** ���� *******************
        //1��
        MakeFloor(F_floor, FID.FIRST);

        //2��
        MakeFloor(C_floor, FID.CLEAN);

        // 3��
        MakeFloor(N_floors[0], FID.RED);

        // 4��
        MakeFloor(N_floors[0], FID.RED);

        // 5��
        MakeFloor(R_floor, FID.REST);

        // ����
        GameObject rp = Instantiate(roof);
        rp.transform.position = new Vector2(0, 2 * building_top_floor + 1);
        _roof = rp.GetComponent<SpriteRenderer>();
        _roof.sprite = roofs[2];
        //************** ���� *******************

        // ���������� ����
        for (int line = 0; line < 3; line++)
            elevator_manager_script.MakeElevator(line);


        rushtimer_days = new int[5] // ���� ������ ��¥��
        {
            Random.Range(7, 10) -5,
            Random.Range(15, 21)-8,
            Random.Range(21, 28)-10,
            Random.Range(28, 35)-13,
            Random.Range(35, 42)-16
        };
        homeless_day = Random.Range(5, 7);
        sanitation_day = Random.Range(7, 15);
        delivery_day = 10;

        time_coroutine = StartCoroutine(TimeGo());
        life_coroutine = StartCoroutine(Life());
        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;
    }

    protected void Start()
    {
        auto_heal_coroutine = StartCoroutine(AutoHeal());
    }

    public IEnumerator OpenDoors()
    {
        var leftdoor_pos = leftdoor.transform.localPosition;
        var rightdoor_pos = rightdoor.transform.localPosition;

        // ���� ������ �ӵ��� ������ ������
        for (int i = 0; i <= 110; i += 10)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 110; i <= 220; i += 8)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 220; i <= 330; i += 6)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 330; i <= 440; i += 4)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 440; i <= 540; i += 2)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x - i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x + i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
    }
    public IEnumerator CloseDoors()
    {
        var leftdoor_pos = leftdoor.transform.localPosition;
        var rightdoor_pos = rightdoor.transform.localPosition;

        // ���� ������ �ӵ��� ������ ������
        for (int i = 0; i <= 110; i += 10)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 110; i <= 220; i += 8)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 220; i <= 330; i += 6)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 330; i <= 440; i += 4)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
        for (int i = 440; i <= 540; i += 2)
        {
            leftdoor.transform.localPosition = new Vector3(leftdoor_pos.x + i, leftdoor_pos.y, leftdoor_pos.z);
            rightdoor.transform.localPosition = new Vector3(rightdoor_pos.x - i, rightdoor_pos.y, rightdoor_pos.z);
            yield return null;
        }
    }

    IEnumerator TimeGo()
    {
        while (true)
        {
            while (hour != 24)
            {
                day_txt.text = day.ToString();
                hour_txt.text = hour + ":00";
                gold_txt.text = gold.ToString();
                yield return new WaitForSeconds(one_hour); // 1�ð� 
                hour++;

                if (hour == 2)
                {
                    is_lunch_time = false;
                    if (day % 3 == 1) // 1 4 7 ...
                    {
                        alarm_queue.Enqueue("��� 5�ð���!");
                        alarm_queue.Enqueue("��� 4�ð���!");
                        alarm_queue.Enqueue("��� 3�ð���!");
                        alarm_queue.Enqueue("��� 2�ð���!");
                        alarm_queue.Enqueue("��� 1�ð���!");
                        StartCoroutine(ShowAlarm());
                    }
                }
                else if (hour == 7) // ��� // �̹��� ���� // �� ���� ����
                {
                    is_night = false;
                    is_go_to_work_time = true;
                    night_regen = 1;

                    if (day % 3 == 1) // 1 4 7 ...
                        for (int i = 0; i < Gamemanager.Instance.buffmanager.go_to_work_rate; i++)
                            (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeHumanInMorning()); // ��ٿ� ����� ����� �ڷ�ƾ 4�� �߰�

                    _roof.sprite = roofs[0];
                }
                else if (hour == 9)
                {
                    is_go_to_work_time = false;
                }
                else if (hour == 11)//����
                {
                    is_lunch_time = true;
                }
                else if (hour == 13)
                {
                    is_lunch_time = false; //���� ��

                    if (day % 3 == 0) // 3 6 9 ...
                    {
                        alarm_queue.Enqueue("��� 5�ð���!");
                        alarm_queue.Enqueue("��� 4�ð���!");
                        alarm_queue.Enqueue("��� 3�ð���!");
                        alarm_queue.Enqueue("��� 2�ð���!");
                        alarm_queue.Enqueue("��� 1�ð���!");
                        StartCoroutine(ShowAlarm());
                    }
                }
                else if (hour == 18) // ��� �̹��� ����
                {
                    is_go_to_home = true;

                    if (day % 3 == 0) // 3 6 9 ...
                        for (int go_to_home_count = 0; go_to_home_count < Gamemanager.Instance.buffmanager.go_to_home_rate; go_to_home_count++)
                        {//����� ���� �ڷ�ƾ, ����� ����� 2�� ����
                            for (int i = 0; i < floors[FID.RED].Count; i++)
                                (floors[FID.RED][i] as F_Normal_Red).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.BLUE].Count; i++)
                                (floors[FID.BLUE][i] as F_Normal_Blue).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.GREEN].Count; i++)
                                (floors[FID.GREEN][i] as F_Normal_Green).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.YELLOW].Count; i++)
                                (floors[FID.YELLOW][i] as F_Normal_Yellow).MakeHumanForGoHome();

                            for (int i = 0; i < floors[FID.VIP].Count; i++)
                                (floors[FID.VIP][i] as F_Vip).MakeHumanForGoHome();
                        }

                    _roof.sprite = roofs[1];
                }
                else if (hour == 20)
                {
                    is_go_to_home = false;
                }
                else if (hour == 22) // �� ���� ����
                {
                    // 2 or 1
                    night_regen = 2 - Gamemanager.Instance.buffmanager.human_create_in_night_rate;
                    is_night = true;

                    if (day > 4)
                    {
                        Gamemanager.Instance.uimanager.ShowExplain(ref is_thief_appear, PID.THIEF);

                        for (int i = 0; i < 3; i++)
                            (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeThief()); // ���� �߰� �ڷ�ƾ 3��

                        alarm_queue.Enqueue("���� ����");
                        StartCoroutine(ShowAlarm());
                    }
                    _roof.sprite = roofs[2];
                }
                else if (hour == 24)
                {
                    is_lunch_time = Gamemanager.Instance.buffmanager.is_there_night_rest; // �߽����� ����
                    if (day % 3 == 0) // 3 6 9 ...
                    {
                        Gamemanager.Instance.uimanager.TimeStop();
                        Gamemanager.Instance.buffmanager.PickAndShowBuff(); // 3���� ���� ����
                        alarm_queue.Enqueue("�ӱ� " + employee_manager_script.PayDay()+ "�� ����"); // �ӱ�����
                        StartCoroutine(ShowAlarm());

                        waiting_threshold += Gamemanager.Instance.buffmanager.supply_size;
                        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;

                        for (int build_count = 0; build_count < Gamemanager.Instance.buffmanager.plus_floor_size; build_count++) // �ǹ� ����
                        {
                            if (Random.Range(0, 100) < 70)
                            {
                                // �Ϲ��� ����
                                int nfloor_idx = Random.Range(0, N_floors.Length);
                                MakeFloor(N_floors[nfloor_idx], nfloor_idx);
                            }
                            else
                            {
                                // Ư���� ����
                                int idx = Random.Range(0, 100);
                                if (idx < 20)
                                    MakeFloor(hospital_floor, FID.HOSPITAL);
                                else if (idx < 45)
                                    MakeFloor(R_floor, FID.REST);
                                else if (idx < 50)
                                    MakeFloor(M_floor, FID.MASTER);
                                else if (idx < 60)
                                    MakeFloor(B_floor, FID.BANK);
                                else if (idx < 80)
                                    MakeFloor(counter_floor, FID.CONVENIENCE);
                                else
                                    MakeFloor(V_floor, FID.VIP);
                            }
                            _roof.transform.position = new Vector2(0, 2 * building_top_floor + 1);
                        }
                    }
                    if (day % 6 == 0)
                    {
                        Gamemanager.Instance.uimanager.TimeStop();
                        plus_Ele.SetActive(true);
                    }
                }
            }
            hour = 0;
            day++;
            employee_manager_script.EmployeesCareerUp();//��� ����

            // 50������ ���߾� ��
            if (day > 50)
            {
                StartCoroutine(GameOver());
                yield break;
            }

            // -------------------Ư�� �̺�Ʈ ��¥---------------------
            if (day == homeless_day) // ���� ����� ����
            {
                Gamemanager.Instance.uimanager.ShowExplain(ref is_homeless_appear, PID.HOMELESS);

                (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeHomeless());
            }
            if (day == sanitation_day) // ���� ������ ����
            {
                Gamemanager.Instance.uimanager.ShowExplain(ref is_sanitation_appear, PID.SANITATION);

                (floors[FID.FIRST][0] as F_First).StartCoroutine((floors[FID.FIRST][0] as F_First).MakeSanitation());
            }
            foreach (var r_day in rushtimer_days)
                if (day == r_day) // Ÿ�̸� ����
                {
                    Gamemanager.Instance.uimanager.ShowExplain(ref is_rush_appear, 1);

                    int random_idx = Random.Range(0, timer_idx.Count);

                    timer_arr[timer_idx[random_idx]].StartTimer();

                    timer_idx.RemoveAt(random_idx);

                    alarm_queue.Enqueue("���ο� Ÿ�̸� ����!");
                    StartCoroutine(ShowAlarm());
                }
        }
    }

    IEnumerator Life()
    {
        while (true)
        {
            if (cur_life > max_life)
                cur_life = max_life;
            if (waiting_cnt > waiting_threshold)
                cur_life -= waiting_cnt - waiting_threshold;
            if (cur_life <= 0)
            {
                cur_life = 0;
                Hp.fillAmount = cur_life / max_life;

                Gamemanager.Instance.uimanager.stop_panels_count = 9999999;

                StartCoroutine(GameOver());
                yield break;
            }
            Hp.fillAmount = cur_life / max_life; // ���������� ȭ�鿡 �׷���
            yield return new WaitForSeconds(life_reduction_interval);
        }
    }

    public void PlusHp(float healing_size)
    {
        cur_life += healing_size;
        if (cur_life < 0)
            cur_life = 0;
        else if (cur_life > max_life)
            cur_life = max_life;
    }

    public void ChangeCurrentWaitingHumanCount(int population) // ���� ��� �ο� ����
    {
        waiting_cnt += population;
        // UI
        cur_waiting_human_state_txt.text = waiting_cnt + " / " + waiting_threshold;
    }

    public void MakeFloor(GameObject floor_prefab, int floor_color)
    {
        GameObject f_temp = Instantiate(floor_prefab);
        f_temp.name = floor_prefab.name; // �̸� ����ȭ, ���ϸ� �ڿ� (clone) ����
        Floor f_temp_f = f_temp.GetComponent<Floor>();
        f_temp.transform.position = new Vector2(0, 2 * building_top_floor); // �� ��ġ ����
        f_temp_f.Set(++building_top_floor); // �� �ʱ�ȭ
        floors[floor_color].Add(f_temp_f); // floors�迭���� ��ũ��Ʈ ����
        floor_of.Add(f_temp_f);
    }

    // arguments: int ���� ��, int[] ���� �� �ִ� �� �����
    // ���� �� �ִ� �� ����� �߿� �������� �ϳ� ����
    public Floor GetDestinationExceptCurrent(List<Floor> destinations, int current_floorlevel, params int[] possible_FIDs)
    {
        destinations.Clear();
        // ���� ���� ������ �� ���� ���� ������ ��� ���� ������� ���� ��
        foreach (var possible_FID in possible_FIDs)
            foreach (var floor_script in floors[possible_FID])
                if (current_floorlevel != floor_script.floor_level)
                    destinations.Add(floor_script);
        return destinations.Count == 0 ? null : destinations[Random.Range(0, destinations.Count)];
    }

    // ���� �����κ��� ���� ����� FID���� ã��
    // �ڽ��� ���ִ� ���� ���� �����ٸ� �� �������� ����� ���� ã��
    public Floor GetClosestFID(int currentfloor_level, int dest_FID)
    {
        if (floors[dest_FID].Count == 0)
            return null;

        int idx = 0, closest_dis = int.MaxValue;
        for (int i = 0; i < floors[dest_FID].Count; i++)
        {
            if (currentfloor_level == floors[dest_FID][i].floor_level)
                continue;

            int d = Mathf.Abs(currentfloor_level - floors[dest_FID][i].floor_level);
            if (d < closest_dis)
            {
                closest_dis = d;
                idx = i;
            }
        }
        return floors[dest_FID][idx];
    }

    public IEnumerator ShowAlarm() // ������ ���ڷ� ����
    {
        while (alarm_queue.Count > 0)
        {
            alarm_txt.text = alarm_queue.Dequeue(); // ����
            alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 1); // ������ ����
            yield return StartCoroutine(AlarmFadeOut());
            yield return new WaitForSeconds(0.01f); // ���� �����ø� ���� ��ũ��Ʈ
        }
    }
    IEnumerator AlarmFadeOut()// �˶� ������ ��������ϱ�
    {
        for (int i = 0; i < 33; i++)
        {
            alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 1 - 0.03f * i);
            yield return null;
        }
        alarm_txt.color = new Color(alarm_txt.color.r, alarm_txt.color.g, alarm_txt.color.b, 0);
    }

    public void PlusGold(int gold)
    {
        this.gold += gold;

        if(this.gold < 0)
        {
            PlusHp(this.gold / 20 * 5);
            this.gold = 0;
        }

        gold_txt.text = this.gold.ToString();
    }
    public void PlusGoldFromHuman(Human hs, int gold)
    {
        this.gold += gold;
        gold_txt.text = this.gold.ToString();

        StartCoroutine(hs.CoinEffect(gold));
    }

    /// <summary>
    /// Gameover Functions Below This Row
    /// </summary>
    IEnumerator GameOver()
    {
        is_gameover = true;
        Time.timeScale = 0; // ���� ����

        // **************************************** �г� ȿ�� ���� ****************************************
        gameover_panel.SetActive(true);

        // ������ ������
        float x = gameover_panel.transform.localPosition.x;
        for (float y = gameover_panel.transform.localPosition.y; y >= -500; y -= 50f)
        {
            gameover_panel.transform.localPosition = new Vector2(x, y);
            yield return null;
        }

        StartCoroutine(SlimeEffect(gameover_panel));
        // **************************************** �г� ȿ�� ���� ****************************************

    }

    IEnumerator SlimeEffect(GameObject panel)
    {
        float origin_width = panel.transform.localScale.x;
        float origin_height = panel.transform.localScale.y;
        for (float Delta = 2.0f; Delta >= 1f; Delta -= 0.5f)
        {
            // ¥�ν�Ű�� ���
            for (float delta = 0f; delta < 50f; delta += 5f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }

            // ���̴� ���
            for (float delta = 50f; delta >= -50f; delta -= 10f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }

            // �����·� ����
            for (float delta = -50f; delta <= 0f; delta += 5f)
            {
                panel.transform.localScale = new Vector2(origin_width + Delta * delta * 1.3f, origin_height - Delta * delta * 0.7f);
                yield return null;
            }
        }
    }

    public void OnClickLoadScene(string name)
    {
        SceneManager.LoadScene(name);
        /*
        next_scene_name = name;
        if (is_scenechange_ad_ready) // ���� �غ�Ǹ� ������
        {
            RequestInterstitial();

            if (this.interstitial.IsLoaded())
                this.interstitial.Show();
        }
        else
            StartCoroutine(OnClickLoadScene_(name));
        */
    }
    /*
    IEnumerator OnClickLoadScene_(string name)
    {
        yield return StartCoroutine(CloseDoors());
        SceneManager.LoadScene(name);
    }
    private InterstitialAd interstitial;

    private void RequestInterstitial() // ȭ�� ��ȯ �����
    {
        #if UNITY_ANDROID
                string adUnitId = "ca-app-pub-3940256099942544/1033173712";
        #elif UNITY_IPHONE
                string adUnitId = "ca-app-pub-3940256099942544/4411468910";
        #else
                string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

    }
    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        is_scenechange_ad_ready = false;
        StartCoroutine(OnClickLoadScene_(next_scene_name));
    }*/
    public void StartDebuffLoanShark()
    {
        PlusGold(3000);
        StartCoroutine(PayBackDept());
    }
    IEnumerator PayBackDept()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(24 * one_hour);

            if (gold < 200)
            {
                StartCoroutine(GameOver());
                yield break;
            }

            gold -= 200;
        }
    }

    // �ڵ����� ü�� ȸ��
    // �ּڰ��� 1
    IEnumerator AutoHeal()
    {
        while (!is_gameover) {
            PlusHp(Mathf.Max(1f, auto_heal_per_second));

            yield return new WaitForSeconds(1f);
        }
    }
}