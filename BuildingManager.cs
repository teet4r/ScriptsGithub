using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour
{
    public InputField cleaner_limit_size_ipf; // û�Һ� ȣ�� �Ӱ谪 ��ǲ�ʵ�

    public int cleaner_limit_size;

    private void Start()
    {
        cleaner_limit_size_ipf.characterLimit = 3;
    }

    public void SaveBuildingSetting() // ���ο� ���� ������ ���� (��ư Ŭ��)
    {
        try
        {
            int temp_cleaner_limit_size = int.Parse(cleaner_limit_size_ipf.text);

            if (temp_cleaner_limit_size < 0)
                temp_cleaner_limit_size = 0;
            else if(temp_cleaner_limit_size > 100)
                temp_cleaner_limit_size = 100;

            cleaner_limit_size = temp_cleaner_limit_size;
        }
        catch
        {
        }
        finally
        {
            cleaner_limit_size_ipf.text = cleaner_limit_size.ToString();
        }
    }
}
