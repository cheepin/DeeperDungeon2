using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace item
{
	public class EmblemTexturePacker : MonoBehaviour,utilEditor.IPacker<EmblemTextureData>
	{
		[SerializeField]
		List<EmblemTextureData> dataList;
		Dictionary<string,Texture2D> emblemDict;
		[SerializeField]
		string targetPath;
		public Sprite this[string name]
		{
			get
			{
				if(emblemDict == null)
					CreateDict();
				return Sprite.Create(emblemDict[name],new Rect(0,0,128,124),new Vector2(0.5f,0.5f));
			}
		}

		public string TargetPath
		{
			get
			{
				return targetPath;
			}

			set
			{
				targetPath = value;
			}
		}

		public List<EmblemTextureData> DataList
		{
			get
			{
				return dataList;
			}

			set
			{
				dataList = value;
			}
		}

		public string DatalistFieldName=>dataList.ToString();

		public void CreateDict()
		{
			emblemDict = new Dictionary<string,Texture2D>();
			DataList.ForEach((X)=>emblemDict.Add(X.name,X.emblemTexture));
		}
	}

	[System.Serializable]
	public struct EmblemTextureData
	{
		public string name;
		[SerializeField]
		public Texture2D emblemTexture;
	}
}

namespace utilEditor
{
	

}