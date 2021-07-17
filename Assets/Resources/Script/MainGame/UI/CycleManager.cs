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
        // ���� ����Ŭ �ؽ�Ʈ ����
        nowText.text = Cycle.now.ToString();

        // ��ǥ ����Ŭ �ؽ�Ʈ ����
        maxText.text = Cycle.goal.ToString();
    }
}
