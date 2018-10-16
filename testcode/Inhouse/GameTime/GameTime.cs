using UnityEngine;
using System.Collections;

using System;

public class GameTime : MonoBehaviour
{
	public static bool pause = false;
	
	public static float delta = 0.0f;					//현재 델타값.
	public static float fixedDelta = 0.0f;				//현재 Fixed 델타값.
														
	public static float gameTime = 0.0f;				//게임 실행 시간.		
														
	public static float playTime = 0.0f;				//게임 접속 후 흐른 시간.
	public static float stageTime = 0.0f;				//전장에 들어온 후로 지난 시간.
	public static float roundTime = 0.0f;				//현재 라운드 시간.
	public static int frameCount = 0;					//현재 프레임 카운트.
	
	public static float ignoreTimeSclaeTime
	{
		get
		{
			return Time.time;
		}
	}
	
	private static bool m_IsPlayingStage = false;		//스테이지 진행 여부.
	private static bool m_IsPlayingRound = false;		//라운드 진행 여부.
	
	private static ulong m_UTCStandard = 0;
		
	public static float standardRunTime = 0; 
	
	public static float runTime
	{
		get
		{
			return Time.realtimeSinceStartup-standardRunTime;
		}
	}
	
#region Temp
	
	
#endregion
	
	void Awake()
	{
		if( this.transform.parent == null )
		{
			DontDestroyOnLoad(this);
		}
			
		StartGame();
	}
	
	void OnApplicationQuit()
	{
		EndGame();
	}
	
	void Update()
	{
		delta = pause == true ? 0.0f : Time.deltaTime;
		fixedDelta = pause == true ? 0.0f : Time.fixedDeltaTime;
		
		playTime += delta;
		
		stageTime += m_IsPlayingStage == true ? delta : 0.0f;
		
		roundTime += m_IsPlayingRound == true ? delta : 0.0f;
				
		frameCount = pause == true ? 0 : Time.frameCount;
	}
	
	public static void SetUTCStandard( ulong std )
	{
		m_UTCStandard = std;
		
		standardRunTime = Time.realtimeSinceStartup;
	}
	
	public static float UTC2Sec( ulong t )
	{
		float diff = (float)(t-m_UTCStandard);
		
		return diff - (Time.realtimeSinceStartup-standardRunTime);
	}

	public static float PrevUTC2Sec( ulong t )
	{
		float diff = (float)(m_UTCStandard - t);

		return diff - (Time.realtimeSinceStartup-standardRunTime);
	}
	
	public static int GetCurrentmonth( )
	{	
		System.DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0);
	    dtDateTime = dtDateTime.AddSeconds( m_UTCStandard ).ToLocalTime();
		
		return dtDateTime.Month;
	}
	
	public static int GetCurrntUTCTime()
	{
		return (int)(m_UTCStandard + (Time.realtimeSinceStartup-standardRunTime));
	}
	
#region To days
    public static double Milliseconds2Days(double milliseconds)
    {
		return TimeSpan.FromMilliseconds(milliseconds).TotalDays;
    }

    public static double Seconds2Days(double seconds)
    {
	return TimeSpan.FromSeconds(seconds).TotalDays;
    }

    public static double Minutes2Days(double minutes)
    {
		return TimeSpan.FromMinutes(minutes).TotalDays;
    }

    public static double Hours2Days(double hours)
    {
		return TimeSpan.FromHours(hours).TotalDays;
    }
#endregion

#region To hours
    public static double Milliseconds2Hours(double milliseconds)
    {
		return TimeSpan.FromMilliseconds(milliseconds).TotalHours;
    }

    public static double Seconds2Hours(double seconds)
    {
		return TimeSpan.FromSeconds(seconds).TotalHours;
    }

    public static double Minutes2Hours(double minutes)
    {
		return TimeSpan.FromMinutes(minutes).TotalHours;
    }

    public static double Days2Hours(double days)
    {
		return TimeSpan.FromHours(days).TotalHours;
    }
#endregion

#region To minutes
    public static double Milliseconds2Minutes(double milliseconds)
    {
		return TimeSpan.FromMilliseconds(milliseconds).TotalMinutes;
    }

    public static double Seconds2Minutes(double seconds)
    {
		return TimeSpan.FromSeconds(seconds).TotalMinutes;
    }

    public static double Hours2Minutes(double hours)
    {
		return TimeSpan.FromHours(hours).TotalMinutes;
    }

    public static double Days2Minutes(double days)
    {
		return TimeSpan.FromDays(days).TotalMinutes;
    }
#endregion

#region To seconds
    public static double Milliseconds2Seconds(double milliseconds)
    {
		return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
    }

    public static double Minutes2Seconds(double minutes)
    {
		return TimeSpan.FromMinutes(minutes).TotalSeconds;
    }

    public static double Hours2Seconds(double hours)
    {
		return TimeSpan.FromHours(hours).TotalSeconds;
    }

    public static double Days2Seconds(double days)
    {
		return TimeSpan.FromDays(days).TotalSeconds;
    }
#endregion

#region To milliseconds
    public static double Seconds2Milliseconds(double seconds)
    {
		return TimeSpan.FromSeconds(seconds).TotalMilliseconds;
    }

    public static double Minutes2Milliseconds(double minutes)
    {
		return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
    }

    public static double Hours2Milliseconds(double hours)
    {
		return TimeSpan.FromHours(hours).TotalMilliseconds;
    }

    public static double Days2Milliseconds(double days)
    {
		return TimeSpan.FromDays(days).TotalMilliseconds;
    }
#endregion
	
	public static void Pause()
	{
		pause = true;
	}
	
	public static void Resume()
	{
		pause = false;
	}
	
	public static void StartGame()
	{
		gameTime = Time.time;
		playTime = 0.0f;
	}
	
	public static void EndGame()
	{
		//nothing!! yet!!
	}
	
	public static void StartStage()
	{
		m_IsPlayingStage = true;
		
		stageTime = 0.0f;
	}
	
	public static void EndStage()
	{
		m_IsPlayingStage = false;
	}
	
	public static void StartRound()
	{
		m_IsPlayingRound = true;
			
		roundTime = 0.0f;
	}
	
	public static void EndRound()
	{
		m_IsPlayingRound = false;
	}
}
