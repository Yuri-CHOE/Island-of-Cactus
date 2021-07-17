using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleManager : MonoBehaviour
{
    [SerializeField]
    Text nowText = null;

    [SerializeField]
    Text maxText = null;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Refresh()
    {
        // 현재 사이클 텍스트 갱신
        nowText.text = Cycle.now.ToString();

        // 목표 사이클 텍스트 갱신
        maxText.text = Cycle.goal.ToString();
    }
}
