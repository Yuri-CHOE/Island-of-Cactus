using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cup_move : MonoBehaviour
{
    public GameObject[] Cup;
    private float Speed = 50f;

    //Shuffle 함수(컵 섞기)
    public void Shuffle(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            int destIndex = Random.Range(0, gameObjects.Length);
            GameObject source = gameObjects[i];
            GameObject dest = gameObjects[destIndex];

            if (source != dest)
            {
                Vector3 tmp = source.transform.position;
                source.transform.position = dest.transform.position;
                dest.transform.position = tmp;

                gameObjects[i] = gameObjects[destIndex];
            }
        }
    }

    void Start()
    {
        Shuffle(Cup);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Shuffle(Cup);
        }
    }

    public void swap(Vector3 a, Vector3 b)
    {
        transform.position = Vector3.MoveTowards(a, b, Speed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(b, a, Speed * Time.deltaTime);
    }//함수 수정해야함

}
