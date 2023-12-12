using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키보드 I키를 누르면 Isometric 뷰로 전환, 회전x, 각도(x축 30도) 및 포지션 조절
public class IsometricCamera : MonoBehaviour
{
    [SerializeField] Vector3 cameraPos;
    [SerializeField] Vector3 cameraRot;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Camera.main.transform.SetParent(null);
            Camera.main.GetComponent<Camera>().orthographic = true;
            Camera.main.transform.position = cameraPos;
            Camera.main.transform.eulerAngles = cameraRot;
        }
    }
}
