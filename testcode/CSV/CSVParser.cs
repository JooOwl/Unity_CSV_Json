using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVParser : TextParser {

	protected bool m_bCommas;
	
	protected int m_MaxRow;
	protected int m_MaxCol;
	
	protected int m_CrtRow;
	protected int m_CrtCol;
	
	//protected char m_Seperator = ',';
	protected char m_Seperator = '\t';
	
	public int CrtRow
	{
		get
		{
			return m_CrtCol;
		}
	}
	
	public const string getTokError = "ZpCSVParser::getTok() - Parser Exception";
	
	public CSVParser()
	{
		m_bCommas = false;
		m_CrtRow = m_CrtCol = m_MaxRow = m_MaxCol = 0;
	}
	
	public CSVParser(string filename)
	{
		m_Filename = filename;
		
		m_bCommas = false;
		m_CrtRow = m_CrtCol = m_MaxRow = m_MaxCol = 0;
	}
	
	public CSVParser(char InSeperator)
	{
		m_bCommas = false;
		m_Seperator = InSeperator;
		m_CrtRow = m_CrtCol = m_MaxRow = m_MaxCol = 0;
	}
	
	public void setCommas(bool in_Commas)
	{
		m_bCommas = in_Commas;
	}
	
	public bool getFileRowCol(ref int out_Row, ref int out_Col)
	{
		int tpCol = 0;
		
		out_Row = 1;
		out_Col = 0;
		m_CurrentIndex = 0;
		
		setDelimiter(m_Seperator + "\n", TextParser.enDelimiterType.DelimiterType_Symbol);
		setDelimiter("\r", TextParser.enDelimiterType.DelimiterType_Ignore);
		
		try
		{
			while (m_Buffer[m_CurrentIndex] != 0)
			{
				m_pCur = m_Buffer[m_CurrentIndex];
				
				if (m_pCur == m_Seperator)
				{
					tpCol++;
				}
				else if (m_pCur == '\n')
				{
					tpCol++;
					
					if (tpCol <= 1)
					{
						////ZpLog.Error(//ZpLog.E_Category.None, "CSV에 1이하의 열이 있지만 이 행 전체를 무시합니다. 파일명 : " + m_Filename + "행 : " + out_Row + " 열 : " + tpCol);
						
						m_CurrentIndex++;
						tpCol = 0;
						
						if (m_CurrentIndex >= m_Buffer.Length)
						{
							break;
						}
						else
						{
							continue;
						}
					}
					
					if (out_Col == 0)
					{
						out_Col = tpCol;
					}
					else
					{
						if (tpCol != out_Col)
						{
							////ZpLog.Error(//ZpLog.E_Category.None, "CSV에 다른 열이 있습니다. 확인해 주세요. 파일명 : " + m_Filename + "행 : " + out_Row + " 열 : " + tpCol + " - " + out_Col);
							UnityEngine.Debug.Break();
							return false;
						}
					}
					
					out_Row++;
					
					tpCol = 0;
				}
				
				m_CurrentIndex++;
				if (m_CurrentIndex >= m_Buffer.Length)
				{
					tpCol++;
					if (tpCol > 1)
					{
						if (tpCol != out_Col)
						{
							////ZpLog.Error(//ZpLog.E_Category.None, "CSV에 다른 열이 있습니다. 확인해 주세요. 파일명 : " + m_Filename + "행 : " + out_Row + " 열 : " + tpCol);
							UnityEngine.Debug.Break();
							return false;
						}
						else
						{
							break;
						}
					}
					else
					{
						out_Row--;
					}
					break;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError(getTokError + " : " + e.ToString() + " m_CurrentIndex = " + m_CurrentIndex);
			UnityEngine.Debug.Break();
		}
		
		m_MaxRow = out_Row;
		m_MaxCol = out_Col;
		m_CurrentIndex = 0;
		
		return true;
	}
	
	//////////////////////////////////////////////////////////////////////////
	// GetData
	public string getString()
	{
		string str = getTok(0);
		getTok(0);
		return str;
	}
	
	public int getInt()
	{
		string str = getString();
		return int.Parse(str);
	}
	
	public float getFloat()
	{
		string str = getString();
		return float.Parse(str);
	}
	
	public override string getTok(int in_CursorIndex)
	{
		if (m_MaxCol == 0 || m_MaxRow == 0)
		{
			//ZpLog.Error(//ZpLog.E_Category.None, "before call getFileRowCol()");
			UnityEngine.Debug.Break();
			return getTokError;
		}
		
		if (in_CursorIndex != 0)
		{
			m_CurrentIndex = in_CursorIndex;
			m_CrtRow = m_CrtCol = 0;
		}
		getData = null;
		
		if (m_CurrentIndex >= m_Buffer.Length)
		{
			if (m_CrtRow < m_MaxRow && m_CrtCol < m_MaxCol)
			{
				getData += "0";
			}
			else
			{
				//ZpLog.Error(//ZpLog.E_Category.None, "CSV데이터가 맞지 않습니다. 확인해 주세요. 파일명 : " + m_Filename + "현재 행 : " + m_CrtRow + " 현재 열 : " + m_CrtCol + "최종 행 : " + m_MaxRow + " 최종 열 : " + m_MaxCol);
				getData += getTokError;
			}
			return getData;
		}
		
		try
		{
			while (m_Buffer[m_CurrentIndex] != 0)
			{
				if (m_pCur == '\n')
				{
					m_CrtCol = 0;
				}
				m_pCur = m_Buffer[m_CurrentIndex];
				
				if (findFromString(m_pCur, m_Delimiter[(int)enDelimiterType.DelimiterType_Symbol]))
				{
					if (getData != null)
					{
						return getData;
					}
					else
					{
						if (!m_bCommas)
						{
							getData += m_pCur;
							m_bCommas = true;
							m_CurrentIndex++;
						}
						else
						{
							getData += "0";
							m_bCommas = false;
						}
						
						if (m_pCur == m_Seperator)
						{
							m_CrtCol++;
						}
						else if (m_pCur == '\n')
						{
							m_CrtCol++;
							m_CrtRow++;
						}
						
						return getData;
					}
				}
				if (findFromString(m_pCur, m_Delimiter[(int)enDelimiterType.DelimiterType_Ignore]))
				{
					// csv
				}
				else
				{
					if (m_bCommas == true)
					{
						m_bCommas = false;
					}
					
					getData += m_pCur;
				}
				m_CurrentIndex++;
				
				if (m_CurrentIndex >= m_Buffer.Length)
				{
					break;
				}
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("ZpCSVParser::getTok() - Parser Exception : " + e.ToString() + " m_CurrentIndex = " + m_CurrentIndex);
			Debug.Log("CSV데이터가 맞지 않습니다. 확인해 주세용. 파일명 : " + m_Filename + " 최종 행 : " + m_CrtRow + " 최종 열 : " + m_CrtCol);
			UnityEngine.Debug.Break();
		}
		
		return getData;
	}
}
