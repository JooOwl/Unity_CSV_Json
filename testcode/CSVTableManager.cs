using UnityEngine;
using System.Collections;

public class CSVTableManager : MonoSingleton<CSVTableManager> 
{
	bool isLoadCSVOn = false;

	NPCStateTable NpcStateTableInfo;
	public NPCStateTable tNpcTableInfo { get { return NpcStateTableInfo; } }

	CSVSkillTable SkillTableInfo;
	public CSVSkillTable tSkillTableInfo { get { return SkillTableInfo; } }

	TextTable TextTableInfo;
	public TextTable tTextTableInfo { get { return TextTableInfo; } }

	EffectTable EffectTableInfo;
	public EffectTable tEffectTableInfo { get { return EffectTableInfo; } }

	void Awake()
	{
		if( mInstance == null )
		{
			mInstance = this;
		}

		if( this.transform.parent == null )
		{
			DontDestroyOnLoad(this);
		}

		isLoadCSVOn = false;
		LoadCSV ();
	}

	public void LoadCSV()
	{
		if( isLoadCSVOn )
		{
			return;
		}

		NpcStateTableInfo = new NPCStateTable ();
		SkillTableInfo = new CSVSkillTable();
		TextTableInfo = new TextTable ();
		EffectTableInfo = new EffectTable ();

		isLoadCSVOn = true;
	}

	public override void Init()
	{
		CreateInstance ();
	}

	static void CreateInstance ()
	{
		myInstance.LoadCSV ();
	}
}
