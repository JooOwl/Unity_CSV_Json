using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIObjectLog : MonoBehaviour {

	const int REMOVE_SIZE_MESSAGE = 100;

	public Vector2 m_ScrollPosition;
	public Color m_Color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
	public string m_ScrollString = "";

	List<string> m_listLog = new List<string>();

	public static GUIObjectLog mInstance = null;

	public bool UsetDebugScroll = true;
	
	void Awake()
	{
		if( mInstance == null )
		{
			mInstance = this;
		}

		DontDestroyOnLoad (this);
	}

	public void Log(string msg)
	{
		m_listLog.Add(msg);
	}

	void OnGUI()
	{
		if( UsetDebugScroll == false )
		{
			return;
		}

		Color lastColor = GUI.color;
		
		m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 25));
		GUI.color = m_Color;
		GUILayout.Label(m_ScrollString/*, m_guiInGameLogStyle*/);
		
		int StartIndex = 0;
		if (m_listLog.Count > REMOVE_SIZE_MESSAGE)
			StartIndex = m_listLog.Count - REMOVE_SIZE_MESSAGE;
		
		m_ScrollString = "";
		for (int Index = StartIndex ; Index < m_listLog.Count ; Index++)
		{
			m_ScrollString += m_listLog[Index];
			m_ScrollString += "\n";
		}
		
		GUILayout.EndScrollView();
		
		if (GUILayout.Button("CLEAR"))
			m_listLog.Clear();
		
		GUI.color = lastColor;
	}
}
