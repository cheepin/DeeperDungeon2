using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using moving;
using moving.player;
using CH =util.CoroutineHelper;
using dungeon;
public class Stair : FloorObject
{
    
	void Start () {

    }
	bool onceTriggerd=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
			SoundManager.DownStair();

			var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			player.MyPlayerData.CurrentFloor++;
			player.SetInvisible(true);
			if(!DungeonManager.Instance.WillSaveFinish)
			{
				DungeonManager.Instance.DungeonLevel = player.MyPlayerData.CurrentFloor;
				player.StartCoroutine(CH.Chain(player,FadeIn.StartFadeOut(),
				CH.Do(()=>SceneManager.LoadScene("Dungeon", LoadSceneMode.Single)),
				CH.Do(()=>player.SetInvisible(false))));
			}
			else
			{
				SaveDataManager.SaveWholeGameData();
				SceneManager.LoadScene("Title", LoadSceneMode.Single);
			}
        }
    
    }
     
}
