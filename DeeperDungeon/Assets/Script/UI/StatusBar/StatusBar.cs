using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using util;
public class StatusBar : MonoBehaviour {

    public Image content;
	private StatusBar instance;
    [SerializeField]
    protected Text valueText;


	private void Awake()
	{
		if(instance == null)
			instance = this;
		else
		{
			Destroy(gameObject);
			return;
		}

	}
    
	public void SetCurrentValue(float currentParam, float maxParam)
    {
        content.fillAmount = MapToRatio(currentParam, maxParam);
		if(valueText != null)
			valueText.text = currentParam.ToString() + "  /  " + maxParam.ToString();
		
    }
	
	int currentValue;
	int maxValue;
	bool willDestroy=false;

	/// <summary>
	/// ゲージを最大値から時間に応じて減らしていく
	/// </summary>
	/// <param name="caller">StartCoroutineを実行するObj</param>
	/// <param name="decreaseTimeInterval">ゲージが消える時間</param>
	/// <param name="actionWhenStopBar">ゲージが０になった時に実行するデリゲート</param>
	public void StartDecreaseValueByTime(MonoBehaviour caller,float decreaseTimeInterval,Action actionWhenStopBar)
	{
		caller.StartCoroutine(CoroutineHelper.DelaySecondLoop(decreaseTimeInterval,-1,()=>
		{
			if(currentValue<1 && !willDestroy)
			{
				willDestroy = true;
				actionWhenStopBar();
				Destroy(gameObject);
			}
			else
				SetCurrentValue(--currentValue,maxValue);

		}));
	}



    protected float MapToRatio(float currentHP, float maxHP)
    {
        return currentHP / maxHP;
    }





	static Vector3 positioner = new Vector2(0,0);
	static int space = 62;
	static List<Vector3> barPosList = new List<Vector3>();

	/// <summary>
	/// ジェネリックバーを生成する
	/// </summary>
	/// <param name="color">カラーバーの色</param>
	/// <param name="startValue">スタートの値</param>
	/// <param name="maxValue">最大値</param>
	/// <returns></returns>
	public static StatusBar CreateGeneralBar(Color color,float startValue,float maxValue)
	{
		var newBarObj = Instantiate(Resources.Load("UIElem/StatusBar/GeneralBar"),GameObject.Find("Canvas").transform.Find("BarPanel")) as GameObject;

		//---既にバーが置かれていたら置かれていない位置まで下へスライド
		while(barPosList.Any(X=> X== newBarObj.transform.position))
			newBarObj.transform.position -= new Vector3(0,space,0);

		barPosList.Add(newBarObj.transform.position);

		var newBar =  newBarObj.GetComponent<StatusBar>();
		newBar.currentValue = (int)startValue;
		newBar.maxValue = (int) maxValue;
		newBar.SetCurrentValue(startValue,maxValue);
		newBar.content.color = color;
		return newBar;
	}

	private void OnDestroy()
	{
		barPosList.Remove(transform.position);
	}



}
