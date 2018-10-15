using UnityEngine;
using System;
using System.Collections.Generic;

public class NPCStateTable : CSVLoadBase<Dictionary<STATETYPE, NpcStateDateStruct>>
{
	public bool TryGetValue(STATETYPE type, out NpcStateDateStruct info)
	{
		return m_data.TryGetValue(type, out info);
	}
	
	protected override void LoadLocal()
	{
		if (isregister)
			return;
		
		TextAsset ta = Resources.Load( "Table/state_table" ) as TextAsset;
		RegisterData("state_table", ta.text);		
	}
	
	protected override bool RegisterData(string _strFileName, string _strData)
	{
		CSVParser tp = LoadFileRowCol(_strFileName, _strData);
		if (tp == null)
			return false;
		
		m_data = new Dictionary<STATETYPE, NpcStateDateStruct>();
		
		for (int i = 1; i < Info_Row; ++i)
		{
			NpcStateDateStruct data = new NpcStateDateStruct();
			
			string statetypename = tp.getString();
			
			foreach( STATETYPE state in Enum.GetValues(typeof(STATETYPE)))  
			{
				if( state.ToString().Equals(statetypename) )
				{
					data.stateType = state;
				}
			}
			
			data.usePoint = tp.getInt();
			data.upPoint = tp.getInt();
			data.maxPoint = tp.getInt();
			data.isUsed = (tp.getInt() == 1) ? true:false;
			
			if( m_data.ContainsKey(data.stateType) == true )
			{
				continue;
			}
			
			m_data.Add(data.stateType, data);
		}
		
		isregister = true;
		return true;
	}
}