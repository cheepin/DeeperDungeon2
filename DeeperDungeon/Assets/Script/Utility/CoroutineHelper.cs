using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace util
{
    public static class CoroutineHelper
    {

		//---単に指定秒待った後にアクション
        static public IEnumerator DelaySecond(float waitSecond,Action action)
        {
            yield return new WaitForSeconds(waitSecond);
            action();
        }

        static public IEnumerator DelaySecondLoop(float waitSecond, int loopTimes, Action action)
        {
            int i = 0;
            while (i++ < loopTimes || loopTimes == -1)
            {
                yield return new WaitForSeconds(waitSecond);
                action();
            }
        }

        static public IEnumerator DelaySecondLoop(float waitSecond, Func<bool> loopCondition, Action action)
        {
            while (loopCondition())
            {
                yield return new WaitForSeconds(waitSecond);
                action();
            }
        }

        static public IEnumerator DelaySecondLoop(float waitSecond, int loopTimes, params Action[] actions)
        {
            int i = 0;
            while (i++ < loopTimes || loopTimes == -1)
                foreach (var action in actions)
                {
                    action();
                    yield return new WaitForSeconds(waitSecond);
                }
        }
		//---ループが終わった後にafterActionを実行する
		static public IEnumerator DelaySecondLoopWithAfterAction(float waitSecond, int loopTimes, Action action,Action afterAction)
        {
            int i = 0;
            while (i++ < loopTimes || loopTimes == -1)
            {
                yield return new WaitForSeconds(waitSecond);
                action();
            }
			afterAction();
        }


        //---IEnumeratorを返す関数を繋げて使用できる。
        static public IEnumerator Chain(MonoBehaviour corutineCaller, params IEnumerator[] actions)
        {
            foreach (IEnumerator action in actions)
            {
                yield return corutineCaller.StartCoroutine(action);
            }

        }

        //---単にコマンドを実行するだけ。Chainで使用する
        static public IEnumerator Do(Action action)
        {
            action();
            yield return 0;
        }

        //---単に待つだけ　主にChain内で使用
        static public IEnumerator WaitSecond(float waitSecond)
        {
            yield return new WaitForSeconds(waitSecond);
        }

		//---DelaySecondのWaitFixedUpdateバージョン
		static public IEnumerator WaitForEndOfFrame(Action action)
        {
            yield return new WaitForEndOfFrame();
            action();
        }

		
    } 
}