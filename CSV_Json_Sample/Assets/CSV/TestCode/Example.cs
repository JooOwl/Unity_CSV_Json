using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using Sinbad;

public class CsvLoader
{
    static CsvLoader _instance;

    public static CsvLoader instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CsvLoader();
                _instance.LoadAll("Assets/Resources/");
            }
            return _instance;
        }
    }

    public Dictionary<string, GreetingWords> _greetingWords;
    public Dictionary<int, DefaultValue> _defaultValue;

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


        return true;
    }
}

// for default table
public class DefaultValue
{
    public int id;  // came from csv header
    public float Value; // came from csv header
    public string StrValue; // came from csv header
}

// for enum table
public class GreetingWords
{
    public string Key;  // came from csv header
    public int Cost;    // came from csv header
    public string Word1;    // came from csv header
    public string Word2;    // came from csv header
    public string Word3;    // came from csv header
    public string Words;    // came from csv header

    // extended function ( custom added ) -------- for using group words
    List<string> _WordsArray = null;
    public List<string> WordsArray
    {
        get
        {
            if (_WordsArray == null)
                _WordsArray = new List<string>(Words.Split(','));
            return _WordsArray;
        }
    }

}

public class MyObject
{
    public string Name;
    public int Level;
    public float Dps;
    public enum Colour
    {
        Red = 1,
        Green = 2,
        Blue = 3,
        Black = 4,
        Purple = 15
    }
    public Colour ShirtColour;

    public MyObject()
    {

    }
    public MyObject(string _name, int _level, float _dsp, Colour _color)
    {
        this.Name = _name;
        this.Level = _level;
        this.Dps = _dsp;
        this.ShirtColour = _color;
    }

    public override string ToString()
    {
        return Name + "," + Level + "," + Dps + "," + ShirtColour;
    }
}

public class Example : MonoBehaviour {

    void Start()
    {
        RunExample1("happy");   // simple way
        RunExample2("happy");   // another way using find ( referencing way )
        RunExample3(13);    // 13 is id & direct way using exact number that already knew
        RunExampleSaveLoad();
    }

    void RunExample1(string input)
    {
        // 1. check include word (Simple way)
        if (CsvLoader.instance._greetingWords[input].WordsArray.Contains("peaceful"))
        {
            Debug.Log(input + " included peachful word");
        }
        Debug.Log(CsvLoader.instance._greetingWords["hello"].Words);
    }


    void RunExample2(string input)
    {
        // a way to get item ( you don't know exact id from the value)
        var find = CsvLoader.instance._defaultValue.Values.FirstOrDefault(va => va.StrValue == input);
        if (find == default(DefaultValue))
            Debug.Log("not found " + input);

        // same result but another way
        find = CsvLoader.instance._defaultValue[12];    // 12 is id that you write on

        if (CsvLoader.instance._greetingWords[find.StrValue].WordsArray.Contains("peaceful"))
        {
            Debug.Log(find.id + " included peachful word," + find.StrValue);
        }

        Debug.Log(find.id);
        Debug.Log(find.StrValue);

    }

    void RunExample3(int id)
    {
        // same result but another way
        var find = CsvLoader.instance._defaultValue[id];    // id that you write on

        if (CsvLoader.instance._greetingWords[find.StrValue].WordsArray.Contains("peaceful"))
        {
            Debug.Log(find.id + " included peachful word," + find.StrValue);
        }

        Debug.Log(find.id);
        Debug.Log(find.StrValue);

    }

    // ////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void RunExampleSaveLoad()
    {
        // Single Data Test
        var obj = new MyObject("Steve", 20, 1002.50f, MyObject.Colour.Red);

        _SaveToLocal(obj);
        _SaveToAssets(obj);

        var loadObj = _LoadFromAssets();
        Debug.Log("Test LoadFromAssets:" + loadObj.ToString());


        Debug.Log("---------------------");
        // Multiple Data Test
        List<MyObject> lst = new List<MyObject>();
        lst.Add(new MyObject("Steve1", 30, 4002.50f, MyObject.Colour.Red));
        lst.Add(new MyObject("Ben", 41, 1102.50f, MyObject.Colour.Blue));
        lst.Add(new MyObject("Steve3", 20, 1202.50f, MyObject.Colour.Red));

        _SaveListToAssets(lst, "Assets/Resources/CsvRecords.csv");

        List<MyObject> lstLoad = Sinbad.CsvUtil.LoadObjects<MyObject>("Assets/Resources/CsvRecords.csv");
        foreach (var data in lstLoad)
        {
            Debug.Log("Data:" + data.ToString());
        }

    }

    void _SaveToLocal(MyObject obj)
    {
        CsvUtil.SaveObject<MyObject>(obj, Application.persistentDataPath + "/CsvForSaveData.csv");
        Debug.Log("Test SaveToLocal");
    }

    void _SaveToAssets(MyObject obj)
    {
        CsvUtil.SaveObject<MyObject>(obj, "Assets/Resources/CsvForSaveData.csv");
        Debug.Log("Test SaveToAssets");
    }

    public MyObject _LoadFromAssets()
    {
        MyObject obj = new MyObject();
        CsvUtil.LoadObject<MyObject>("Assets/Resources/CsvForSaveData.csv", ref obj);
        return obj;
    }

    void _SaveListToAssets(List<MyObject> lst, string filename)
    {
        CsvUtil.SaveObjects<MyObject>(lst, filename);
        Debug.Log("Test SaveListToAssets");
    }
}
