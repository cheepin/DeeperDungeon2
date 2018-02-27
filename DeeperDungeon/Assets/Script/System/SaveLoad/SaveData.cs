using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using skill;

//---ベース６４にコンバートできる
//---クラスごとセーブできる
namespace util
{

	[Serializable]
	public class ConvertableBase64
	{
		public string DataName{get {return dataName; } }

		string dataName;
		string winSavePath= "C:/Users/fujit/Downloads/savedata/";
		public ConvertableBase64(string name)
		{
			dataName = name;
		}

		public virtual void SaveData()
		{
			#if UNITY_EDITOR
			util.saveload.Save.SaveObjToPath(dataName,this,winSavePath+dataName+".txt");
			#endif 

			#if UNITY_ANDROID
			util.saveload.Save.SaveObjToPlayerPref(dataName,this);
			#endif

			#if UNITY_IOS
			#endif
		}
		public ConvertableBase64 LoadData()
		{

			ConvertableBase64 copyed;
			#if UNITY_EDITOR
			copyed = util.saveload.Load.LoadObjFromPath<ConvertableBase64>(dataName,winSavePath+dataName+".txt");
			#endif 

			#if UNITY_ANDROID
			copyed = util.saveload.Load.LoadPlayerPref<ConvertableBase64>(dataName);
			#endif

			return copyed;

		}

		public void DeleteData()
		{
			#if UNITY_EDITOR_WIN
			util.saveload.Remove.RemoveObjFromPath(winSavePath);
			#endif
			#if UNITY_ANDROID
			util.saveload.Remove.RemovePrefData(dataName);
			#endif
		}

	}
}

namespace moving
{
	
}

[Serializable]
public class StatusData:util.ConvertableBase64
{
	public StatusData(string name):base(name)
	{
	}

	private int maxHP;
	private int hp;
	private int defense;
	private int attack;

	public int MaxHP
	{
		get {return maxHP;}
		set 
		{
			maxHP = value;
			CallBackAction(nameof(MaxHP),value);

		}
	}
	public int HP
	{
		get {return hp;}
		set 
		{
			if(maxHP<value)
				hp = maxHP;
			else
				hp = value;
			CallBackAction(nameof(HP),value);
		}
	}
	public int Defense
	{
		get {return defense;}
		set 
		{
			defense = value;
			CallBackAction(nameof(Defense),value);
		}
	}
	public int Attack
	{
		get{ return attack;}
		set 
		{
			attack = value;
			CallBackAction(nameof(Attack),value);
		}
	}
	public bool Poisoned{get;set;}
	public int PoisonResistance{get;set; }
	public int FireResistance{get;set; }
	public int IceResistance{get;set; }
	public int LightningResistance{get;set; }

	public enum Enchant
	{
		Fire,Ice,Electrical
	}

	//---データが入力された時に呼ばれるコールバック
	private Dictionary<string,Action<int>> ActionWhenParamUpdate = new Dictionary<string, Action<int>>();
	private List<string> ActionNameList = new List<string>();
	public void SetActionWhenParamUpdate(string name,Action<int> action,string actionName,bool canDuplication)
	{
		if(ActionWhenParamUpdate.ContainsKey(name))
		{
			if(!ActionNameList.Any(X => X == actionName))
			{
				ActionWhenParamUpdate[name] += action;
				ActionNameList.Add(actionName);

			}

		}
		else
		{
			ActionWhenParamUpdate[name] = action;
			ActionNameList.Add(actionName);

		}
	}

	protected void CallBackAction(string ownParamName,int settedValue)
	{
		if(ActionWhenParamUpdate.ContainsKey(ownParamName))
		{
			ActionWhenParamUpdate[ownParamName](settedValue);
		}
	}

	//---オーバーライド
	public override void SaveData()
	{
		ActionWhenParamUpdate.Clear();
		base.SaveData();
	}

}


[Serializable]
public class PlayerData:StatusData
{
	public PlayerData():base("PlayerData")
	{
	}
	public int  CurrentLevel{get;set; }


	public int NumberOfGem
	{
		set
		{
			numberOfGem = value;
			SkillManager.SetGemWhenFillExpBar(numberOfGem);

		}	
		get
		{
			return numberOfGem; 
		}
	}
    public int CurrentExp
	{
		set
		{ 
			currentExp = value;
			Debug.Assert(AmountToNextLevel!=0,"AmountToNextLevelが０なので無限ループになります");
			while(currentExp >=AmountToNextLevel)
			{
				currentExp -= AmountToNextLevel;
				NumberOfGem +=1;
				AmountToNextLevel = (int)(AmountToNextLevel*1.17f);
				CallBackAction("LevelUp",++Level);
				SoundManager.GetGem();
				
			}
			CallBackAction(nameof(CurrentExp),value);
		}
		get
		{
			return currentExp;
		}
		
		
	}

	public void CopyData(PlayerData fromData)
	{
		//---基底クラス
		Attack = fromData.Attack;
		Defense = fromData.Defense;
		MaxHP = fromData.MaxHP;
		HP = fromData.HP ;
		MaxMana = fromData.MaxMana;
		FireResistance = fromData.FireResistance;
		IceResistance = fromData.IceResistance;
		LightningResistance = fromData.LightningResistance;
		PoisonResistance = fromData.PoisonResistance;
		Level = fromData.level;
		//---自分のクラス
		CurrentLevel = fromData.CurrentLevel;
		NumberOfGem = fromData.NumberOfGem;
		AmountToNextLevel = fromData.AmountToNextLevel;
		CurrentExp = fromData.CurrentExp;
		CurrentFloor = fromData.CurrentFloor;
		Difficulty = fromData.Difficulty;
		OnceRecover = fromData.OnceRecover;
	}
	
	public int CurrentFloor
	{
		get{return currentFloor;}
		set
		{
			currentFloor = value;
			CallBackAction(nameof(CurrentFloor),value);
		}
	}

	public int Level
	{
		get{ return level;}
		set
		{
			level = value;
		}
	}

	public int AmountToNextLevel{ set; get; }
    public int MaxMana { set; get; }
	public int Difficulty{set;get; }
	public bool OnceRecover{set;get;} = false;

	private int numberOfGem;
	private int currentExp;
	private int currentFloor;
	private int level;
}
