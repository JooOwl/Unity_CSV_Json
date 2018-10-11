using UnityEngine;
using System;
using System.Collections;

namespace Assets.Script
{
    public class Main : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            GAME_REQ_USER_LOGIN a = new GAME_REQ_USER_LOGIN();
            a.Name = "xxxx";
            //Proxy.GAME_REQUEST_USER_LOGIN(a);

//             GAME_REQ_JSON b = new GAME_REQ_JSON();
//             b.UserKey = "내키는 유니티다";
//             b.Dummy = "키 테스트";

            //Proxy.GAME_REQUEST_JSON(b);

            GAME_REQ_DUMMY b = new GAME_REQ_DUMMY();
            b.dummy.a = "asd";
            b.dummy.b = 123;
            b.c = "efg";

            Proxy.GAME_REQUEST_DUMMY(b);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}