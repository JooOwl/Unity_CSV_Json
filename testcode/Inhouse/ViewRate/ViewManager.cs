#define AUTO_VIEWROTATION

using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour {

	static public ViewManager myInstance = null;

	[SerializeField]
	public int m_GameScreenWidth = 540;
	[SerializeField]
	public int m_GameScreenHeight = 960;

	public VIEWROTATION ViewRateMode= VIEWROTATION.PORTRAIT;	

	void Awake()
	{
#if AUTO_VIEWROTATION
		if ( Screen.orientation == ScreenOrientation.Portrait ) 
		{
			ViewRateMode = VIEWROTATION.PORTRAIT;
		}
		else 
		{
			ViewRateMode = VIEWROTATION.LANDSCAPE;
		}
#endif
	
		if( myInstance == null )
		{
			myInstance = this;
		}

		if( this.transform.parent == null )
		{
			DontDestroyOnLoad(this);
		}
	}
}
