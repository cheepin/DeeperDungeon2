using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving.player;


public class WASDButton : HoldButton
{
    public Player player;
    public int MoveX, MoveY;

    protected override void Start()
    {
        pressed = false;
        clickFunc = ()=>player.Moving(MoveX, MoveY);
        base.Start();
    }
}
