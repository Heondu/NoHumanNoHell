using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 데이터를 관리하기 위한 클래스
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
    /// id에 해당하는 대사들을 list 형태로 전부 반환하는 함수
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
