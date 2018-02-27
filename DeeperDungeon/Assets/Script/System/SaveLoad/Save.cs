using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace util
{
	namespace saveload
	{

		[Serializable]
		public class Save 
		{
			//---PlayerPrefにBase64形式でオブジェクトを保存
			static public void SaveObjToPlayerPref<T>(string key,T obj)
			{
				var base64 = SerializeBase64(obj);
				PlayerPrefs.DeleteKey(key);
				PlayerPrefs.SetString(key,base64);
			}

			static public void SaveObjToPath<T>(string key,T obj,string path)
			{
				var base64 = SerializeBase64(obj);
				StreamWriter writer = File.CreateText(path);
				string json = "{ \"" + key + "\":" + base64;
				writer.Write(json);
				writer.Close();
			}

			//static public void SaveObjToPathAsJson(object obj)
			//{
			//	var stream = new MemoryStream();
			//	var serializer = new DataContractJsonSerializer(obj.GetType());
			//	serializer.WriteObject(stream,obj);
			//	var utf8 = Encoding.UTF8.GetString(stream.ToArray());
			//	Debug.Log(utf8);
			//}

			//---オブジェクトをBase64に変換
			static public string SerializeBase64<T>(T obj)
			{
				BinaryFormatter bineryformatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream();
				bineryformatter.Serialize(memoryStream, obj);
				string base64 = Convert.ToBase64String(memoryStream.GetBuffer());
				return base64;
			}
			//---指定したパスにテキストを保存
			static public void WriteText(string str,string path)
			{
				TextWriter textwriter = File.CreateText(path);
				textwriter.Write(str);
				textwriter.Close();
			}
		}

		static public class Load
		{
			//---PlayerPrefからロードする
			static public T LoadPlayerPref<T>(string key)
			{
				var base64 = PlayerPrefs.GetString(key);
				T yieldProduct =  DeserializeBase64<T>(base64);
				return yieldProduct;
			}
			//---保存していたbase64データを抜き出す
			static public T LoadObjFromPath<T>(string key, string path)
			{
				var base64 = StringPerser.ExtrustValueFromFile(key,path);
				T yieldProduct = DeserializeBase64<T>(base64);
				return yieldProduct;
			}


			//---Base64を指定したオブジェクトに戻す
			static public T DeserializeBase64<T>(string base64Data)
			{
				var binary = Convert.FromBase64String(base64Data);
				var stream = new MemoryStream(binary);
				BinaryFormatter bineryformatter = new BinaryFormatter();
				return (T)bineryformatter.Deserialize(stream);
			}
		
		}
	
		static public class Remove
		{
			static public void RemovePrefData(string key)
			{
				PlayerPrefs.DeleteKey(key);
			}
			static public void RemoveObjFromPath(string path)
			{
				File.Delete(path);
			}
		}
	}

	static class StringPerser
	{
		static public string ExtrustValueFromFile(string key, string filePath)
		{
			var reader = File.OpenText(filePath);
			string line;
			string data="null";
			while((line = reader.ReadLine()) != null)
			{
				if(line.Contains(key))
				{
					data = line.Split(':')[1];
				}
						
			}
			reader.Close();
			if(data==null)
				Debug.Log("Key was not Hit");
			return data;
		}

	}
	

}

