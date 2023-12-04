using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 사용자의 마우스 입력을 받아서 고개(카메라)를 돌린다.
// 2. 키보드 3번 키를 누르면 3인칭 뷰로 전환한다.
public class CameraRotate : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool isThirdPersonView = false;
    [SerializeField] GameObject thirdPersonPosition;
    [SerializeField] float speed = 10;
    float mouseX, mouseY;
    Vector3 originCamPos;

    private void Start()
    {
        originCamPos = transform.localPosition;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            isThirdPersonView = !isThirdPersonView;

            if (isThirdPersonView)
            {
                transform.SetParent(thirdPersonPosition.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.SetParent(thirdPersonPosition.transform.parent);
                transform.localPosition = originCamPos;
                transform.localRotation = Quaternion.identity;

            }
        }

        if(!isThirdPersonView) // 1인칭의 경우
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            mouseX += horizontalInput * speed * Time.deltaTime;
            mouseY += verticalInput * speed * Time.deltaTime;

            mouseY = Mathf.Clamp(mouseY, -90, 90);

            Vector3 rotationValue = new Vector3(-mouseY, mouseX, 0);

            transform.eulerAngles = rotationValue;
        }
        else // 3인칭의 경우
        {
            transform.LookAt(target);
        }
    }
}
