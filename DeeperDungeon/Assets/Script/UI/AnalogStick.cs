using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.EventSystems;
using moving;
using moving.player;


public class AnalogStick :util.Singleton<AnalogStick>{

    Player player;
	static float x_LeftThreshold=0.55f;
	static float x_RightThreshold=0.55f;
	static float y_UpThreshold=0.55f;
	static float y_DownThreshold=0.55f;



	void Start () {
		dungeon.DontDestroyHelper.SetDontDestroy(gameObject);
		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		LoadThreshold();
		LoadAnalogStickSize();

    }

	static public void LoadThreshold()
	{
		x_LeftThreshold = 100 - PlayerPrefs.GetInt(optionData.OptionManager.id_X_LeftAxisSensitive);
		x_RightThreshold = 100 - PlayerPrefs.GetInt(optionData.OptionManager.id_X_RightAxisSensitive);
		y_UpThreshold = 100 - PlayerPrefs.GetInt(optionData.OptionManager.id_Y_UpAxisSensitive);
		y_DownThreshold = 100 - PlayerPrefs.GetInt(optionData.OptionManager.id_Y_DownAxisSensitive);

		x_LeftThreshold /= 100;
		x_RightThreshold /= 100;
		y_UpThreshold /= 100;
		y_DownThreshold /= 100;
	}

	static public void LoadAnalogStickSize()
	{
		float size = PlayerPrefs.GetFloat(optionData.OptionManager.id_AnalogStickSize);
		GameObject.Find("MobileJoystick").transform.localScale = new Vector3(size, size, 1);
		GameObject.Find("JoystickBase").transform.localScale = new Vector3(size, size, 1);
		GameObject.Find("JoystickBall").transform.localScale = new Vector3(size, size, 1);

		float opaque = 1.0f;//PlayerPrefs.GetFloat(optionData.OptionManager.id_AnalogStickOpaque);
		GameObject.Find("MobileJoystick").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, opaque);
		GameObject.Find("JoystickBase").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, opaque);
		GameObject.Find("JoystickBall").GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, opaque);

	}

    void Update () {

        float x = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Horizontal")*1,-1,1);
        float y = Mathf.Clamp(CrossPlatformInputManager.GetAxis("Vertical")*1,-1,1);
        if (Input.GetKey("w")) y = 1;
        if (Input.GetKey("a")) x = -1;
        if (Input.GetKey("s")) y = -1;
        if (Input.GetKey("d")) x=  1;
		player.Moving(x,y);
		
    }


    static public Vector2 CalculateInputDegree(float x,float y)
    {

		//---設定したスレッショルドより横軸が動いてたら判定
        if (x > x_RightThreshold)
        {
			//---設定したスレッショルドより縦軸が少なかったら右移動
            if (y >= 0 && y < y_UpThreshold)
            {
                x = 1;
                y = 0;
            }
            else if (y <= 0 && y > -y_DownThreshold)
            {
                x = 1;
                y = 0;
            }
            else if(y<-y_DownThreshold)
            {
                x = 0.87f;
                y = -0.87f;
            }
            else if (y>= y_UpThreshold) 
            {
                x = 0.87f;
                y = 0.87f;
            }
        }

        else if (x < -x_LeftThreshold)
        {
            if (y >= 0 && y < y_UpThreshold)
            {
                x = -1;
                y = 0;
            }
            else if (y <=0 && y > -y_DownThreshold)
            {
                x = -1;
                y = 0;
            }
            else if (y < -y_DownThreshold)
            {
                x = -0.87f;
                y = -0.87f;
            }
            else if(y>= y_UpThreshold)
            {
                x = -0.87f;
                y = 0.87f;
            }
        }
		//---横軸(x)の移動がXスレッショルドより低く、かつ縦軸(y)の移動がYスレッショルドより高い時
		else if(y > y_UpThreshold)
		{
			x = 0f;
			y = 1.0f;
		}
		else if(y < -y_DownThreshold)
		{
			x = 0f;
			y = -1.0f;
		}
		//---どの移動もスレッショルド以下なら0
		else
		{
			x = 0;
			y = 0;
		}

        return new Vector2(x,y);
    }

}
