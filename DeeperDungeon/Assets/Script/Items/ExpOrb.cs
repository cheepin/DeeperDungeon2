using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using moving.player;

namespace item
{
	static public class ExpOrbList
	{
		public static Dictionary<int, Color> ColorList = new Dictionary<int, Color>()
		{
			//---必ず降順で書くこと
			{5000,Color.red},
			{500,Color.cyan},
			{50,Color.yellow }
		};

	}


	public class ExpOrb : FloatingObject
	{
		Player player;
		public int Exp
		{
			get
			{
				return exp;
			}
			set
			{
				exp = value;
				//---EXPを指定の値で割る　オーブの色付け
				numberOfOrb = DivideBurstOrbNumber(exp);
				//---バーストをEXPの数に設定

				var burst = new ParticleSystem.Burst[1];
				burst[0] = new ParticleSystem.Burst(0, numberOfOrb, numberOfOrb, 1, 0.01f);
				myParticalSystem.emission.SetBursts(burst);
			}
		}
		private int exp;
		int bigBall = 0;

		Color bigBallColor = Color.yellow;
		Color superBigBallColor = Color.cyan;


		protected override void Start()
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			base.Start();
		}

		
		//---新しくオーブが設定したコライダーに入ったら呼ばれる
		protected override void ActionWhenNewOrbEntered(int newComer, List<ParticleSystem.Particle> enter)
		{
			//---ビッグボールの数が前より増えていたらbieballExpPointの経験値を得る	


			//---取得したボールの数
			int bigballNum=0;
			for(int i = 0; i < gettedBigBallList.Count; i++)
			{
				//---カラーを取得
				var enteredBigBallNum = enter.Count(X => X.startColor == ExpOrbList.ColorList.ElementAt(i).Value);
				
				//---ストックしているカラーボールより入ってきたカラーボールの方が多かったら
				if(gettedBigBallList[i] < enteredBigBallNum)
				{
					player.SetExp(ExpOrbList.ColorList.ElementAt(i).Key * (enteredBigBallNum - gettedBigBallList[i]));
					gettedBigBallList[i] = enteredBigBallNum;
					bigballNum++;
				}
			}
			player.SetExp(newComer);
			SoundManager.GetOrb();
		}

		//---コライダーの判別　Trueを出したコライダーにオーブは追従する
		protected override bool SetFollowCorridor(Transform other)
		{
			if(other.tag == "Player")
				return true;
			else
				return false;
		}
		//---経験値に応じてパーティクルの色付け
		protected override void SettingEachParticle(ParticleSystem.Particle[] particle, int length)
		{
			int writeIndex=0;

			for(int i = 0; i < bigBallList.Count; i++)
			{
				for(int k =0; k < bigBallList[i]; k++)
				{
					particle[writeIndex].startColor = ExpOrbList.ColorList.ElementAt(i).Value;
					particle[writeIndex].startSize *= 1.3f;
					writeIndex++;
				}
			}
		}

		//---ExpをBigBallに割当
		List<int> bigBallList = new List<int>();
		List<int> gettedBigBallList = new List<int>();

		short DivideBurstOrbNumber(int _exp)
		{
			int remainExp=_exp;
			foreach(var expValue in ExpOrbList.ColorList.Select(X=>X.Key))
			{
				//---ビッグボールの数を算出
				bigBall = remainExp / expValue;
				bigBallList.Add(bigBall);
				//---現在ゲットしているビッグボールを図るための要素　当然０を入れる
				gettedBigBallList.Add(0);
				remainExp = (_exp % expValue);

			}
			return (short)(bigBallList.Aggregate((X,Y)=>X+Y) + (remainExp));
		}

	}

}