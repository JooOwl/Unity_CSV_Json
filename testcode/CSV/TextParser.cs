using System.Collections;
using System.IO;

public class TextParser {

	//////////////////////////////////////////////////////////////////////////
	// getTok()
	public enum enDelimiterType
	{
		DelimiterType_Ignore = 0,
		DelimiterType_Symbol,
		DelimiterType_Size,
	};
	
	public const string defDefaultDelimiter_Ignore = " \t\r\n[]=,{}\"";
	public const string defDefaultDelimiter_Symbol = "()";
	
	public TextParser()
	{
		m_Delimiter = new string[(int)enDelimiterType.DelimiterType_Size];
		m_Delimiter[(int)enDelimiterType.DelimiterType_Ignore] = defDefaultDelimiter_Ignore;
		m_Delimiter[(int)enDelimiterType.DelimiterType_Symbol] = defDefaultDelimiter_Symbol;
		
		m_CurrentIndex = 0;
	}
	
	protected string m_Filename;
	
	//////////////////////////////////////////////////////////////////////////
	// Delimiter
	protected string[] m_Delimiter;
	public void setDelimiter(string in_Delimiter, enDelimiterType in_DelimiterType)
	{
		if (in_DelimiterType == enDelimiterType.DelimiterType_Size)
		{
			return;
		}
		m_Delimiter[(int)in_DelimiterType] = in_Delimiter;
	}
	public bool compDelimiter_Ignore(char in_comp)
	{
		return findFromString(in_comp, m_Delimiter[(int)enDelimiterType.DelimiterType_Ignore]);
	}
	public bool compDelimiter_Symbol(char in_comp)
	{
		return findFromString(in_comp, m_Delimiter[(int)enDelimiterType.DelimiterType_Symbol]);
	}
	
	//////////////////////////////////////////////////////////////////////////
	// Cursor
	protected char m_pCur;
	protected int m_CurrentIndex;
	public void setCursorIndex(int in_CurrentIndex)
	{
		m_CurrentIndex = in_CurrentIndex;
	}
	public int getCursorIndex()
	{
		return m_CurrentIndex;
	}
	
	//////////////////////////////////////////////////////////////////////////
	// Buffer
	protected string m_Buffer;
	public void readTextString(string in_Buffer)
	{
		m_Buffer = in_Buffer;
	}
	
	//////////////////////////////////////////////////////////////////////////
	// Util
	public static bool findFromString(char in_Towolen, string in_String)
	{
		if (in_String == null || in_String.Length == 0)
		{
			return false;
		}
		
		foreach (char temp in in_String)
		{
			if (temp == in_Towolen)
			{
				return true;
			}
		}
		return false;
	}
	
	//////////////////////////////////////////////////////////////////////////
	// GetData
	protected string getData;
	public virtual string getTok(int in_CursorIndex)
	{
		if (in_CursorIndex != 0)
		{
			m_CurrentIndex = in_CursorIndex;
		}
		getData = null;
		
		while (m_Buffer.Length > m_CurrentIndex && m_Buffer[m_CurrentIndex] != 0)
		{
			m_pCur = m_Buffer[m_CurrentIndex];
			
			if (findFromString(m_pCur, m_Delimiter[(int)enDelimiterType.DelimiterType_Symbol]))
			{
				if (getData != null)
				{
					return getData;
				}
				else
				{
					getData += m_pCur;
					m_CurrentIndex++;
					return getData;
				}
			}
			if (findFromString(m_pCur, m_Delimiter[(int)enDelimiterType.DelimiterType_Ignore]))
			{
				if (getData != null)
				{
					return getData;
				}
			}
			else
			{
				getData += m_pCur;
			}
			m_CurrentIndex++;
			
			if (m_CurrentIndex > m_Buffer.Length)
			{
				break;
			}
		}
		return getData;
	}
}
