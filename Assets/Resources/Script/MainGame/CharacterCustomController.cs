using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomController : MonoBehaviour
{
    /*
     * < �̱��� �䱸���� >
     * ĳ���� �׼� ����
     * ĳ���� �̵� ����
     * ĳ���� �ִ� �ּ� ��ǥ ����
     */

        


    [SerializeField]
    float posMinY = 1.9f;           // ĳ���� �ּ� ����
    [SerializeField]
    float posMaxY = 20f;            // ĳ���� �ִ� ����




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �� ����
        Tool.HeightLimit(transform, posMinY, posMaxY);
    }
}
