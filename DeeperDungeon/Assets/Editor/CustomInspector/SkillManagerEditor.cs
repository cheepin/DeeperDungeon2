using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using utilEditor;


namespace skill
{

	[CustomEditor(typeof(SkillDataPacker))]
	public class SkillManagerEditor : DataPackerEditor<SkillDataPacker,SkillDataDict>
	{
		protected override void CreateSetting(SkillDataPacker emblemTexturePacker)
		{
			emblemTexturePacker.DataList.Clear();
			var texture2DList = Assets.GetAssetsFromDirectory<SkillData>(emblemTexturePacker.TargetPath,".asset");
			foreach(var newTexture in texture2DList)
			{
				emblemTexturePacker.DataList.Add(new SkillDataDict
				{
					name = newTexture.name, 
					skillData = newTexture
				});
			}
		}

	}
	


}