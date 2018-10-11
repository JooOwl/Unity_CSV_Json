using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class Stub
    {
        static public bool InitializeStub()
        {
            if(WWWHelper.Instance == null)
            {
                return false;
            }

            return true;
        }

        public static void GAME_ANSWER_USER_LOGIN(string jsonData)
        {
            GAME_ANS_USER_LOGIN answer = JsonUtility.FromJson<GAME_ANS_USER_LOGIN>(jsonData);
            Debug.Log(jsonData);
        }

        public static void GAME_ANSWER_JSON(string jsonData)
        {
            GAME_ANS_JSON answer = JsonUtility.FromJson<GAME_ANS_JSON>(jsonData);
            Debug.Log(jsonData);
        }

        public static void GAME_ANSWER_DUMMY(string jsonData)
        {
            GAME_REQ_DUMMY answer = JsonUtility.FromJson<GAME_REQ_DUMMY>(jsonData);
            Debug.Log(jsonData);
        }
    }
}
