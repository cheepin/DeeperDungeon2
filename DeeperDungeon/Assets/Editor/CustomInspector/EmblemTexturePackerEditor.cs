using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using utilEditor;

namespace item
{
	[CustomEditor(typeof(EmblemTexturePacker))]
	public class EmblemTexturePackerEditor : DataPackerEditor<EmblemTexturePacker,EmblemTextureData>
	{
		protected override void CreateSetting(EmblemTexturePacker emblemTexturePacker)
		{
			emblemTexturePacker.DataList.Clear();
			var texture2DList = Assets.GetAssetsFromDirectory<Texture2D>(emblemTexturePacker.TargetPath,".psd");
			foreach(var newTexture in texture2DList)
			{
				emblemTexturePacker.DataList.Add(new EmblemTextureData
				{
					name = newTexture.name, 
					emblemTexture = newTexture
				});
			}
		}

	}

}


