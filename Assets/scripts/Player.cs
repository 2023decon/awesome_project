using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float firstDir = 0f;
    int multiKey = 0;

    void Update()
    {
        float input = GetAxisRaw(KeyCode.A, KeyCode.D);

        transform.Translate(Vector2.right * input * 0.5f);
    }

    float GetAxisRaw(KeyCode key1, KeyCode key2)
    {
        float dir = 0f;

        if (Time.timeScale == 0f) return dir;
        if (Input.GetKey(key1))
        {
            dir = -1f;

            if (Input.GetKeyDown(key1))
            {
                multiKey++;
            }

            if (multiKey == 1)
            {
                firstDir = -1;
            }
        }

        if (Input.GetKey(key2))
        {
            dir = 1f;

            if (Input.GetKeyDown(key2))
            {
                multiKey++;
            }

            if (multiKey == 2)
            {
                firstDir = 1;
            }
        }

        if (multiKey == 2)
        {
            dir = -firstDir;
        }

        if (Input.GetKeyUp(key1) || Input.GetKeyUp(key2))
        {
            multiKey--;
        }

        if (multiKey == 0)
        {
            dir = 0;
        }

        return dir;
    }
}
