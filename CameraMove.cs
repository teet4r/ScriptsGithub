using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    BuildGame buildgame_script;
    ElevatorManager elevator_manager_script;

    bool is_touching = false; // ���� ȭ���� ��ġ ���̸� true
    Rigidbody2D camera_rigid; // ī�޶� �̵��� ���� ������Ģ ����
    Vector2 start_touch; // ��ġ�� ���۵� ��ġ
    Vector2 right_before_touch; // ��ġ�� ���۵Ǳ� ������ ��ġ�Ǿ��� ��ġ

    [SerializeField]
    Management_detail managedetail;
    [SerializeField]
    Management manage;


    ElevatorClass chase_elevator = null; // �i���ִ� ����������
    Coroutine chase_selected_elevator_coroutine;

    protected void Awake()
    {
        buildgame_script = GetComponent<BuildGame>();
        elevator_manager_script = GetComponent<ElevatorManager>();
        camera_rigid = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(CameraMoving());
    }
    IEnumerator CameraMoving() // ȭ�� �巡�׷� ���� ī�޶� ��ġ ���� 
    {
        while (true)
        {
            if (Input.touchCount == 1)
            {
                Touch tempTouchs = Input.GetTouch(0);
                if (tempTouchs.phase == TouchPhase.Began && !is_touching)
                {    //�ش� ��ġ�� ���۵ƴٸ�.
                    is_touching = true;
                    camera_rigid.velocity = Vector3.zero;
                    right_before_touch = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.

                }
                else if (tempTouchs.phase == TouchPhase.Moved)
                {    //�ش� ��ġ�� �����̴� ���̶��.
                    Vector2 touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    camera_rigid.AddForce(new Vector3(0, right_before_touch.y - touchedPos.y, 0) * 300, ForceMode2D.Force);
                    right_before_touch = touchedPos;
                }
                else if (tempTouchs.phase == TouchPhase.Ended) // �����ٸ�
                {
                    Vector2 touchedPos = Camera.main.ScreenToWorldPoint(tempTouchs.position);//get world position.
                    right_before_touch = touchedPos;
                    is_touching = false;
                }
            }
            if (gameObject.transform.position.y < 2) // ������ ����
            {
                gameObject.transform.position = new Vector3(0, 2, -10);
                camera_rigid.velocity = Vector3.zero;
            }
            else if (gameObject.transform.position.y > (buildgame_script.building_top_floor - 2) * 2) // �ְ��� ����
            {
                gameObject.transform.position = new Vector3(0, (buildgame_script.building_top_floor - 2) * 2, -10);
                camera_rigid.velocity = Vector3.zero;
            }
            yield return null;
        }
    }
    public void SelectElevator(ElevatorClass elevator_script) // ���������� �������� ���� ���������͸� ���������� �ش� ���������͸� ����ٴϴ� �ڷ�ƾ���۽����ִ� �Լ�
    {
        chase_elevator = elevator_script;
        chase_selected_elevator_coroutine = StartCoroutine(ChaseElevator(elevator_script.gameObject));
    }
    IEnumerator ChaseElevator(GameObject elevator)
    {
        while (true)
        {
            if (elevator.transform.position.y < 2) // ������ ����
                gameObject.transform.position = new Vector3(camera_rigid.position.x, 2, -10);
            else if (elevator.transform.position.y > (buildgame_script.building_top_floor - 2) * 2) // �ְ��� ����
                gameObject.transform.position = new Vector3(camera_rigid.position.x, (buildgame_script.building_top_floor - 2) * 2, -10);
            else
                camera_rigid.position = new Vector3(camera_rigid.position.x, elevator.transform.position.y, -10);
            yield return null;
        }
    }
    public void StopChaseElevator() // �� �ڷ�ƾ�� �ܺο��� ���� �����ִ� �Լ�
    {
        if (chase_elevator != null)
        {
            StopCoroutine(chase_selected_elevator_coroutine);
            chase_elevator = null;
        }
    }

    public void PushManagePanelOffButton()
    {
        StopChaseElevator();

        if (managedetail.gameObject.activeSelf)
            managedetail.Save();

        if (manage.elevator_btn_selected || manage.employee_btn_selected || manage.building_btn_selected)
        {
            if (manage.btn_idx == 0)
                manage.PushElevator();
            else if (manage.btn_idx == 1)
                manage.PushEmployee();
            else if (manage.btn_idx == 2)
                manage.PushBuilding();
        }
    }
}
