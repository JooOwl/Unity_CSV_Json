using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

public class GamePlayManager :  MonoSingleton <GamePlayManager> 
{
	public Dictionary <int, NPCInfoData> dic_NPCInofData = new Dictionary<int, NPCInfoData>();
	public List<NPCInfoData> lst_NPCInfoDataTamp = new List<NPCInfoData> ();

	void Awake()	{	}

	// Use this for initialization
	void Start ()
	{
		if( mInstance == null )
		{
			mInstance = this;
		}

		if( this.transform.parent == null )
		{
			DontDestroyOnLoad(this);
		}

		Init ();
	}

	public override void Init() 
	{
		dic_NPCInofData.Clear ();
		lst_NPCInfoDataTamp.Clear ();

		LoadJsonFile ();
		LoadTempNPCData ();
	}

	public int NPCBackNumber()
	{
		int temp = 0;

		if( dic_NPCInofData.Count == 0)
		{
			return 0;
		}
		else
		{
			foreach (KeyValuePair<int, NPCInfoData> kvp in dic_NPCInofData) 
			{
				if( temp < kvp.Value.nNpcUniqueNumber )
				{
					temp = kvp.Value.nNpcUniqueNumber;
				}
			}
		}

		return ++temp;
	}

	public void NPCDataAdd(NPCInfoData _npcdata)
	{
		if( dic_NPCInofData == null )
		{
			SDebugLog.LogError ("if( dic_NPCData == null )");
			return;
		}

		if( dic_NPCInofData.ContainsKey( _npcdata.nNpcUniqueNumber ) )
		{
			dic_NPCInofData [_npcdata.nNpcUniqueNumber] = _npcdata;
		}
		else
		{
			dic_NPCInofData.Add (_npcdata.nNpcUniqueNumber, _npcdata);
		}
	}

	public bool NPCData_Panalty(int _npcNum)
	{
		if ( dic_NPCInofData.ContainsKey (_npcNum) ) 
		{
			NPCInfoData tNpcInfo = dic_NPCInofData [_npcNum];

			NpcStateDateStruct tATKInfo;
			if (CSVTableManager.myInstance.tNpcTableInfo.TryGetValue (STATETYPE.ATKCNT, out tATKInfo)) 
			{
				NpcStateDateStruct tAATKInfo;
				if (CSVTableManager.myInstance.tNpcTableInfo.TryGetValue (STATETYPE.AATKCNT, out tAATKInfo)) 
				{
					int nAtk = tNpcInfo.dicStateData[STATETYPE.ATKCNT];
					int nAAtk = tNpcInfo.dicStateData[STATETYPE.AATKCNT];

					if( nAtk > 0 &&
						nAAtk > 0 )
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	public int NPCData_StageChage(bool _up, int _npcNum, STATETYPE _type, ATTRIBUTE_TYPE _attrType = ATTRIBUTE_TYPE.NONE)
	{
		if (CSVTableManager.myInstance == null) 
		{
			SDebugLog.LogError ("if (CSVTableManager.myInstance == null) ");
			return -1;
		}

		NpcStateDateStruct tStateInfo;

		if (CSVTableManager.myInstance.tNpcTableInfo.TryGetValue (_type, out tStateInfo)) 
		{
			if( dic_NPCInofData.ContainsKey(_npcNum) == false )
			{
				SDebugLog.LogError ("if( dic_NPCInofData.ContainsKey(_npcNum) == false )");
				return -1;
			}

			NPCInfoData tNpcInfo = dic_NPCInofData [_npcNum];

			if (_type == STATETYPE.ATTRIBUTE) 
			{
				if (_attrType == ATTRIBUTE_TYPE.NONE) 
				{
					tNpcInfo.dicStateData [_type] = 0;

					if (tNpcInfo.eAtrbType != ATTRIBUTE_TYPE.NONE) 
					{
						tNpcInfo.nStatePastPoint -= tStateInfo.usePoint;
					}

					tNpcInfo.eAtrbType = _attrType;
				}
				else 
				{
					tNpcInfo.dicStateData [_type] = 1;

					if (tNpcInfo.eAtrbType == ATTRIBUTE_TYPE.NONE) 
					{
						tNpcInfo.nStatePastPoint += tStateInfo.usePoint;
					}

					tNpcInfo.eAtrbType = _attrType;
				}

				tNpcInfo.nStatePossiblePoint = tNpcInfo.nStateMaxPoint - tNpcInfo.nStatePastPoint;
			}
			else 
			{
				if( _up )
				{
					int tempPoint =	tNpcInfo.nStatePastPoint + tStateInfo.usePoint;

					if( tempPoint <= tNpcInfo.nStateMaxPoint )
					{
						tNpcInfo.dicStateData [_type] += 1;
						tNpcInfo.nStatePastPoint += tStateInfo.usePoint;
					}
				}
				else
				{
					int tempPoint =	tNpcInfo.nStatePastPoint - tStateInfo.usePoint;

					if( tempPoint >= 0 )
					{
						tNpcInfo.dicStateData [_type] -= 1;
						tNpcInfo.nStatePastPoint -= tStateInfo.usePoint;
					}
				}

				tNpcInfo.nStatePossiblePoint = tNpcInfo.nStateMaxPoint - tNpcInfo.nStatePastPoint;
			}

			dic_NPCInofData [_npcNum] = tNpcInfo;
		}

		return dic_NPCInofData [_npcNum].dicStateData [_type];
	}

	public void ChageMaxPoint(int _npcNum, GRADEPOINT eGradePoint)
	{
		int nAddPoint = 0;

		NPCInfoData tNpcInfo = dic_NPCInofData [_npcNum];

		if( dic_NPCInofData.ContainsKey(_npcNum) == false)
		{
			SDebugLog.LogError ("if( dic_NPCInofData.ContainsKey(_npcNum) == false)");
			return;
		}

		switch(eGradePoint)
		{
		case GRADEPOINT.TEN_UP:
			nAddPoint = 10;
			break;
		case GRADEPOINT.TEN_DOWN:
			nAddPoint = -10;
			break;
		case GRADEPOINT.HUNDERD_UP:
			nAddPoint = 100;
			break;
		case GRADEPOINT.HUNDERD_DOWN:
			nAddPoint = -100;
			break;
		}


		if (eGradePoint == GRADEPOINT.TEN_UP
		    || eGradePoint == GRADEPOINT.TEN_DOWN
		    || eGradePoint == GRADEPOINT.HUNDERD_UP
		    || eGradePoint == GRADEPOINT.HUNDERD_DOWN)
		{
			if( tNpcInfo.nStatePossiblePoint + nAddPoint < 0 )
			{
				return;
			}

			tNpcInfo.nStateMaxPoint += nAddPoint;
			tNpcInfo.nStatePossiblePoint += nAddPoint;
		}
		else 
		{
			tNpcInfo.eGradePoint = eGradePoint;

			int point = (int)tNpcInfo.eGradePoint * (int)GRADEPOINT.STANDPOINT;

			tNpcInfo.nStateMaxPoint = point;
			tNpcInfo.nStatePastPoint = 0;
			tNpcInfo.nStatePossiblePoint = tNpcInfo.nStateMaxPoint;
		}

		dic_NPCInofData [_npcNum] = tNpcInfo;
	}

	public void ResetStateNpc(int _npcNum)
	{
		if (dic_NPCInofData.ContainsKey (_npcNum) == false) 
		{
			SDebugLog.LogError ("if( dic_NPCInofData.ContainsKey(_npcNum) == false )");
			return;
		}

		NPCInfoData tNpcInfo = dic_NPCInofData [_npcNum];

		tNpcInfo.nStatePastPoint = 0;
		tNpcInfo.nStatePossiblePoint = tNpcInfo.nStateMaxPoint;

		foreach (STATETYPE value in Enum.GetValues(typeof(STATETYPE))) 
		{
			tNpcInfo.dicStateData [value] = 0;
		}

		dic_NPCInofData [_npcNum] = tNpcInfo;
	}

	/// <summary>
	/// Loads the json file.
	/// </summary>
	void LoadJsonFile()
	{
		string path = Application.temporaryCachePath;;
		string fileName = "NPC_DATA_JSON_NET.txt";

		string JsonFileName = string.Format("{0}/{1}", path, fileName );

		FileInfo _finfo = new FileInfo(JsonFileName);
		if (_finfo.Exists) 
		{
			dic_NPCInofData = LoadAndDeserialize<Dictionary<int, NPCInfoData>> (path, fileName);
		}
		else 
		{
			dic_NPCInofData.Clear ();
		}

		SDebugLog.LogView (dic_NPCInofData.Count.ToString());
	}

	public T LoadAndDeserialize<T>(string path, string fileName)
	{
		//string filePath = string.Format(path, Application.dataPath);

		string JsonFileName = string.Format("{0}/{1}", path, fileName );

		var streamReader = new System.IO.StreamReader(JsonFileName);
		string data = streamReader.ReadToEnd();
		streamReader.Close();

		return JsonConvert.DeserializeObject<T>(data);
	}

	void LoadTempNPCData()
	{
		TextAsset npcText = Resources.Load( "NPC_DATA_JSON_NET" ) as TextAsset;

		string data = npcText.ToString ();

		Dictionary <int, NPCInfoData> dic_NPCInofDataTemp = new Dictionary<int, NPCInfoData>();
		dic_NPCInofDataTemp = JsonConvert.DeserializeObject<Dictionary<int, NPCInfoData>>(data);

		foreach(KeyValuePair<int, NPCInfoData> kvp in dic_NPCInofDataTemp)
		{
			lst_NPCInfoDataTamp.Add (kvp.Value);
		}
	}

	public void SaveJsonFile()
	{
		string path;
		string fileName = "NPC_DATA_JSON_NET.txt";

		path = Application.temporaryCachePath;

		string JsonFileName = string.Format("{0}/{1}", path, fileName );

		StreamWriter stream_write;	
		stream_write = File.CreateText (JsonFileName);
	
		string jsonString = JsonConvert.SerializeObject ( dic_NPCInofData, Newtonsoft.Json.Formatting.Indented );

		stream_write.Write( jsonString );

#if UNITY_EDITOR
		Application.OpenURL (path);
#endif

		stream_write.Close();
	}
}
