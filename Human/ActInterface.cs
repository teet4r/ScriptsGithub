using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActInterface
{
    IEnumerator ActAtATM(ATM atm);
    /// <���>
    ///  Human
    ///  �� ȹ��
    ///  ----------------
    ///  Normal
    ///  �� ����
    ///  ----------------
    ///  Fat
    ///  �� ����
    ///  ----------------
    ///  Vip
    ///  �� ����
    ///  ----------------
    ///  Mechnic
    ///  ���� ����
    ///  ----------------
    ///  Cleaner
    ///  ���� ����
    ///  ----------------
    ///  Patient
    ///  �� ����
    ///  ----------------
    ///  Sick
    ///  ���� ����
    ///  ----------------
    ///  Thief
    ///  �� ����
    ///  ----------------
    ///  Master
    ///  ���� ����
    /// <���>
    IEnumerator ActAtVM(VendingMachine vm); // Vending Machine
    /// <���>
    ///  Human
    ///  ���� ����
    ///  ----------------
    ///  Normal
    ///  �� ���
    ///  (����Ȯ�� Sick ����)
    ///  ----------------
    ///  Fat
    ///  �� ���
    ///  (����Ȯ�� Sick ����)
    ///  ----------------
    ///  Vip
    ///  �� ���
    ///  (����Ȯ�� Sick ����)
    ///  ----------------
    ///  Mechnic
    ///  ���� ����
    ///  ----------------
    ///  Cleaner
    ///  ���� ����
    ///  ----------------
    ///  Patient
    ///  ????
    ///  ----------------
    ///  Sick
    ///  ���� ����
    ///  ----------------
    ///  Thief
    ///  ���� ����
    ///  ----------------
    ///  Master
    ///  ���� ����
    /// <���>
    IEnumerator ActAtHD(HospitalDoor hd);
    /// <���>
    ///  Human
    ///  ���� ����
    ///  ----------------
    ///  Normal
    ///  ????
    ///  ----------------
    ///  Fat
    ///  ????
    ///  ----------------
    ///  Vip
    ///  ????
    ///  ----------------
    ///  Mechnic
    ///  ���� ����
    ///  ----------------
    ///  Cleaner
    ///  ���� ����
    ///  ----------------
    ///  Patient
    ///  ���� ����
    ///  ----------------
    ///  Sick
    ///  Patient�� ����Ǵ� ���� ������ ��
    ///  �� ����
    ///  ----------------
    ///  Thief
    ///  ���� ����
    ///  ----------------
    ///  Master
    ///  ���� ����
    /// <���>
    IEnumerator ActAtSAD(SmokingAreaDoor sad); // Smoking Area Door
    /// <summary>
    /// ���߿� �������� �߰�
    /// </summary>
    /// <param name="elevator"></param>
    IEnumerator ActAtExit(Exit exit);
    IEnumerator ActAtInfo(Infomation info);
    IEnumerator ActAtWP(WaterPurifier wp); // Water Purifier
    IEnumerator ActAtBS(BookShelf bs); // Book Shelf
    IEnumerator ActAtND(NormalDoor nd); // Normal Door
    IEnumerator ActAtFFExit(FirstFloorExit ffe); // Fisrt Floor Exit
    IEnumerator ActAtRestExit(RestExit re);
    IEnumerator ActAtCounter(Counter c);

    /// <summary>
    /// ��������� ��ȣ�ۿ�
    /// </summary>
    /// <returns></returns>
    IEnumerator ActWithGuard(Human other); // ������� ��ȣ�ۿ�
    IEnumerator ActWithThief(Human other); // ���ϰ��� ��ȣ�ۿ�

    void ActInElevator(ElevatorClass elevator);
    /// <���>
    ///  Human
    ///  ���� �ڷ�ƾ ����
    ///  ----------------
    ///  Normal
    ///  ���� ����
    ///  ----------------
    ///  Fat
    ///  ���� ����
    ///  ----------------
    ///  Vip
    ///  ���� ����
    ///  **Vip_F**
    ///  ����vip�� ���ٰ� �˸� 
    ///  ----------------
    ///  Mechnic
    ///  ���� ����
    ///  ----------------
    ///  Cleaner
    ///  ���� ����
    ///  ----------------
    ///  Patient
    ///  ���� ����
    ///  ----------------
    ///  Sick
    ///  ���� ����
    ///  ----------------
    ///  Thief
    ///  ������ ���ٰ� �˸�
    ///  ----------------
    ///  Master
    ///  ������ ���ٰ� �˸�
    /// <���>
    void ActOffElevator(ElevatorClass elevator);
    /// <���>
    ///  Human
    ///  ��� ����
    ///  �� ������
    ///  Hp ȸ��?
    ///  ��ǳ�� ����
    ///  ���̾� ����
    ///  �̵�
    ///  ----------------
    ///  Normal
    ///  ���� ����
    ///  ----------------
    ///  Fat
    ///  ���� ����
    ///  ----------------
    ///  Vip
    ///  **Vip_F**
    ///  ����vip�� ���ȴٰ� �˸�
    ///  �������
    ///  ���̾� ����
    ///  �̵�
    ///  ----------------
    ///  Mechnic
    ///  ���� ����
    ///  ----------------
    ///  Cleaner
    ///  ���� ����
    ///  ----------------
    ///  Patient
    ///  ���� ����
    ///  ----------------
    ///  Sick
    ///  ���� ����
    ///  ----------------
    ///  Thief
    ///  ������ ���ȴٰ� �˸�
    ///  ���̾� ����
    ///  �̵�
    ///  ----------------
    ///  Master
    ///  ����� ���ȴٰ� �˸�
    ///  ���̾� ����
    ///  �̵�
    /// <���>
}