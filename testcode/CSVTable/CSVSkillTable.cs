using UnityEngine;
using System;
using System.Collections.Generic;

public class CSVSkillTable : CSVLoadBase<Dictionary<int, SkillStruct>>
{
	public int GetDicCount()
	{
		return m_data.Count;
	}

	public List<SkillStruct> GetSkillList(bool isAll)
	{
		List<SkillStruct> tempList = new List<SkillStruct> ();

		foreach(KeyValuePair<int, SkillStruct> kvp in m_data )
		{
			if(isAll)
			{
				if (kvp.Value.eTarget == ESkillTarget.ENEMY_ALL) 
				{
					tempList.Add (kvp.Value);
				}
			}
			else
			{
				if (kvp.Value.eTarget == ESkillTarget.ENEMY_RANDOM) 
				{
					tempList.Add (kvp.Value);
				}
			}
		}

		return tempList;
	}

	public bool TryGetValue(int _index, out SkillStruct info)
	{
		return m_data.TryGetValue(_index, out info);
	}

	protected override void LoadLocal()
	{
		if (isregister)
			return;

		TextAsset ta = Resources.Load( "Table/skill_table" ) as TextAsset;
		RegisterData("skill_table", ta.text);		
	}

	protected override bool RegisterData(string _strFileName, string _strData)
	{
		CSVParser tp = LoadFileRowCol(_strFileName, _strData);
		if (tp == null)
			return false;

		m_data = new Dictionary<int, SkillStruct>();

		for (int i = 1; i < Info_Row; ++i)
		{
			SkillStruct data = new SkillStruct();

			data.nIndex				= tp.getInt();
			data.str_Name			= tp.getString();
			data.eSkilltype			= (ESkillType)tp.getInt();
			data.nChance			= tp.getInt();
			data.eTarget			= (ESkillTarget)tp.getInt();
			data.eSkillDamageType	= (ESkillFlowType)tp.getInt();
			data.nAbsoluteValue		= tp.getInt();
			data.nPercentValue		= tp.getInt();

			data.eActionFlow		= (ESkillFlowType)tp.getInt();
			data.eActionType		= (ESkillStateType)tp.getInt();
			data.eActionTarget		= (ESkillTarget)tp.getInt();
			data.nActionValue		= tp.getInt();

			data.eConditionFlow		= (ESkillFlowType)tp.getInt();
			data.eConditionType		= (ESkillStateType)tp.getInt();
			data.eConditionTarger	= (ESkillTarget)tp.getInt();
			data.nConditionValue	= tp.getInt();

			data.str_iconName		= tp.getString();
			data.str_EffectName		= tp.getString();

			data.nSkillUsedNpcIndex	= tp.getInt();
			data.str_explanation	= tp.getString();

			data.isSkillOn 			= true;

			m_data.Add(data.nIndex, data);
		}

		isregister = true;
		return true;
	}
}
