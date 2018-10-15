using UnityEngine;

public abstract class CSVLoadBase<T> 
{

	public int Info_Row;
	public int Info_Col;
	protected bool isregister;
	
	protected T m_data;
	
	public CSVLoadBase()
	{
		isregister = false;
		LoadLocal();
	}
	
	protected abstract void LoadLocal();
	
	protected virtual CSVParser LoadFileRowCol(string _strFileName, string _strData)
	{
		bool isOk;
		CSVParser tp = new CSVParser(_strFileName);
		tp.readTextString(_strData);
		isOk = tp.getFileRowCol(ref Info_Row, ref Info_Col);
		
		if (isOk == false)
		{
			//ZpLog.Normal(ZpLog.E_Category.None, _strFileName + " register false");
			return null;
		}
		
		string[] arrayInfo_Col = new string[Info_Col];
		
		for (int i = 0 ; i < Info_Col ; ++i)
		{
			arrayInfo_Col[i] = tp.getString();
		}
		
		return tp;
	}
	
	protected virtual bool RegisterData(string _strFileName, string _strData)
	{
		return LoadFileRowCol(_strFileName, _strData) != null;
	}
}
