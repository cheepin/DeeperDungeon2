using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : StatusBar {

    void Start()
    {
        gameObject.SetActive(false);
        valueText.gameObject.SetActive(false);
    }
}
