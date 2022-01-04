using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����͸� �����ϱ� ���� Ŭ����
/// </summary>
public class DBManager : MonoBehaviour
{
    private static DBManager instance;
    public static DBManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBManager();
                instance.Init();
            }
            return instance;
        }
    }

    private List<Dictionary<string, object>> messageDB = new List<Dictionary<string, object>>();

    private void Init()
    {
        messageDB = CSVReader.Read("DB/Message");
    }

    /// <summary>
    /// id�� �ش��ϴ� ������ list ���·� ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<string> FindMessages(string id)
    {
        List<string> list = new List<string>();

        for (int i = 0; i < messageDB.Count; i++)
        {
            if (messageDB[i]["ID"].ToString() == id)
            {
                list.Add(messageDB[i]["Content"].ToString());
            }
        }

        return list;
    }
}
