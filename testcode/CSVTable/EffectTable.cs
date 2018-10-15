using UnityEngine;
using System;
using System.Collections.Generic;

public class EffectTable : CSVLoadBase<Dictionary<int, EffectDataStruct>>
{
	public bool GetEffectInfo(int _key, out EffectDataStruct _info)
	{
		return m_data.TryGetValue(_key, out _info);
	}

	protected override void LoadLocal()
	{
		if (isregister)
			return;

		TextAsset ta = Resources.Load( "Table/effect_table" ) as TextAsset;
		RegisterData("effect_table", ta.text);		
	}

	protected override bool RegisterData(string _strFileName, string _strData)
	{
		CSVParser tp = LoadFileRowCol(_strFileName, _strData);
		if (tp == null)
			return false;

		m_data = new Dictionary<int, EffectDataStruct>();

		for (int i = 1; i < Info_Row; ++i)
		{
			EffectDataStruct data = new EffectDataStruct();

			data.index = tp.getInt();
			data.eLoof = (ePlayLoof)tp.getInt();
			data.nPlayTime = tp.getInt();
			data.ePos = (eEffectPos)tp.getInt();
			data.sResourceName = tp.getString();
			data.sDec = tp.getString();

			m_data.Add( data.index, data);
		}

		isregister = true;
		return true;
	}
}
