using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum VIEWROTATION
{
	NONE,
	PORTRAIT,
	LANDSCAPE,
}

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class UIViewSizeRoot : MonoBehaviour
{	
	int PhoneRealWidthSize = Screen.width;
	int PhoneRealHeightSize = Screen.height;

	[SerializeField]
	public int m_GameScreenWidth = 540;
	[SerializeField]
	public int m_GameScreenHeight = 960;

	public VIEWROTATION viewrotation = VIEWROTATION.PORTRAIT;
	Transform mTrans;
	float fXrate = 1f;

#if UNITY_EDITOR
	public bool bReSetViewRate = true;
#endif

	public int activeHeight
	{
		get
		{
			int height = Mathf.Max(2, Screen.height);

			if (height == m_GameScreenHeight)
				return height;
			else
				return m_GameScreenHeight;
		}
	}

	void Awake ()
	{
		if( ViewManager.myInstance != null )
		{
			m_GameScreenWidth = ViewManager.myInstance.m_GameScreenWidth;
			m_GameScreenHeight = ViewManager.myInstance.m_GameScreenHeight;

			viewrotation = ViewManager.myInstance.ViewRateMode;
		}

		PhoneRealWidthSize = Screen.width;
		PhoneRealHeightSize = Screen.height;
	}

	void Start()	
	{
		if( ViewManager.myInstance != null )
		{
			m_GameScreenWidth = ViewManager.myInstance.m_GameScreenWidth;
			m_GameScreenHeight = ViewManager.myInstance.m_GameScreenHeight;

			viewrotation = ViewManager.myInstance.ViewRateMode;
		}

		GetXRate();
		SetViewRate();
	}

#if UNITY_EDITOR
	void Update ()
	{
		if( bReSetViewRate )
		{
			bReSetViewRate = false;

			PhoneRealWidthSize = Screen.width;
			PhoneRealHeightSize = Screen.height;

			GetXRate();
			SetViewRate();
		}
	}
#endif

	// X 비율을 구하는 함수.
	void GetXRate()
	{
#if UNITY_EDITOR
		PhoneRealWidthSize = Screen.width;
		PhoneRealHeightSize = Screen.height;
#endif

		mTrans = this.transform;
		int nLcm = GetLCM( m_GameScreenWidth, m_GameScreenHeight );
		
		float GameWidthRate = m_GameScreenWidth / nLcm;
		float GameHeightRate = m_GameScreenHeight / nLcm;

		fXrate = RateScale( PhoneRealWidthSize, PhoneRealHeightSize, GameWidthRate, GameHeightRate);
	}

	// 실제 구한 값을 이용하여 스케일링 하는 방법.
	void SetViewRate()
	{
		if (mTrans != null)
		{
			float calcActiveHeight = activeHeight;

			if (calcActiveHeight > 0f )
			{
				float size = 2f / calcActiveHeight;

				if( viewrotation == VIEWROTATION.PORTRAIT)
				{
					//size = fXrate * size;
					mTrans.localScale = new Vector3(size, size, size);
				}
				else if(viewrotation == VIEWROTATION.LANDSCAPE)
				{
					size = fXrate * size;
					mTrans.localScale = new Vector3(size, size, size);
				}
			}
		}
	}

	// 최대 공략수와 이것 저것을 이용하여 가로의 비율의 길이를 구하는 방법.
	float RateScale(int sw, int sh, float GameWidthRate, float GameHeightRate)
	{		
		if ( viewrotation == VIEWROTATION.NONE ) 
		{
			if ( sw >= sh ) 
			{
				viewrotation = VIEWROTATION.LANDSCAPE;
			}
			else 
			{
				viewrotation = VIEWROTATION.PORTRAIT;
			}
		}
		
		int nLcm = GetLCM( sw, sh );
		
		float fW_Rate = sw / nLcm;
		float fH_Rate = sh / nLcm;
		
		float fRateScale = (fW_Rate / GameWidthRate * GameHeightRate / fH_Rate);
		float fWidht = (float)m_GameScreenWidth * fRateScale;
		
		float fGameScreen = (float)m_GameScreenWidth;
		
		if( fGameScreen < fWidht)
		{
			float temp;
			temp = fGameScreen;
			fGameScreen = fWidht;
			fWidht = temp;
		}
		
		float rateX = fWidht / fGameScreen;
		return rateX;
	}

	// 최대 공략수를 구하기 위한 함수.
	int GetLCM(int u, int v)
	{
	    int t;
	    while ( true )
	    {
	        t = u % v;
	        u = v;
	        v = t;
			
			if( v == 0 || u == 0 )
	        {
	            break;
	        }
	    }
	
	    return u;
	}
	
	[ContextMenu("ReSetView")]
	public virtual void Reposition ()
	{
		NGUIDebug.Log( string.Format("{0}, {1}", PhoneRealWidthSize, PhoneRealHeightSize));
		GetXRate();
		SetViewRate();
	}

	///////////////////////////////////////////////////////////////////////////////

	public void UpDateOn()
	{
		NGUIDebug.Log( string.Format("{0}, {1}", PhoneRealWidthSize, PhoneRealHeightSize));

		GetXRate();
		SetViewRate();
	}

	public void BtnSetView()
	{
		Reposition ();
	}
	///////////////////////////////////////////////////////////////////////////////
}
