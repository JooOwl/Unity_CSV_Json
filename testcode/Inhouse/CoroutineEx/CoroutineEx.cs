#define DEF_DEL_EACH_FUNCTION

using UnityEngine;
using System.Collections;

using System.Reflection;

using System.Linq;

public static class CoroutineEx
{
#if true

    public static Ev.CoroutineUnit StartCoroutineEx(this MonoBehaviour behaviour, string func, System.Action finish = null, System.Action cancel = null)
    {
		if( null == behaviour )
		{
			return null;
		}

		//Debug.Log("StartCoroutineEx " + behaviour.gameObject.name + " + " + func  );
		
        return StartCoroutineEx(behaviour, func, null, finish, cancel);
    }

    public static Ev.CoroutineUnit StartCoroutineEx(this MonoBehaviour behaviour, string func, object[] param, System.Action finish = null, System.Action cancel = null)
    {
		if( null == behaviour )
		{
			return null;
		}

		//Debug.Log("StartCoroutineEx " + behaviour.gameObject.name + " + " + func );
		
        if (string.IsNullOrEmpty(func))
        {
			NGUIDebug.Log("string.IsNullOrEmpty(func)");
            Debug.LogError("string.IsNullOrEmpty(func)");
            return null;
        }

        MethodInfo m = behaviour.GetType().GetMethod(func, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        if (null == m)
        {
			NGUIDebug.Log("null == m " + func);
            Debug.LogError("null == m " + func);
            return null;
        }

        if (m.ReturnType == typeof(IEnumerator))
        {
            IEnumerator fiber = m.Invoke(behaviour, param) as IEnumerator;
			
			if( null == fiber )
			{
				Debug.LogError("null == fiber " + behaviour.gameObject.name + " + " + func );
				return null;
			}

            Ev.CoroutineUnit corutineUnit = behaviour.gameObject.AddComponent<Ev.CoroutineUnit>() as Ev.CoroutineUnit;
			
			if( null == corutineUnit )
			{
				Debug.LogError("null == corutineUnit "  + behaviour.gameObject.name + " + " + func );
				return null;
			}
			
            corutineUnit.Initialize(fiber, func, finish, cancel);

            return corutineUnit;
        }
		
		//Debug.Log("Not Found Coroutine "  + behaviour.gameObject.name + " + " + func );

        return null;

    }

    public static void StopAllCoroutineEx(this MonoBehaviour behaviour)
    {
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("StopAllCoroutineEx " + behaviour.gameObject.name );
		
#if DEF_DEL_EACH_FUNCTION
		
		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();
		
		if( null == coroutineUnits )
		{
			Debug.LogError("null == coroutineUnits "  + behaviour.gameObject.name );
			
			return;
		}
		
		//Debug.Log("Foreach Start "  + behaviour.gameObject.name );
		
		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			if( null == coroutineUnit )
			{
				Debug.LogError("null == coroutineUnit "  + behaviour.gameObject.name );
				
				continue;
			}
			
			//Debug.Log("Foreach Run "  + behaviour.gameObject.name );
			
			coroutineUnit.Cancel();
		}
		
		//Debug.Log("Foreach End "  + behaviour.gameObject.name );
		
#else
        behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Each(x => x.Cancel());
#endif
		
		//Debug.Log("StopAllCoroutineEx End " + behaviour.gameObject.name );
    }

    public static void StopCoroutineEx(this MonoBehaviour behaviour, string func)
    {
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("StopCoroutineEx "  + behaviour.gameObject.name + " + " + func );
		
#if DEF_DEL_EACH_FUNCTION
		
		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();
		
		if( null == coroutineUnits )
		{
			return;
		}
		
		//Debug.Log("Foreach In "  + behaviour.gameObject.name + " + " + func );
		
		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			if( string.IsNullOrEmpty(coroutineUnit.functionName) )
			{
				Debug.Log("string.IsNullOrEmpty(coroutineUnit.functionName) "  + behaviour.gameObject.name + " + " + func );
				continue;
			}
			
			if( coroutineUnit.functionName == func )
			{
				//Debug.Log("Foreach Run "  + behaviour.gameObject.name + " + " + func );
				
				coroutineUnit.Cancel();
			}
		}
		
		//Debug.Log("Foreach End "  + behaviour.gameObject.name + " + " + func );
		
#else
        behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Where(x => x.functionName == func)
            .Each(x => x.Cancel());
#endif
		
		//Debug.Log("StopCoroutineEx End "  + behaviour.gameObject.name + " + " + func  );
    }

    public static bool IsCoroutineEx(this MonoBehaviour behaviour, string func)
    {		
		if( null == behaviour )
		{
			return false;
		}

		//Debug.Log("IsCoroutineEx " + behaviour.gameObject.name + " + " + func);
		
        bool result = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Where(x => x.functionName == func)
            .Count() > 0;
		
		//Debug.Log("IsCoroutineEx End " + behaviour.gameObject.name + " + " + func );
		
		return result;
    }

    public static bool IsCoroutineEx(this MonoBehaviour behaviour)
	{	
		if( null == behaviour )
		{
			return false;
		}

		//Debug.Log("IsCoroutineEx " + behaviour.gameObject.name );
		
        bool result = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Count() > 0;
		
		//Debug.Log("IsCoroutineEx End " + behaviour.gameObject.name );
		
		return result;
    }
	
	public static int GetCoroutineCount(this MonoBehaviour behaviour)
	{
		if( null == behaviour )
		{
			return -1;
		}

		//Debug.Log("GetCoroutineCount " + behaviour.gameObject.name );
		
		int count = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Count();
		
		//Debug.Log("GetCoroutineCount End " + behaviour.gameObject.name );
		
		return count;
	}
	
	public static bool IsCoroutineCount(this MonoBehaviour behaviour, int count )
	{
		if( null == behaviour )
		{
			return false;
		}

		//Debug.Log("IsCoroutineCount " + behaviour.gameObject.name );
		
		bool result = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Count() == count;
		
		//Debug.Log("GetCoroutineCount End " + behaviour.gameObject.name + " + "  + result );
		
		return result;
	}


	//ksh 2016-03-08
	public static void PauseAllCoroutineEx( this MonoBehaviour behaviour )
	{
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("StopAllCoroutineEx " + behaviour.gameObject.name );

#if DEF_DEL_EACH_FUNCTION

		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();

		if( null == coroutineUnits )
		{
			Debug.Log("null == coroutineUnits "  + behaviour.gameObject.name );

			return;
		}

		//Debug.Log("Foreach Start "  + behaviour.gameObject.name );

		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			if( null == coroutineUnit )
			{
				Debug.Log("null == coroutineUnit "  + behaviour.gameObject.name );

				continue;
			}

			//Debug.Log("Foreach Run "  + behaviour.gameObject.name );

			coroutineUnit.Pause();
		}

		//Debug.Log("Foreach End "  + behaviour.gameObject.name );

#else
		behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
			.Each( x => x.Pause() );
#endif

		//Debug.Log("StopAllCoroutineEx End " + behaviour.gameObject.name );
	}
	
	public static void PauseCoroutineEx( this MonoBehaviour behaviour, string func )
	{
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("PauseCoroutineEx " + behaviour.gameObject.name + " + " + func );
		
#if DEF_DEL_EACH_FUNCTION
		
		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();
		
		if( null == coroutineUnits )
		{
			Debug.LogError("null == coroutineUnits "  + behaviour.gameObject.name + " + " + func );
			return;
		}
		
		//Debug.Log("Foeach Start "  + behaviour.gameObject.name + " + " + func );
		
		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			if( coroutineUnit.functionName == func )
			{
				//Debug.Log("Foeach Run "  + behaviour.gameObject.name + " + " + func );
				
				coroutineUnit.Pause();
			}
		}
		
		//Debug.Log("Foeach End "  + behaviour.gameObject.name + " + " + func );
		
#else
       
		behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Where(x => x.functionName == func)
            .Each( x => x.Pause() );
#endif
		
		//Debug.Log("PauseCoroutineEx End " + behaviour.gameObject.name );
	}

	//ksh 2016-03-08
	public static void ResumeAllCoroutineEx( this MonoBehaviour behaviour )
	{
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("ResumeCoroutineEx " + behaviour.gameObject.name );

#if DEF_DEL_EACH_FUNCTION

		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();

		if( null == coroutineUnits )
		{
			Debug.Log("null == coroutineUnits "  + behaviour.gameObject.name );

			return;
		}

		//Debug.Log("Foreach Start "  + behaviour.gameObject.name + " + " + func );

		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			coroutineUnit.Resume();
		}

		//Debug.Log("Foreach End "  + behaviour.gameObject.name + " + " + func );

#else
		behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
			.Each( x => x.Resume());
#endif

		Debug.Log("ResumeCoroutineEx End " + behaviour.gameObject.name );
	}
	
	public static void ResumeCoroutineEx( this MonoBehaviour behaviour, string func )
	{
		if( null == behaviour )
		{
			return;
		}

		//Debug.Log("ResumeCoroutineEx " + behaviour.gameObject.name );
		
#if DEF_DEL_EACH_FUNCTION
		
		Ev.CoroutineUnit[] coroutineUnits = behaviour.gameObject.GetComponents<Ev.CoroutineUnit>();
		
		if( null == coroutineUnits )
		{
			Debug.Log("null == coroutineUnits "  + behaviour.gameObject.name + " + " + func );
			
			return;
		}
		
		//Debug.Log("Foreach Start "  + behaviour.gameObject.name + " + " + func );
		
		foreach( Ev.CoroutineUnit coroutineUnit in coroutineUnits )
		{
			if( coroutineUnit.functionName == func )
			{
				//Debug.Log("Foreach Run "  + behaviour.gameObject.name + " + " + func );
				
				coroutineUnit.Resume();
			}
		}
		
		//Debug.Log("Foreach End "  + behaviour.gameObject.name + " + " + func );
		
#else
		behaviour.gameObject.GetComponents<Ev.CoroutineUnit>()
            .Where(x => x.functionName == func)
            .Each( x => x.Resume());
#endif
		
		//Debug.Log("ResumeCoroutineEx End " + behaviour.gameObject.name );
	}
	
#endif

#if false
    public static Ev.CoroutineUnit StartCoroutineEx(this GameObject obj, IEnumerator fiber, System.Action f = null, System.Action c = null)
    {
        if (fiber == null)
        {
            return null;
        }

        Ev.CoroutineUnit corutineUnit = obj.AddComponent<Ev.CoroutineUnit>() as Ev.CoroutineUnit;
        corutineUnit.Initialize(fiber, f , c );

        return corutineUnit;
    }

    public static Ev.CoroutineUnit StartCoroutineEx(this MonoBehaviour behaviour, IEnumerator fiber, System.Action f = null, System.Action c = null)
    {
        return StartCoroutineEx(behaviour.gameObject, fiber, f, c);
        /*
        if (fiber == null)
        {
            return null;
        }

        Ev.CoroutineUnit corutineUnit = behaviour.gameObject.AddComponent<Ev.CoroutineUnit>() as Ev.CoroutineUnit;
        corutineUnit.Initialize(fiber, f, c);

        return corutineUnit;
        */
    }

    public static Ev.CoroutineUnit StartCoroutineEx(this GameObject obj, string func, IEnumerator fiber, System.Action f = null, System.Action c = null)
    {
        var m = obj.GetType().GetMethod(func, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        
        if (m.ReturnType == typeof(IEnumerator))
        {
            return obj.StartCoroutineEx((IEnumerator)m.Invoke(obj, null));
        }

        return null;

    }

    public static Ev.CoroutineUnit StartCoroutineEx(this MonoBehaviour behaviour, string func, IEnumerator fiber, System.Action f = null, System.Action c = null)
    {
        return StartCoroutineEx(behaviour.gameObject, func, fiber, f, c);
    }
#endif
}
