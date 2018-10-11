using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace Assets.Script
{ 
    class WWWHelper : MonoBehaviour
    {
        /// <summary>
        /// 해당 클래스의 Thread-Safe 를 위한 lock 객체
        /// </summary>
        static object lockObject = new object();

        /// <summary>
        /// HTTP Request 성공시에 콜백 될 스텁 델리게이트 타입
        /// </summary>
        /// <param name="jsonData"></param>
        public delegate void StubDelegate(string jsonData);

        /// <summary>
        /// HTTP Reuqest 가 성공 했지만 대응 되는 스텁이 없을 경우 콜백 될 델리게이트 타입
        /// </summary>
        /// <param name="www"></param>
        public delegate void NoStubDelegate(WWW www);

        /// <summary>
        /// HTTP Request 실패시에 콜백 될 델리게이트 타입
        /// </summary>
        /// <param name="www"></param>
        public delegate void RequestErrorDelegate(WWW www);

        /// <summary>
        /// HTTP Request 실패시에 콜백
        /// </summary>
        public event RequestErrorDelegate OnRequestError;

        /// <summary>
        /// Stub이 설정 되어 있지 않았을 때 콜백 됨
        /// </summary>
        public event NoStubDelegate OnNoStub;

        /// <summary>
        /// 이 클래스의 유일한 인스턴스
        /// </summary>
        static WWWHelper current = null;

        /// <summary>
        /// 객체를 생성하기 위한 Unity GameObject
        /// </summary>
        static GameObject container = null;

        /// <summary>
        /// 이 클래스의 유일한 인스턴스를 가져온다.
        /// </summary>
        public static WWWHelper Instance
        {
            get
            {
                lock(lockObject)
                {
                    if (current == null)
                    {
                        container = new GameObject();
                        container.name = "WWWHelper";
                        current = container.AddComponent(typeof(WWWHelper)) as WWWHelper;
                    }
                    return current;
                }
            }
        }

        /// <summary>
        /// HTTP POST 방식 통신 (json 데이터를 사용한다.)
        /// </summary>
        /// <param name="uri">요청 되는 엔드 포인트 URI</param>
        /// <param name="jsonData">HTTP Body에 실릴 Serialize된 json 데이터</param>
        public void HttpPostWithJson(string uri, string jsonData, StubDelegate stub)
        {
            // 스텁 설정이 되어 있지 않으면 exception을 throw 한다.
            if(stub == null)
            {
                throw new System.Exception("Stub은 반드시 설정 해야 합니다.");
            }

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");
            WWW w = new WWW(uri, Encoding.UTF8.GetBytes(jsonData.ToCharArray()), header);
            StartCoroutine(WaitForRequest(w, stub));
        }

        /// <summary>
        /// 통신 처리를 위한 코루틴
        /// </summary>
        /// <param name="www">HTTP 요청시에 사용했던 WWW 객체</param>
        /// <returns></returns>
        private IEnumerator WaitForRequest(WWW www, StubDelegate stub)
        {
            // 응답이 올떄까지 기다림
            yield return www;

            // HTTP Request 가 실패 함
            if(www.error != null)
            {
                // RequestError 델리게이트가 세팅 되어 있지 않았을 때 처리
                Debug.Assert(OnRequestError != null, "RequestError 델리게이트가 설정 되어 있지 않음");

                // 콜백
                OnRequestError(www);
            }

            if(stub != null)
            {
                stub(www.text);
            }
            else
            {
                Debug.Assert(OnNoStub != null, "NoStub 델리게이트가 설정 되어 있지 않음");
                OnNoStub(www);
            }

            // 통신 해제
            www.Dispose();
        }
    }
}
