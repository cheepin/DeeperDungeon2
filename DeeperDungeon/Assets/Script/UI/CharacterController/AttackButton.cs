using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CoroutineHelper = util.CoroutineHelper;
using moving.player;



public class AttackButton : HoldButton {

    // Use this for initialization
    Player player;
    ChargeBar chargeBar;

    private float chargeSpeed = 0.005f;
    
    Coroutine frushPlayer,barCharge;
    protected override void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        var canvas = player.transform.Find("PlayerCanvas");
        var chargeB = canvas.transform.Find("ChargeBar");
        chargeBar = chargeB.GetComponent<ChargeBar>();

        clickFunc       = player.AttemptAttackEnemy;
        holdFunc        = player.SpecialAttackEnemy;
        chargeCount     = 0;
        chargeTime      = 20;
        chargeBar.SetCurrentValue(chargeCount, chargeTime);
		LoadButtonSizeAndOpaque();

        base.Start();
    }

    void Update()
    {

        if (pressed)
        {
            chargeBar.gameObject.SetActive(true);
            
            if (barCharge == null)
                barCharge = StartCoroutine(
                    CoroutineHelper.DelaySecondLoop(chargeSpeed,-1, () =>
                    {
                        if(chargeCount <= chargeTime)
                        chargeBar.SetCurrentValue(chargeCount++, chargeTime);
                    }
                    ));
            //---チャージが完了したら点滅開始

            if (chargeCount > chargeTime && frushPlayer == null && player.TempPlayerData.Mana >= 30)
            {
                frushPlayer = StartCoroutine(
                    CoroutineHelper.DelaySecondLoop(0.1f,-1,
                     () => player.GetComponent<SpriteRenderer>().color = Color.white,
                     () => player.GetComponent<SpriteRenderer>().color = Color.red));
                chargeBar.content.color = Color.red; 
            }

        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        //---チャージバーを非表示
        chargeBar.gameObject.SetActive(false);
        chargeBar.content.color = Color.green;


        //---点滅を解除
        if (frushPlayer != null)
            StopCoroutine(frushPlayer);
        if (barCharge != null)
            StopCoroutine(barCharge);
        player.GetComponent<SpriteRenderer>().color = Color.white;
        frushPlayer = null;
        barCharge = null;
        chargeBar.SetCurrentValue(0, chargeTime);
        base.OnPointerUp(eventData);
    }

	static public void LoadButtonSizeAndOpaque()
	{
		var instance = GameObject.Find("AttackButton");
		instance.GetComponent<UnityEngine.UI.Image>().color = new Color(1,1,1,PlayerPrefs.GetFloat(optionData.OptionManager.id_AttackButonOpaque));

		var size =  PlayerPrefs.GetFloat(optionData.OptionManager.id_AttackButonSize);
		instance.transform.localScale = new Vector3(size,size,1);

	}

}
