using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace util {
public class DirectionHelper{
    public enum Direction : int { Down = 0, Left = 1, Right = 2, Up = 3 };

    static public Vector3 MapByNowDirection(Direction nowDirect, float moveAmountStr, float moveAmountWeak)
    {
        Vector3 mapedPosition = new Vector3();
        switch (nowDirect)
        {
            case Direction.Down:
                mapedPosition = new Vector3(moveAmountWeak, -moveAmountStr, 0);
                break;
            case Direction.Left:
                mapedPosition = new Vector3(-moveAmountStr, moveAmountWeak, 0);
                break;
            case Direction.Right:
                mapedPosition = new Vector3(moveAmountStr, moveAmountWeak, 0);
                break;
            case Direction.Up:
                mapedPosition = new Vector3(moveAmountWeak, moveAmountStr, 0);
                break;
        }
        return mapedPosition;
    }

        static public Vector3 GetDirection(Direction nowDirect)
        {
            Vector3 mapedPosition = new Vector3();
            switch (nowDirect)
            {
                case Direction.Down:
                    mapedPosition = Vector3.down;
                    break;
                case Direction.Left:
                    mapedPosition = Vector3.left;
                    break;
                case Direction.Right:
                    mapedPosition = Vector3.right;
                    break;
                case Direction.Up:
                    mapedPosition = Vector3.up;
                    break;
            }
            return mapedPosition;
        }



        static public Direction GetDirectivity(float xDir,float yDir)
        {
            if (Mathf.Abs(xDir) > Mathf.Abs(yDir))
                return (xDir >= 0) ? Direction.Right : Direction.Left;
            else
                return (yDir >= 0) ? Direction.Up : Direction.Down;

        }
    }




}