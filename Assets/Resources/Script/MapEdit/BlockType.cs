using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockType
{
    public enum Type
    {
        None,
        Normal,
        Event,
        Special,
    }


    public enum TypeDetail
    {
        none,

        plus,
        minus,

        boss,
        trap,
        lucky,

        shop,
        unique,
        shortcutIn,
        shortcutOut,
    }

    public static Type GetTypeByDetail(TypeDetail typeDetail)
    {
        Type ttt;
        switch (typeDetail)
        {
            case TypeDetail.plus:
                ttt = Type.Normal;
                break;

            case TypeDetail.minus:
                ttt = Type.Normal;
                break;

            case TypeDetail.boss:
                ttt = Type.Event;
                break;

            case TypeDetail.trap:
                ttt = Type.Event;
                break;

            case TypeDetail.lucky:
                ttt = Type.Event;
                break;

            case TypeDetail.shop:
                ttt = Type.Special;
                break;

            case TypeDetail.unique:
                ttt = Type.Special;
                break;

            case TypeDetail.shortcutIn:
                ttt = Type.Special;
                break;

            case TypeDetail.shortcutOut:
                ttt = Type.Special;
                break;

            default :
                ttt = Type.None;
                break;
        }
        return ttt;
    }

}
