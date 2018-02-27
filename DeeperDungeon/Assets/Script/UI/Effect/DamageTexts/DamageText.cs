using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    public Animator animator;
    Text damageText;

	void OnEnable () {
        damageText = animator.GetComponent<Text>();
        Destroy(gameObject,animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}
	
    public void SetText(string text,Color textColor)
    {
        damageText.text = text;
        damageText.color = textColor;
    }

	void Update () {
	}
}
