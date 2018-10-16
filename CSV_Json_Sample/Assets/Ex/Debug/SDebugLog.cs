using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public enum LogColor
{
    RED,
    ORAGE,
    DULL_GREEN,
    GREEN,
    BRIGHT_BLUE,
    YELLOW,
    DULL_RED,
    PURPLE,
    GREEN_ISH,
    WHITE,
    BLACK,
    DARK_PURPLE,
    BROWN_ISH,
    BROWN_ISH2,
    DIRTY_GREEN,
    WASH_RAD,
    CERULEAN_BLUE,
    CERULEAN_BLUE2,
}

public class SDebugLog : MonoBehaviour {

	static List<string> mLines = new List<string>();

	static bool mRayDebug = false;
	static public void LogClear () { mLines.Clear(); }
	static SDebugLog mInstance = null;

    static Dictionary<LogColor, string> dic_color = new Dictionary<LogColor, string>();

	static public bool debugRaycast
	{
		get
		{
			return mRayDebug;
		}
		set
		{
			if (Application.isPlaying)
			{
				mRayDebug = value;
				if (value) CreateInstance();
			}
		}
	}

    static public void LogWarring(params object[] objs)
    {
        string text = "";

        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }

        Debug.LogWarning(text);
    }

    static public void LogError (params object[] objs)
	{
		string text = "";

		for (int i = 0; i < objs.Length; ++i)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text += ", " + objs[i].ToString();
			}
		}

		Debug.LogError (text);
	}

	void ClearToTime()
	{
		mLines.Clear();
	}

	static public void LogView (params object[] objs)
	{
		string text = "";
		
		for (int i = 0; i < objs.Length; ++i)
		{
			if (i == 0)
			{
				text += objs[i].ToString();
			}
			else
			{
				text += ", " + objs[i].ToString();
			}
		}
		LogString(text);
	}

    static void SetTextColor()
    {
        if(dic_color.Count <= 0)
        {
            dic_color.Add(LogColor.RED, "FF0000");
            dic_color.Add(LogColor.ORAGE, "FF9933");
            dic_color.Add(LogColor.DULL_GREEN, "669999");
            dic_color.Add(LogColor.GREEN, "55CC29");
            dic_color.Add(LogColor.BRIGHT_BLUE, "99CCFF");
            dic_color.Add(LogColor.YELLOW, "FFFF00");
            dic_color.Add(LogColor.DULL_RED, "CC6699");
            dic_color.Add(LogColor.PURPLE, "D96EBF");
            dic_color.Add(LogColor.GREEN_ISH, "66FFCC");
            dic_color.Add(LogColor.WHITE, "FFFFFF");
            dic_color.Add(LogColor.BLACK, "000000");
            dic_color.Add(LogColor.DARK_PURPLE, "666699");
            dic_color.Add(LogColor.BROWN_ISH, "AC5930");
            dic_color.Add(LogColor.BROWN_ISH2, "AC5980");
            dic_color.Add(LogColor.DIRTY_GREEN, "BAC74A");
            dic_color.Add(LogColor.WASH_RAD, "E38B81");
            dic_color.Add(LogColor.CERULEAN_BLUE, "E65751");
            dic_color.Add(LogColor.CERULEAN_BLUE2, "00008F");
        }
    }

	static public void LogString (string text, LogColor logColor = LogColor.BLACK)
	{
        SetTextColor();

        string logtext = string.Format("<color=#{0}>{1}</color>", dic_color[logColor], text);

        if (Application.isPlaying)
		{
			if (mLines.Count > 30) mLines.RemoveAt(0);

			mLines.Add(text);

			CreateInstance();

			Debug.Log(logtext);
		}
		else Debug.Log(logtext);

		if( mInstance != null)
		{
			mInstance.CancelInvoke ( "ClearToTime" );
			mInstance.Invoke( "ClearToTime", 60f);
		}
	}

	static public void CreateInstance ()
	{
		if (mInstance == null)
		{
			GameObject go = new GameObject("_SDebug_Log");
			mInstance = go.AddComponent<SDebugLog>();
			DontDestroyOnLoad(go);
		}
	}

	void OnGUI()
	{
		if (mLines.Count == 0)
		{
            /*
			if (mRayDebug && UICamera.hoveredObject != null && Application.isPlaying)
			{
				GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", ""));
			}
            */
		}
		else
		{
			for (int i = 0, imax = mLines.Count; i < imax; ++i)
			{
				GUILayout.Label(mLines[i]);
			}
		}
	}
}
