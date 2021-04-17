using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockType
{
    public enum Type
    {
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

}
