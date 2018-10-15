using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using UnityEngine;

using Sinbad;

public class CSVLoader
{
    static CSVLoader _instance;

    public static CSVLoader instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CSVLoader();
                _instance.LoadAll("Assets/Resources/");
            }
            return _instance;
        }
    }

    public Dictionary<string, GreetingWords> _greetingWords;
    public Dictionary<int, DefaultValue> _defaultValue;
    public Dictionary<string, KshWords> _kshWords;

    bool LoadAll(string path)
    {
        // load & make a table with GreetingWords csv
        var loadGreetingWords = CsvUtil.LoadObjects<GreetingWords>(path + "TestDataForCSV - GreetingWords.csv");
        _greetingWords = new Dictionary<string, GreetingWords>();
        for (int i = 0; i < loadGreetingWords.Count; i++)
        {
            _greetingWords.Add(loadGreetingWords[i].Key, loadGreetingWords[i]);
        }

        // load & make a table with defaultValue csv
        var LoadDefault = CsvUtil.LoadObjects<DefaultValue>(path + "TestDataForCSV - DefaultValue.csv");
        _defaultValue = new Dictionary<int, DefaultValue>();
        foreach (var info in LoadDefault)
            _defaultValue.Add(info.id, info);

        //ksh
        var loadKshWords = CsvUtil.LoadObjects<KshWords>(path + "TestDataKSH.csv");
        _kshWords = new Dictionary<string, KshWords>();
        for (int i = 0; i < loadKshWords.Count; i++)
        {
            _kshWords.Add(loadKshWords[i].Key, loadKshWords[i]);
        }

        return true;
    }
}
