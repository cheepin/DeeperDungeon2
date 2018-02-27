using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using util;
using UnityEngine.Audio;
public class SoundManager : Singleton<SoundManager> {

	[SerializeField]
	AudioClip hit=null;
	[SerializeField]
	AudioClip receiveDamage=null;
	[SerializeField]
	AudioClip explocive=null;
	[SerializeField]
	AudioClip bubble=null;
	[SerializeField]
	AudioClip charge=null;
	[SerializeField]
	AudioClip wind=null;
	[SerializeField]
	AudioClip magic=null;
	[SerializeField]
	AudioClip magic2=null;
	[SerializeField]
	AudioClip skillActivate=null;
	[SerializeField]
	AudioClip levelUp=null;
	[SerializeField]
	AudioClip click1=null;
	[SerializeField]
	AudioClip click2=null;
	[SerializeField]
	AudioClip push=null;
	[SerializeField]
	AudioClip get=null;
	[SerializeField]
	AudioClip getOrb=null;
	[SerializeField]
	AudioClip getGem=null;
	[SerializeField]
	AudioClip downStair=null;
	[SerializeField]
	AudioClip openKey=null;
	[SerializeField]
	AudioClip parry=null;
	[SerializeField]
	AudioClip concentration=null;
	[SerializeField]
	AudioClip aura=null;
	[SerializeField]
	AudioClip skillSet = null;
	[SerializeField]
	AudioClip skillSet2 = null;
	[SerializeField]
	AudioClip swordBrade = null;
	//[SerializeField]
	//AudioClip get;






	AudioSource audioSource;
	public AudioMixer mixer;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
		
		audioSource = GetComponent<AudioSource>();
		//---音量の設定
		int seVolume = PlayerPrefs.GetInt(optionData.OptionManager.id_SEVolume);
		mixer.SetFloat("SEVolume",seVolume);

		audioSource.spatialize= false;
		//---空打ち
		Instance.audioSource.PlayOneShot(Instance.hit,0.0f);
	}
	
	static public void SetVolume(float gain)
	{
		Instance.mixer.SetFloat("SEVolume",gain);
	}


	static public void AttackHit()
	{
		Instance.audioSource.PlayOneShot(Instance.hit,0.5f);
	}

	static public void ReceiveDamage()
	{
		Instance.audioSource.PlayOneShot(Instance.receiveDamage,0.25f);
	}

	static public void Explocive()
	{
		Instance.audioSource.PlayOneShot(Instance.explocive,0.5f);
	}

	static public void Bubble()
	{
		Instance.audioSource.PlayOneShot(Instance.bubble,0.5f);
	}

	static public void Charge()
	{
		Instance.audioSource.PlayOneShot(Instance.charge,0.34f);
	}

	static public void Wind()
	{
		Instance.audioSource.PlayOneShot(Instance.wind,0.5f);
	}
	static public void Magic()
	{
		Instance.audioSource.PlayOneShot(Instance.magic,0.3f);
	}
	static public void Magic2()
	{
		Instance.audioSource.PlayOneShot(Instance.magic2,0.5f);
	}
	static public void SkillActivate()
	{
		Instance.audioSource.PlayOneShot(Instance.skillActivate,0.5f);
	}
	static public void LevelUp()
	{
		Instance.audioSource.PlayOneShot(Instance.levelUp,0.6f);
	}
	static public void Click1()
	{
		Instance.audioSource.PlayOneShot(Instance.click1,0.6f);
	}
	static public void Click2()
	{
		Instance.audioSource.PlayOneShot(Instance.click2,0.6f);
	}
	static public void Push()
	{
		Instance.audioSource.PlayOneShot(Instance.push,1.0f);
	}
	static public void Get()
	{
		Instance.audioSource.PlayOneShot(Instance.get,0.25f);
	}
	static public void GetOrb()
	{
		Instance.audioSource.PlayOneShot(Instance.getOrb, 0.5f);
	}
	static public void GetGem()
	{
		Instance.audioSource.PlayOneShot(Instance.getGem, 0.5f);
	}
	static public void DownStair()
	{
		Instance.audioSource.PlayOneShot(Instance.downStair, 0.6f);
	}
	static public void OpenKey()
	{
		Instance.audioSource.PlayOneShot(Instance.openKey, 0.6f);
	}
	static public void Parry()
	{
		Instance.audioSource.PlayOneShot(Instance.parry, 0.6f);
	}
	static public void Concentration()
	{
		Instance.audioSource.PlayOneShot(Instance.concentration, 0.6f);
	}

	static public void Aura()
	{
		Instance.audioSource.PlayOneShot(Instance.aura, 0.6f);
	}
	static public void SkillSet()
	{
		Instance.audioSource.PlayOneShot(Instance.skillSet, 0.6f);
	}
	static public void SkillSet2()
	{
		Instance.audioSource.PlayOneShot(Instance.skillSet2, 0.6f);
	}
	static public void SwordBrade()
	{
		Instance.audioSource.PlayOneShot(Instance.swordBrade, 0.32f);
	}
}
