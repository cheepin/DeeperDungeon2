using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {
    private static DamageText damateText;
    private static GameObject canvas;

    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        damateText = Resources.Load<DamageText>("UIELem/DamageTexts/DamageText");
    }

    public static void CreateDamageText(string text,Vector2 location, Color textColor)
    {
        var instance = (Instantiate(damateText));
        instance.transform.SetParent(canvas.transform,false);
        instance.SetText(text, textColor);
        
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location);
        instance.transform.position = screenPosition;
        
    }

    



}
