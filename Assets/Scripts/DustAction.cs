using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �ð� ���� �������.
public class DustAction : MonoBehaviour
{
    [SerializeField] float destroyTime = 1.5f;
    float currentTime;

    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
