using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sinbad;

using System.IO;
using System.Text;

public class CSVUtilSample : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        TestLoadSingle();
        TestLoadSingleWithHeader();
        TestLoadSingleEmbeddedCommas();
        TestLoadMulti();
        TestLoadMultiWithSpacesInHeader();

        TestSaveSingle();
        TestSaveSingleQuoted();
        TestSaveMulti();
    }

    #region TestObject
    public class TestObject
    {
        public string StringField;
        public int IntField;
        public float FloatField;
        public enum Colour
        {
            Red = 1,
            Green = 2,
            Blue = 3,
            Purple = 15
        }
        public Colour EnumField;

        public TestObject() { }
        public TestObject(string s, int i, float f, Colour c)
        {
            StringField = s;
            IntField = i;
            FloatField = f;
            EnumField = c;
        }
    }

    public void TestLoadSingle()
    {
        // It's ok to have newline with padding at start, should trim field names (not values)
        // Include spaces in string field value to prove all content preserved
        // Also put fields out of order
        string csvData = @"String Field, FloatField ,#Description, Int  Field ,   EnumField
""This,has,commas,in it"",2.34,Something ignored,35,Red
Hello World,256.25,""Notes here"",10003,Purple
Zaphod Beeblebrox,3.1,""Amazingly amazing"",000359,Green";

        TestObject t = new TestObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }

        Debug.Log(t.ToString());
    }

    public void TestLoadSingleWithHeader()
    {
        // Test that we can include a header line if we want
        string csvData = @"#Field,#Value,#Description
			StringField, Hello World  ,This is an ignored description,also ignored
			EnumField,Blue,Something Something
			IntField,1234,Comment here
			FloatField,1.5,More commenting";

        TestObject t = new TestObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }

        Debug.Log(t.ToString());
    }

    public void TestLoadSingleEmbeddedCommas()
    {
        // It's ok to have newline with padding at start, should trim field names (not values)
        // Include spaces in string field value to prove all content preserved
        // Also put fields out of order
        string csvData = @"StringField,""Commas, commas everywhere,abcd"",Ignored,ignored
			EnumField,Purple,Something Something
			IntField,-5002,Comment here
			FloatField,-3.142,Pi Pi Baby";

        TestObject t = new TestObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }

        Debug.Log(t.ToString());
    }

    public void TestLoadMulti()
    {
        // Header first, then N values
        // #Field headers are ignored
        // This time we don't want any prefixing since not trimmed
        string csvData = @"StringField,FloatField,#Description,IntField,EnumField
""This,has,commas,in it"",2.34,Something ignored,35,Red
Hello World,256.25,""Notes here"",10003,Purple
Zaphod Beeblebrox,3.1,""Amazingly amazing"",000359,Green";

        List<TestObject> objs;
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                objs = CsvUtil.LoadObjects<TestObject>(sr);
            }
        }
        
        TestObject t = objs[0];
        t = objs[1];
        t = objs[2];

        Debug.Log(t.ToString());
    }
    
    public void TestLoadMultiWithSpacesInHeader()
    {
        // Header first, then N values
        // #Field headers are ignored
        // This time we don't want any prefixing since not trimmed
        string csvData = @"String Field, FloatField ,#Description, Int  Field ,   EnumField
                ""This,has,commas,in it"",2.34,Something ignored,35,Red
                Hello World,256.25,""Notes here"",10003,Purple
                Zaphod Beeblebrox,3.1,""Amazingly amazing"",000359,Green";

        List<TestObject> objs;
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                objs = CsvUtil.LoadObjects<TestObject>(sr);
            }
        }
        
        TestObject t = objs[0];
        t = objs[1];
        t = objs[2];

        Debug.Log(t.ToString());
    }

    public void TestSaveSingle()
    {

        var obj = new TestObject("Hello there", 123, 300.2f, TestObject.Colour.Blue);
        using (var stream = new MemoryStream(256))
        {
            using (var w = new StreamWriter(stream))
            {
                CsvUtil.SaveObject(obj, w);
                w.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(stream);
                var content = r.ReadToEnd();
                var expected = @"StringField,Hello there
                                IntField,123
                                FloatField,300.2
                                EnumField,Blue";
            }
        }
    }
    
    public void TestSaveSingleQuoted()
    {

        var obj = new TestObject("Hello, there", 123, 300.2f, TestObject.Colour.Blue);
        using (var stream = new MemoryStream(256))
        {
            using (var w = new StreamWriter(stream))
            {
                CsvUtil.SaveObject(obj, w);
                w.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(stream);
                var content = r.ReadToEnd();
                var expected = @"StringField,""Hello, there""
                                IntField,123
                                FloatField,300.2
                                EnumField,Blue";
            }
        }
    }
    
    public void TestSaveMulti()
    {

        List<TestObject> objs = new List<TestObject>() {
                new TestObject("Hello there", 123, 300.2f, TestObject.Colour.Blue),
                new TestObject("This,has,commas", 42, 12.123f, TestObject.Colour.Purple),
                new TestObject("Semi;colons", 40001, -75.2f, TestObject.Colour.Green),
            };
        using (var stream = new MemoryStream(256))
        {
            using (var w = new StreamWriter(stream))
            {
                CsvUtil.SaveObjects(objs, w);
                w.Flush();

                stream.Seek(0, SeekOrigin.Begin);
                var r = new StreamReader(stream);
                var content = r.ReadToEnd();
                var expected = @"StringField,IntField,FloatField,EnumField
                            Hello there,123,300.2,Blue
                            ""This,has,commas"",42,12.123,Purple
                            ""Semi;colons"",40001,-75.2,Green";
            }
        }
    }

    #endregion

    #region TestPropertiesObject

    public class TestPropertiesObject
    {
        public string StringProperty { get; private set; }
        public int IntProperty { get; private set; }
        public float FloatProperty { get; private set; }
        public enum Colour
        {
            Red = 1,
            Green = 2,
            Blue = 3,
            Purple = 15
        }
        public Colour EnumProperty { get; private set; }

        public TestPropertiesObject() { }
        public TestPropertiesObject(string s, int i, float f, Colour c)
        {
            StringProperty = s;
            IntProperty = i;
            FloatProperty = f;
            EnumProperty = c;
        }
    }

    public void TestLoadSingleWithSpacesInHeader()
    {
        // It's ok to have newline with padding at start, should trim field names (not values)
        // Include spaces in string field value to prove all content preserved
        string csvData = @"StringProperty, Hello World  ,This is an ignored description,also ignored
			  Enum Property,Blue,Something Something
			 Int  Property  ,1234,Comment here
			FloatProperty  ,1.5,More commenting";

        TestPropertiesObject t = new TestPropertiesObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }
    }

    public void TestLoadProperties()
    {
        // It's ok to have newline with padding at start, should trim field names (not values)
        // Include spaces in string field value to prove all content preserved
        string csvData = @"StringProperty, Hello World  ,This is an ignored description,also ignored
			EnumProperty,Blue,Something Something
			IntProperty,1234,Comment here
			FloatProperty,1.5,More commenting";

        TestPropertiesObject t = new TestPropertiesObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }
    }
    #endregion

    #region TestPropertiesAndFieldsObject
    public class TestPropertiesAndFieldsObject
    {
        public string StringMember { get; protected set; }
        private string stringMember;
        public string GetStringField() { return stringMember; }

        public int IntMember { get; set; }
        protected int intMember;
        public int GetIntField() { return intMember; }

        public float FloatMember { get; private set; }
        public float floatMember;
        public float GetFloatField() { return floatMember; }

        public enum Colour
        {
            Red = 1,
            Green = 2,
            Blue = 3,
            Purple = 15
        }
        public Colour EnumMember { get; private set; }
        private Colour enumMember;
        public Colour GetEnumField() { return enumMember; }

        public TestPropertiesAndFieldsObject() { }
        public TestPropertiesAndFieldsObject(string s, int i, float f, Colour c)
        {
            StringMember = stringMember = s;
            IntMember = intMember = i;
            FloatMember = floatMember = f;
            EnumMember = enumMember = c;
        }
    }

    public void TestLoadPropertiesAndFieldsSameName()
    {
        // It's ok to have newline with padding at start, should trim field names (not values)
        // Include spaces in string field value to prove all content preserved
        string csvData = @"StringMember, Hello World  ,This is an ignored description,also ignored
			EnumMember,Blue,Something Something
			IntMember,1234,Comment here
			FloatMember,1.5,More commenting";

        TestPropertiesAndFieldsObject t = new TestPropertiesAndFieldsObject();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }
    }
    #endregion

    #region TestStruct
    public struct TestStruct
    {
        public float f;
        public string s;
    }

    public void TestLoadStruct()
    {
        string csvData = @"s, Hello World  ,This is an ignored description,also ignored
			f,1234.5,Comment here";

        TestStruct t = new TestStruct();

        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(csvData)))
        {
            using (var sr = new StreamReader(ms))
            {
                CsvUtil.LoadObject(sr, ref t);
            }
        }
    }
    #endregion
}
