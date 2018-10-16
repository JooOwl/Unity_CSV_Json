using UnityEngine;
using System.Collections;

namespace Ev
{

    public abstract class CoroutineCondition
    {
		public bool pause = false;
        public abstract bool check { get; }

        //public abstract void Update(float frameCount, float deltaTime);
    }

    //WaitForFrameCount
    public class WaitForFrameCount : CoroutineCondition
    {
        private int m_FrameCount = 0;
        private int m_TotalFrameCount = 0;

        public WaitForFrameCount(int frameCount)
        {
            m_FrameCount = frameCount;
        }
        
        public override bool check 
        {
            get
            {
                m_TotalFrameCount += GameTime.frameCount;

                return m_TotalFrameCount >= m_FrameCount; 
            }
        }
    }

    //WaitForSeccond
    public class WaitForSeccond : CoroutineCondition
    {
        private float m_Second = 0;
        private float m_StartTime = 0;

        public WaitForSeccond(float sec)
        {
            m_StartTime = Time.time;
            m_Second = sec;
        }

        //public void Update(float frameCount, float deltaTime)
        //{
        //}

        public override bool check
        {
            get
            {
				if( GameTime.pause || pause )
				{
					m_StartTime += Time.deltaTime;
				}
				
                return Time.time - m_StartTime > m_Second;
            }
        }
    }
	
	//WaitForTickTime, This is ignore Time.timeScale = 0
    public class WaitForTickTime : CoroutineCondition
    {
        private float m_Second = 0;
		private float m_Current = 0;
        //private float m_StartTime = 0;
		
		private static float tick = 0.01f;

        public WaitForTickTime(float sec)
        {
            //m_StartTime = Time.time;
            m_Second = sec;
        }

        public override bool check
        {
            get
            {
				m_Current += tick;
				
				return m_Current >= m_Second;
			}
        }
    }
	
	public class WaitForAnimation : CoroutineCondition
    {
        public GameObject go = null;
        public string name = "";
		public AnimationState state = null;

		public WaitForAnimation()
        {
        }

        public WaitForAnimation(GameObject go, string name)
        {
			if( null == go )
			{
				Debug.LogError("null == go");
				return;
			}
			
			if( string.IsNullOrEmpty(name) )
			{
				Debug.LogError("null == go");
				return;
			}
			
            this.go = go;
            this.name = name;
			
			this.state = go.GetComponent<Animation>()[name];
			
			if( null == state )
			{
				Debug.LogError("null == state " + name + " " + go.name );
				return;
			}
        }

        public override bool check
        {
            get
            {
				if( null == state )
				{
					return true;
				}
				
				if( false == state.enabled )
				{
					return true;
				}

                if ( false == go.GetComponent<Animation>().IsPlaying(name) )
                {
                    return true;
                }
				
                return state.normalizedTime >= 1.0f - float.Epsilon;
            }
        }
	}

	/*
    //WaitForAnimation
    public class WaitForAnimation : CoroutineCondition
    {
        private GameObject _go;
        private string _name;
        private float _time;
        private float _weight;
        private int startFrame;

        public string name
        {
            get
            {
                return _name;
            }
        }

        public WaitForAnimation()
        {
        }

        public WaitForAnimation(GameObject go, string name)
            : this(go, name, 1f, -1)
        {
        }
        
        public WaitForAnimation(GameObject go, string name, float time)
            : this(go, name, time, -1)
        {
        }
        
        public WaitForAnimation(GameObject go, string name, float time, float weight)
        {
            startFrame = GameTime.frameCount;
            _go = go;
            _name = name;
            _time = Mathf.Clamp01(time);
            _weight = weight;
        }


        public override bool check
        {
            get
            {
				if ( GameTime.frameCount <= startFrame + 4)
                {
                    return false;
                }

                var anim = _go.animation[_name];
				
				if( null == anim )
				{
					return true;
				}

                bool ret = true;

                if (anim.enabled)
                {

                    if (_weight == -1)
                    {
                        ret = anim.normalizedTime >= _time;

                    }
                    else
                    {
                        if (_weight < 0.5)
                        {
                            ret = anim.weight <= Mathf.Clamp01(_weight + 0.001f);
                        }
                        ret = anim.weight >= Mathf.Clamp01(_weight - 0.001f);
                    }

                }
				
                if (!_go.animation.IsPlaying(_name))
                {
                    ret = true;
                }
				
                if (ret)
                {
                    if (anim.weight == 0 || anim.normalizedTime == 1)
                    {
                        anim.enabled = false;
                    }
                }
				
                return ret;
            }
        }
    }
	*/
		
	//WaitForAnimationFrame
	
	public class WaitForAnimationFrame : CoroutineCondition
    {
        public GameObject go = null;
        public string name = "";
        public float time = 1.0f;
		public AnimationState state = null;

		public WaitForAnimationFrame()
        {
        }

        public WaitForAnimationFrame(GameObject go, string name)
            : this(go, name, 1f)
        {
        }
        
        public WaitForAnimationFrame(GameObject go, string name, float time)
        {
			if( null == go )
			{
				Debug.LogError("null == go");
				return;
			}
			
			if( string.IsNullOrEmpty(name) )
			{
				Debug.LogError("null == go");
				return;
			}
			
			this.go = go;
            this.name = name;
            //this.time = Mathf.Clamp01(time);
			this.time = time;
			this.state = go.GetComponent<Animation>()[name];
			
			//Debug.Log(time / state.clip.length * state.clip.frameRate);
			
			if( null == state )
			{
				Debug.LogError("null == state " + name );
				return;
			}
			//
			//if( time == 0.0f )
			//{
			//	Debug.Log("Who?");
			//}
        }
        
        public override bool check
        {
            get
            {
				if( null == state )
				{
					return true;
				}
				
				if( false == state.enabled )
				{
					return true;
				}

                if ( false == go.GetComponent<Animation>().IsPlaying(name) )
                {
                    return true;
                }
				
				return state.time >= time / state.clip.frameRate;
		
				//return state.time >= (time - 1) / state.clip.frameRate;
						
				//return state.normalizedTime >= (time / (state.clip.length * state.clip.frameRate));
                //return state.time >= (time-1) / state.clip.frameRate;
            }	
        }
		/*
		public static Func<bool> WaitForAnimationStateToFrame(this CustomCoroutineScheduler scheduler, AnimationState state, int targetFrame)
	    {
	        return () => state.time >= (targetFrame - 1) / state.clip.frameRate;
	    }
	
	    public static Func<bool> WaitForAnimationStateToDisabled(this CustomCoroutineScheduler scheduler, AnimationState state)
	    {
	        return () => !state.enabled;
	    }
	
	    public static Func<bool> WaitForAnimationStateToLastFrame(this CustomCoroutineScheduler scheduler, AnimationState state)
	    {
	        return () => state.normalizedTime >= 1.0f - 1e-3f;
	    }
	   */
    }

    public class WaitForCondition : CoroutineCondition
    {
        private System.Func<bool> m_Condition = null;

        public WaitForCondition(System.Func<bool> con)
        {
            m_Condition = con;
        }

        public override bool check
        {
            get
            {
                return m_Condition();
            }
        }
    }
	
#region AssetBundleCondition

	//WaitForLoadAssetBundle
	public class WaitForLoadAssetBundle : Ev.CoroutineCondition
	{
		/*
		// 레고 에셋 번들이랑 패쳐에서 사용하던 부분.
	    private string m_AssetBundleName = "";
	    private string m_AssetName = "";
		private System.Type m_AssetType = typeof(GameObject);
		*/
	
	    public WaitForLoadAssetBundle(string assetBundleName, string assetName, System.Type assetType )
	    {
			//NGUIDebug.Log("4_1");
			
			if( string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogError("string.IsNullOrEmpty(assetBundleName)");
			}
			
			if( string.IsNullOrEmpty(assetName))
			{
				Debug.LogError("string.IsNullOrEmpty(assetName)");
			}
		
			/*
			// 레고 에셋 번들이랑 패쳐에서 사용하던 부분.
	        m_AssetBundleName = assetBundleName;
			m_AssetName = assetName;
			m_AssetType = assetType;
			*/
			
			//NGUIDebug.Log("4_2");
	    }
	    
	    public override bool check 
	    {
	        get
	        {
				return true;
				/*
				/// 레고에서 사용 하던 부분. 나중에는 없애야하는 부분. 우선은 가지고 있따가 예제가 완료 되면 바꾸자.
				if( Patcher.instance != null )
				{
					if( false == Patcher.instance.completeLoad )
					{
						return false;
					}
				}
				
				if( null == AssetBundleMgr.instance )
				{
					Debug.LogError("null == AssetBundleMgr.instance");
					
					return false;
				}
				
	            if( false == AssetBundleMgr.instance.IsAssetBundle( m_AssetBundleName ) )
				{
					AssetBundleMgr.instance.Load(m_AssetBundleName );
					
					//Debug.Log("AssetBundleMgr.instance.Load(m_AssetBundleName ) " + m_AssetBundleName );
				}

				if( AssetBundleMgr.instance.IsLoadFaieldAsset(m_AssetBundleName) )
				{
					return false;
				}
				
#if true
				
				return AssetBundleMgr.instance.GetAsset(m_AssetBundleName, m_AssetName, m_AssetType ) != null;
				//if( false == AssetBundleMgr.instance.IsAsset( m_AssetBundleName, m_AssetName ) )
				//{
				//	AssetBundleMgr.instance.LoadAsset(m_AssetBundleName, m_AssetName );
				//	return false;
				//}
				
				//return true;
#endif
				
#if false
				return AssetBundleMgr.instance.IsAsset(m_AssetBundleName, m_AssetName );
#endif
				*/
	        }
	    }
	}

#endregion

    public class Breaking
    {

    }

    public class CoroutineUnit : MonoBehaviour
    {

        private IEnumerator m_Fiber = null;

        private bool m_Finish = false;

        public bool finish
        {
            get
            {
                return m_Finish;
            }
        }

        private bool m_Cancel = false;

        public bool cancel
        {
            get
            {
                return m_Cancel;
            }
        }
		
		private bool m_Pause = false;

        public bool pause
        {
            get
            {
                return m_Pause;
            }
        }
		
		[SerializeField]
        private string m_FunctionName = "";

        public string functionName
        {
            get
            {
                return m_FunctionName;
            }
        }

        public System.Action m_FinishAction = null;
        public System.Action m_CancelAction = null;

        private CoroutineUnit m_Next = null;
        private CoroutineCondition m_Condition = null;
		
#region Test
		
		private int m_CancelCount = 0;
		
#endregion
		

        public void Initialize(IEnumerator fiber, string func, System.Action f, System.Action c)
        {
            m_Fiber = fiber;
            m_FunctionName = func;
            m_FinishAction = f;
            m_CancelAction = c;
        }

        // Update is called once per frame
        void Update()
        {
			//Debug.Log( "Coroutine " + m_FunctionName + " Start");
			
            if (m_Next != null)
            {
                bool isCancel = m_Next.cancel;

                if (false == isCancel)
                {
                    bool isFinishNext = m_Next.finish;

                    if (false == isFinishNext)
                    {
                        return;
                    }
					
					//Debug.Log( "Stop Coroutine " + m_FunctionName + " Start");
					
                    m_Next.Stop();
					
					//Debug.Log( "Stop Coroutine " + m_FunctionName + " End");
                }
                else
                {
					//Debug.Log( "Cancel Coroutine " + m_FunctionName + " End");
					
                    m_Next.Cancel();
                }

                m_Next = null;
            }

            if ( m_Cancel )
            {
                Cancel();
                return;
            }

            if (m_Condition != null && !m_Condition.check)
            {
                return;
            }
			
			if( null == m_Fiber )
			{
				Cancel();
				
				return;
			}

            if ( !m_Fiber.MoveNext())
            {
                Stop();

                return;
            }

            object res = m_Fiber.Current ?? "null";

            if( res is string )
            {
                if ( (string)res == "break")
                {
					//Debug.Log( "Breaking functuion " + m_FunctionName );
					
                    Cancel();
					
					//Debug.Log( "Breaking functuion End " + m_FunctionName );
                }
				else if( (string)res == "end")
				{
					Stop();
				}
            }
            else if (res is CoroutineUnit)
            {
                m_Next = (CoroutineUnit)res;
            }
            else if (res is CoroutineCondition)
            {
                m_Condition = (CoroutineCondition)res;
            }
            else if(res is Breaking)
            {
                Cancel();
            }
            else
            {
                UnityEngine.Debug.LogError("Unexpected coroutine yield type: " + res.GetType());
            }
        }

        public void Stop()
        {
            m_Finish = true;

            if (m_FinishAction != null)
            {
                m_FinishAction();
            }

            Destroy(this);
        }

        public void Cancel()
        {
			if( true == m_Cancel )
			{
				return;
			}
			
			m_CancelCount++;
			
			//Debug.Log( m_FunctionName + " " + m_CancelCount.ToString() );
			
			//m_FunctionName
			
            m_Cancel = true;

            if (m_CancelAction != null)
            {
                m_CancelAction();
            }
			
			//gameObject.SetActive(false);

            Destroy(this);
        }
		
		public void Pause()
		{
			m_Pause = true;
			
			if (m_Condition != null )
			{
				m_Condition.pause = true;
			}
		}
		
		public void Resume()
		{
			m_Pause = false;
			
			if (m_Condition != null )
			{
				m_Condition.pause = false;
			}
		}
    }
}
