using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class Proxy
    {
        static public void GAME_REQUEST_USER_LOGIN(GAME_REQ_USER_LOGIN request)
        {
            WWWHelper.Instance.HttpPostWithJson(ProtocolConfig.SERVER_ROOT_URL + "api/user/login", 
                JsonUtility.ToJson(request), 
                Stub.GAME_ANSWER_USER_LOGIN);
        }

        static public void GAME_REQUEST_JSON(GAME_REQ_JSON request)
        {
            WWWHelper.Instance.HttpPostWithJson(ProtocolConfig.SERVER_ROOT_URL + "api/user/json",
                JsonUtility.ToJson(request),
                Stub.GAME_ANSWER_JSON);
        }

        static public void GAME_REQUEST_DUMMY(GAME_REQ_DUMMY request)
        {
            WWWHelper.Instance.HttpPostWithJson(ProtocolConfig.SERVER_ROOT_URL + "api/user/dummy",
                JsonUtility.ToJson(request),
                Stub.GAME_ANSWER_DUMMY);
        }
    }
}
