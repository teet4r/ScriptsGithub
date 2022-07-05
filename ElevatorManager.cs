using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ***********************
// attached in Main Camera
// ***********************
public class ElevatorManager : MonoBehaviour
{
    public GameObject elevator_origin;
    public GameObject managepanel;

    public List<List<GameObject>> elevators;
    public List<List<Button>> elevators_simple_btn;
    public Button ele_info_simple; // ������
    public Transform[] btnparent;

    public Vector2 elevator_spawn_point { get; private set; } = new Vector2(-2.3f, 0.2f);

    protected void Awake()
    {
        elevators = new List<List<GameObject>>(3);
        elevators_simple_btn = new List<List<Button>>(3);
        for (int i = 0; i < 3; i++)
            elevators.Add(new List<GameObject>());
        for (int i = 0; i < 3; i++)
            elevators_simple_btn.Add(new List<Button>());
    }
    public void MakeElevator(int line) // 0,1,2
    {
        GameObject ele_clone = Instantiate(elevator_origin);
        elevators[line].Add(ele_clone);
        ele_clone.name = elevators[line].Count + "ȣ��"; // �⺻������ �� ���δ� 1ȣ��, 2ȣ�� �̷��� �̸��� ������
        ElevatorClass ele_clone_script = ele_clone.GetComponent<ElevatorClass>();
        ele_clone.GetComponent<Rigidbody2D>().position = elevator_spawn_point + Vector2.right * 0.85f * line;
        ele_clone_script.Set(line, Gamemanager.Instance.buildgame.building_bottom_floor, Gamemanager.Instance.buildgame.building_top_floor);
        MakeElevatorSimpleButton(btnparent[line], ele_clone_script, line);
    }
    public void MakeElevatorSimpleButton(Transform parent, ElevatorClass elevator_script,int line) // ���������� ���� �гο��� �����ϰ� �������� ��ư��
    {
        Button new_btn = Instantiate(ele_info_simple, parent);
        new_btn.GetComponent<EleSimpleInfo>().Init(elevator_script);
        elevators_simple_btn[line].Add(new_btn);
        
        // ��Ÿ ����
    }
    public void PlusPoint() // ���� ���ʽ� �޴³� ��
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < elevators[i].Count; j++) 
                elevators[i][j].GetComponent<ElevatorClass>().cur_point += 3;
    }
    public void HalfVolume() // ����� ��ȸ�� �Ÿ��α�� 
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < elevators[i].Count; j++)
                elevators[i][j].GetComponent<ElevatorClass>().max_volume /= 2;
    }
}
