using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeManager : MonoBehaviour
{

    // ���� Ư¡
    // �ӱ��� ������ ����
    // ���� �ð����� �ӱ��� �����
    // ����ϱ� ���ؼ� ���� ��尡 �־����
    // �ӱ��� ���� ���ϸ� �������� >> �ذ�!
    int cleaner_id = 0;
    public Dictionary<int, H_Cleaner> cleaner_dic { get; private set; }
    int guard_id = 0;
    public Dictionary<int, H_Guard> guard_dic { get; private set; }
    int mechanic_id = 0;
    public Dictionary<int, H_Mechanic> mechanic_dic { get; private set; }

    public int remain_mechanic_count;

    public GameObject cleaners_info;
    public Transform cleaners_contents; // ���� ���� �г�(���� ȭ�鿡�� ���� �Ʒ� ��ư ������ ������ �г�) ���� �����ϴ� ��ũ�� ���� Content
    public GameObject guards_info;
    public Transform guards_contents; // ���� ���� �г�(���� ȭ�鿡�� ���� �Ʒ� ��ư ������ ������ �г�) ���� �����ϴ� ��ũ�� ���� Content
    public GameObject mechanices_info;
    public Transform mechanices_contents; // ���� ���� �г�(���� ȭ�鿡�� ���� �Ʒ� ��ư ������ ������ �г�) ���� �����ϴ� ��ũ�� ���� Content

    [SerializeField]
    int cleaner_hire_gold_size;
    public Button cleaner_hire_button; // �� �Ǻ� ��� ��ư
    public Text cleaner_hire_gold;
    [SerializeField]
    int guard_hire_gold_size;
    public Button guard_hire_button;
    public Text guard_hire_gold;
    [SerializeField]
    int mechanic_hire_gold_size;
    public Button mechanic_hire_button;
    public Text mechanic_hire_gold;

    public Button cleaners_tab;
    public Button guards_tab;
    public Button mechanices_tab;

    public Image manager_tab_img; // ��� ����
    public Sprite[] manager_tab_images;

    public GameObject cleaner_info_sample; // ���� ���� ���� �������� ������ �г��� ���ð�(origin)
    public GameObject guard_info_sample; // ���� ���� ���� �������� ������ �г��� ���ð�(origin)
    public GameObject mechanic_info_sample; // ���� ���� ���� �������� ������ �г��� ���ð�(origin)

    // ���� �̸� ���� ���� Ǯ
    public readonly string[] first_name = { "�ٸ�", "��", "�̽�Ÿ", "��", "��", "��", "��", "���۵�", "�ٱ��", "�쿡�϶�", "Ű�", "�����", "��ī����", "����Ǫ��", "������", "��", "ä", "��" };
    public readonly string[] second_name = { "�ƺο�", "��", "Ǫ�̿�", "����", "����", "�̷�", "��������", "�ȹ���", "�p", "�̼���", "����", "����", "����", "����", "ĳ����", "���̴�" };

    // ���� ����� ���� ���� Ǯ
    public readonly string[] birth_place = { "���￪" , "���� ���ε�","û�ҵ�����", "�뼭�� ������", "�˴ܻ� ���ɸ���",
    "ȭ��", "�ƺ� ���", "�̼�ī��" ,"���Ϲ̱�","66�� ����" , "�� �༺","����� ����","Į�ٶ� ����","Ȳ��","�Ƕ�̵�",
        "����","�������","��׽ý�","�丮��","������ ���", "�Ҹ��� Ŀ��","Į������","��������","��ǰ�"};

    protected void Awake()
    {
        cleaner_dic = new Dictionary<int, H_Cleaner>();
        guard_dic = new Dictionary<int, H_Guard>();
        mechanic_dic = new Dictionary<int, H_Mechanic>();
    }
    private void Start()
    {
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";
        guard_hire_gold.text = guard_hire_gold_size + " G";
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";

        StartCoroutine(ShowButtonEnable());
    }
    IEnumerator ShowButtonEnable()
    {
        while (true && !Gamemanager.Instance.buildgame.is_gameover)
        {
            cleaner_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= cleaner_hire_gold_size;
            guard_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= guard_hire_gold_size;
            mechanic_hire_button.enabled = Gamemanager.Instance.buildgame.gold >= mechanic_hire_gold_size && mechanic_dic.Count < 3;

            yield return null;
        }
    }
    // û����
    // û���� (max �κ�) > (�뵿���� �κ�) �� ���� û�Һ� ���� ���� << û�������� �����Ͽ� ����� ��
    public void HireCleaner()
    {
        GameObject cleaner = Gamemanager.Instance.objectpool.GetCleaner();
        cleaner.SetActive(false);
        H_Cleaner cleaner_script = cleaner.GetComponent<H_Cleaner>();

        Gamemanager.Instance.buildgame.PlusGold(-cleaner_hire_gold_size);
        cleaner_hire_gold_size += 500;
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";

        cleaner_script.color = FID.CLEAN;
        cleaner_script.is_working = false;
        cleaner_script.speed = Random.Range(1f, 2f);
        cleaner_script.cleaning_speed = Random.Range(5f, 15f); // ù ���� ����, �ذ��ϰ� �ٽ� ����ϸ� �纯��
        cleaner_script.pay = (int)(50 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // ���� ����

        cleaner_script.id = cleaner_id; // ���̵� ����ȭ
        // û�Һ� ���
        cleaner_dic.Add(cleaner_id++, cleaner_script);

        // �гο� ��ü �߰�(UI)
        Instantiate(cleaner_info_sample, cleaners_contents).GetComponent<Cleaner_Info>().SettingCleanerInfo(cleaner_script); // �θ� ���� ���� �� �ʱ�ȭ
    }
    public void FireCleaner(int id)
    {
        cleaner_dic.Remove(id);
        cleaner_hire_gold_size -= 500;
        cleaner_hire_gold.text = cleaner_hire_gold_size + " G";
    }
    public H_Cleaner GetCleaner() // ���� ��밡���� û�Һθ� �޾ƿ�
    {
        foreach (var cleaner in cleaner_dic)        // val = <cleaner_id, cleaner_script> 
            if (!cleaner.Value.is_working) // ȣ�� ����
            {
                cleaner.Value.is_working = true;
                return cleaner.Value;
            }

        return null; // �θ� ��� ����
    }
    public void ReturnCleaner(int id)
    {
        cleaner_dic[id].gameObject.SetActive(false);
        cleaner_dic[id].is_working = false;
    }
    public void HireGuard()
    {
        GameObject guard = Gamemanager.Instance.objectpool.GetGuard();
        H_Guard guard_script = guard.GetComponent<H_Guard>();

        Gamemanager.Instance.buildgame.PlusGold(-guard_hire_gold_size);
        guard_hire_gold_size += 1000;
        guard_hire_gold.text = guard_hire_gold_size + " G";

        guard_script.color = PID.GUARD;
        guard_script.speed = Random.Range(1f, 2f) * (1 + Gamemanager.Instance.buffmanager.patron_speed_rate);
        guard_script.pay = (int)(100 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // ���� ����

        guard_script.id = guard_id; // ���̵� ����ȭ
        // ���� ���
        guard_dic.Add(guard_id++, guard_script);

        // �гο� ��ü �߰�(UI)
        Instantiate(guard_info_sample, guards_contents).GetComponent<Guard_Info>().SettingGuardInfo(guard_script); // �θ� ���� ���� �� �ʱ�ȭ

        //����� ������ڸ��� 1������ �ٷ� �����ǰ� �� ����
        guard_script.Set(Gamemanager.Instance.buildgame.floors[FID.FIRST][0]);
    }
    public void FireGuard(int id)
    {
        guard_dic.Remove(id);
        guard_hire_gold_size -= 1000;
        guard_hire_gold.text = guard_hire_gold_size + " G";
    }

    public void HireMechanic()
    {
        GameObject mechanic = Gamemanager.Instance.objectpool.GetMechanic();
        H_Mechanic mechanic_script = mechanic.GetComponent<H_Mechanic>();
        remain_mechanic_count++;

        Gamemanager.Instance.buildgame.PlusGold(-mechanic_hire_gold_size);
        mechanic_hire_gold_size += 2500;
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";

        mechanic_script.color = FID.REPAIRSHOP;
        mechanic_script.speed = 2f;
        mechanic_script.fixing_speed = Random.Range(4, 8);
        mechanic_script.fix_elevator_count = 0;
        mechanic_script.pay = (int)(100 * (1 + Gamemanager.Instance.buffmanager.pay_increase_rate)); // ���� ����

        mechanic_script.id = mechanic_id; // ���̵� ����ȭ
        // ���� ���
        mechanic_dic.Add(mechanic_id++, mechanic_script);

        // �гο� ��ü �߰�(UI) ���� �ʿ� ������ ���
        Instantiate(mechanic_info_sample, mechanices_contents).GetComponent<Mechanic_Info>().SettingMechanicInfo(mechanic_script); // �θ� ���� ���� �� �ʱ�ȭ
    }
    public void FireMechanic(int id)
    {
        remain_mechanic_count--;
        mechanic_dic.Remove(id);
        mechanic_hire_gold_size -= 2500;
        mechanic_hire_gold.text = mechanic_hire_gold_size + " G";
    }
    public H_Mechanic GetMechanic() // ���� ��밡���� ��ī���� �޾ƿ�
    {
        foreach (var mechanic in mechanic_dic)        // val = <mechanic_id, mechanic_script> 
            if (!mechanic.Value.is_working) // ȣ�� ����
            {
                mechanic.Value.is_working = true;
                return mechanic.Value;
            }

        return null; // �θ� ��� ����
    }
    public void ReturnMechanic(int id)
    {
        mechanic_dic[id].gameObject.SetActive(false);
        mechanic_dic[id].is_working = false;
    }
    public void PushCleanersInfo() // û�Һε��� ����ĭ�� ����
    {
        cleaners_info.SetActive(true);
        guards_info.SetActive(false);
        mechanices_info.SetActive(false);
        //�̹��� ����
        manager_tab_img.sprite = manager_tab_images[0];
    }
    public void PushGuardsInfo() // ������� ����ĭ�� ����
    {
        cleaners_info.SetActive(false);
        guards_info.SetActive(true);
        mechanices_info.SetActive(false);
        //�̹��� ����
        manager_tab_img.sprite = manager_tab_images[1];
    }
    public void PushMechanicsInfo() // ��������� ����ĭ�� ����
    {
        cleaners_info.SetActive(false);
        guards_info.SetActive(false);
        mechanices_info.SetActive(true);
        //�̹��� ����
        manager_tab_img.sprite = manager_tab_images[2];
    }
    public void EmployeesCareerUp()//�Ϸ簡 ������ ����� ����, �ǽð����� ���ϰ� �ϱ� ���� �Լ�
    {
        for (int i = 0; i < cleaners_contents.childCount; i++)
            cleaners_contents.GetChild(i).GetComponent<Cleaner_Info>().PlusCareer();
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().PlusCareer();
        for (int i = 0; i < mechanices_contents.childCount; i++)
            mechanices_contents.GetChild(i).GetComponent<Mechanic_Info>().PlusCareer();
    }
    public void EmployeesPayUp()//����� ö�����
    {
        for (int i = 0; i < cleaners_contents.childCount; i++)
            cleaners_contents.GetChild(i).GetComponent<Cleaner_Info>().IronBowl();
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().IronBowl();
        for (int i = 0; i < mechanices_contents.childCount; i++)
            mechanices_contents.GetChild(i).GetComponent<Mechanic_Info>().IronBowl();
    }
    public void PatronSpeedUp() // ���� ���� 5���ƿ�
    {
        for (int i = 0; i < guards_contents.childCount; i++)
            guards_contents.GetChild(i).GetComponent<Guard_Info>().PatronUp();
    }
    public int PayDay()
    {
        int pay_sum = 0;

        foreach (var cleaner in cleaner_dic)        // val = <cleaner_id, cleaner_script> 
            pay_sum += cleaner.Value.pay;

        foreach (var guard in guard_dic)            // val = <guard_id, guard_script> 
            pay_sum += guard.Value.pay;

        foreach (var mechanic in mechanic_dic)      // val = <mechanic_id, mechanic_script> 
            pay_sum += mechanic.Value.pay;

        Gamemanager.Instance.buildgame.PlusGold(-pay_sum);

        return pay_sum;
    }
}
