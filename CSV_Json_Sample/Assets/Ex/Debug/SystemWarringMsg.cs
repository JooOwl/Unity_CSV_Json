using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SystemWarringMsg : MonoBehaviour {

	private string m_DebugText;
	const string m_ConstNewLine = "\r\n";

	void Awake()
	{
		DontDestroyOnLoad (this);

		Application.logMessageReceived += LogCallback;

		// Application.logMessageReceived -= LogCallback; 
		//Application.RegisterLogCallback(new Application.LogCallback(LogCallback));
	}

	void LogCallback(string InCondition, string InStacktrace, LogType InType)
	{
		if (InType == LogType.Exception || InType == LogType.Error || InType == LogType.Warning)
		{
			m_DebugText = InType.ToString() + "-" + InCondition +
				m_ConstNewLine + InStacktrace + m_ConstNewLine + m_ConstNewLine + m_DebugText;

			if (InType == LogType.Exception)
			{
				Alert("Bug Report", m_DebugText);
			}

			if (InType == LogType.Exception)
			{
				//StartCoroutine(CoroutineUploadReport());
			}
		}
	}

	public void Alert(string title, string message)
	{
		#if UNITY_EDITOR
		EditorUtility.DisplayDialog(title, message, "OK", "Cancel");
		#else
		Debug.LogError(message);
		#endif
	}

}
