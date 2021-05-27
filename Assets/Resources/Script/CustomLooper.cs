using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CustomLooper
{
    Coroutine _coroutine;
    public Coroutine coroutine { get { return _coroutine; } set { isStart = true; _coroutine = value; } }

    public bool isStart;
    public bool isFinish;
}
