using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reward_move_script : MonoBehaviour
{
    public Transform Target;
    public float speed = 500f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
        transform.position = Target.position;
    }
}
