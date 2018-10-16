using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace JsonEx
{
    public static class JsonUtilEx
    {
        public static T LoadAndDeserialize<T>(string path, string fileName)
        {
            //string filePath = string.Format(path, Application.dataPath);

            string JsonFileName = string.Format("{0}/{1}", path, fileName);

            var streamReader = new System.IO.StreamReader(JsonFileName);
            string data = streamReader.ReadToEnd();
            streamReader.Close();

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SaveJsonObject<T>(T obj, string _path, string _filename)
        {
            string JsonFileName = string.Format("{0}/{1}", _path, _filename);

            StreamWriter stream_write;
            stream_write = File.CreateText(JsonFileName);

            string jsonString = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

            stream_write.Write(jsonString);

#if UNITY_EDITOR
            //Application.OpenURL(_path);
#endif
            stream_write.Close();
        }
    }
}
