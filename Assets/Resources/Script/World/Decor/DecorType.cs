using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorType
{
    public enum Type
    {
        None,
        Bone,
        Cactus,
        Oasis,
        Obelisk,
        Plant,
        Pyramid,
        Rock,
        Sand,
        Temple,
        Tree,
    }

    public static int GetTypeCount()
    {
        return System.Enum.GetValues(typeof(Type)).Length;
    }
}
