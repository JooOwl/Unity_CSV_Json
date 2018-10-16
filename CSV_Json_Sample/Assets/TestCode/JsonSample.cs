using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

using JsonEx;

public class TestJsonData
{
    public int Key;
    public string str01;
    public string str02;
    public List<string> lst_str;
}

public class JsonSample : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        SDebugLog.LogString("ASDF", LogColor.BLACK);
        SDebugLog.LogToType(DebugLogType.Screen, "ASDFASDF");

        Dictionary<int, TestJsonData> temp = new Dictionary<int, TestJsonData>();

        for(int i=0; i<10;++i)
        {
            TestJsonData ddd = new TestJsonData();

            ddd.lst_str = new List<string>();

            ddd.Key = i;
            ddd.str01 = i.ToString();
            ddd.str02 = i.ToString();

            for(int j=0; j<4; ++j)
            {
                ddd.lst_str.Add(j.ToString());
            }

            temp.Add(ddd.Key, ddd);
        }

        JsonUtilEx.SaveJsonObject(temp, Application.persistentDataPath, "TEST_JSON");

        Dictionary<int, TestJsonData> tem = JsonUtilEx.LoadAndDeserialize<Dictionary<int, TestJsonData>>(Application.persistentDataPath, "TEST_JSON");

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void MakeTestJson()
    {

    }
}
