using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        if (active)
        {
            gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
