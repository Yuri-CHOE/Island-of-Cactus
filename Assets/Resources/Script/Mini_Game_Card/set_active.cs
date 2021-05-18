using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class set_active : MonoBehaviour
{
    public bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            gameObject.SetActive(true);
        }
    }
}
