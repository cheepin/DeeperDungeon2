using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : util.Singleton<ResourceLoader> {

    // Use this for initialization
	public item.EmblemTexturePacker EmblemTexturePacker
	{
		get
		{
			if(emblemTexturePacker==null)
			{
				emblemTexturePacker = texturePackerRes.GetComponent<item.EmblemTexturePacker>();
			}
			return emblemTexturePacker;
		} 
		private set {emblemTexturePacker = value; }
	}
	private item.EmblemTexturePacker emblemTexturePacker;

    public GameObject expOrb;
    public GameObject texturePackerRes;
	Dictionary<string,GameObject> effectDict = new Dictionary<string, GameObject>();	

	static public GameObject GetResouce(string name)=> Instance.effectDict[name];

    private void Start()
    {
		DontDestroyOnLoad(gameObject);
        expOrb = Resources.Load<GameObject>("Items/ExpOrb");
	
		effectDict["frameBall"] = Resources.Load("Effect/frameBall") as GameObject;
		effectDict["Spark"] = Resources.Load("Effect/Spark") as GameObject;
		effectDict["Parry"] = Resources.Load("Effect/Parry") as GameObject;
		effectDict["SkillActivate"] = Resources.Load("Effect/SkillActivate") as GameObject;
		effectDict["Arrow"] = Resources.Load("SkillEffect/Arrow") as GameObject;
		effectDict["EdgeBrade"] = Resources.Load("SkillEffect/EdgeBrade") as GameObject;
		effectDict["SkeletonWarriorBrade"] = Resources.Load("Effect/SkeletonWarriorBrade") as GameObject;
		effectDict["CyanSkeletonWarriorBrade"] = Resources.Load("Effect/CyanSkeletonWarriorBrade") as GameObject;

		effectDict["Fire"] = Resources.Load("SkillEffect/Fire") as GameObject;
		effectDict["FireBall"] = Resources.Load("SkillEffect/FireBall") as GameObject;
		effectDict["VineTrap"] = Resources.Load("SkillEffect/VineTrap") as GameObject;
		effectDict["FrostFall"] = Resources.Load("SkillEffect/FrostFall") as GameObject;


    }

}
