using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using moving;
using Direction = util.DirectionHelper.Direction;
namespace moving.player
{

	public class MockPlayer : Player
	{
		protected override void Start()
		{
			moveTime = 0.2f; 
			SetUp();
		}

		// Use this for initialization
		public override void Moving(float x, float y)
		{
			Vector2 inputAmount = AnalogStick.CalculateInputDegree(x, y);

			if(Mathf.Abs(inputAmount.x) > 0 || Mathf.Abs(inputAmount.y) > 0)
			{
				nowRunning = true;
				if(Mathf.Abs(inputAmount.x) > 0.5)
					animator.SetInteger("Direction", (inputAmount.x > 0) ? (int)Direction.Right : (int)Direction.Left);
				else
					animator.SetInteger("Direction", (inputAmount.y > 0) ? (int)Direction.Up : (int)Direction.Down);
			}

			//---現在方向を取得
			var preDireciton = nowDirection;
			nowDirection = (Direction)animator.GetInteger("Direction");



			//---位置が変わったら今走ってるコルーチンを削除
			//---これをやらないと滑った挙動になる
			if(nowDirection != preDireciton && smoothMovement != null)
			{
				nowMoving = false;
			}

			if(nowRunning == true && !dead && !nowAttack)
				base.AttemptMove(inputAmount.x, inputAmount.y);

			if(inputAmount.x == 0 && inputAmount.y == 0)
			{
				nowRunning = false;
			}

		}

		protected override void Update()
		{
			
		}
	}

}