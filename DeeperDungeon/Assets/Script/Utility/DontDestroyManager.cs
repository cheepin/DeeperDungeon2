using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//---特定のシーンの遷移で破棄されないオブジェクトを作成
namespace util
{
	public class DontDestroyManager :  Singleton<DontDestroyManager>
	{
		Dictionary<GameObject,List<string>> registerdlist = new Dictionary<GameObject, List<string>>();
		static List<System.Action> FuncWhenNewLevelLoaded = new List<System.Action>(); 

		protected override void Awake()
		{
			base.Awake();

			//---新しくレベルが読み込まれた時、登録しておいたファンクタを発動
			foreach(var func in FuncWhenNewLevelLoaded)
			{
				func();
			}

			
		}

		private void Start()
		{
			SceneManager.sceneLoaded  += (scene,mode)=>{Onfadeout(scene.name,mode);}; 
			DontDestroyOnLoad(gameObject);
			
		}
		//---シーン判定　登録しておいたシーンと違う名前なら破棄
		private void Onfadeout(string sceneName,LoadSceneMode mode)
		{
			var deletedList = Instance.registerdlist.Where((x)=>mode == LoadSceneMode.Single && !x.Value.Contains(sceneName)).Select((x)=>(x.Key)).ToList();
			deletedList.ForEach((x)=>{Destroy(x);});
			
		}

		static public void Set(string sceneName,GameObject target)
		{
			target.transform.SetParent(Instance.transform);
			if(Instance  ==null)
				print("NULL ERROR");
			//---初めてレジスターリストを作成する場合
			if(!Instance.registerdlist.ContainsKey(target))
			{
				Instance.registerdlist[target] = new List<string>()	{sceneName};
			}
			else
				Instance.registerdlist[target].Add(sceneName);
		}
			
		static public void SetFuncWhenNewLevelLoaded(System.Action functor)
		{
			FuncWhenNewLevelLoaded.Add(functor);
		}
		static public void RemoveFuncWhenNewLevelLoaded(System.Action functor)
		{
			FuncWhenNewLevelLoaded.Remove(functor);
		}
	}

}