using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일정 시간 이후 사라진다.
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
