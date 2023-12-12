using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ű���� IŰ�� ������ Isometric ��� ��ȯ, ȸ��x, ����(x�� 30��) �� ������ ����
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
