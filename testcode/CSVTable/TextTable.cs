using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TextTable : CSVLoadBase<Dictionary<string, string>>
{
	public string GetString(string key)
	{
		string str_info = "";
		if( !m_data.TryGetValue(key, out str_info) )
		{
			str_info = string.Format("No Message KEY : {0}", key);
		}

		return str_info;
	}

	protected override void LoadLocal()
	{
		if (isregister)
			return;

		TextAsset ta = Resources.Load( "Table/text_table" ) as TextAsset;
		RegisterData("text_table", ta.text);		
	}

	protected override bool RegisterData(string _strFileName, string _strData)
	{
		CSVParser tp = LoadFileRowCol(_strFileName, _strData);
		if (tp == null)
			return false;

		m_data = new Dictionary<string, string>();

		for (int i = 1; i < Info_Row; ++i)
		{
			string key = tp.getString ();
			string svalue = tp.getString ();
			
			m_data.Add(key, svalue);
		}

		isregister = true;
		return true;
	}
}