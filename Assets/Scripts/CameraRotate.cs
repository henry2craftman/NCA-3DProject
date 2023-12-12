using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 사용자의 마우스 입력을 받아서 고개(카메라)를 돌린다.
// 2. 키보드 3번 키를 누르면 3인칭 뷰로 전환한다. 3인칭 카메라 포지션의 x축 20도 고정

public class CameraRotate : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool isThirdPersonView = false;
    public bool IsThirdPersonView { get { return isThirdPersonView; } }
    [SerializeField] GameObject thirdPersonObject;
    [SerializeField] Vector3 thirdPersonPosition; // 3인칭 뷰 포지션 값
    [SerializeField] Vector3 thirdPersonRotation; // 3인칭 뷰 각도
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

            if (isThirdPersonView) // 3인칭
            {
                transform.SetParent(thirdPersonObject.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Camera.main.fieldOfView = 80;
            }
            else // 1인칭
            {
                transform.SetParent(thirdPersonObject.transform.parent);
                transform.localPosition = originCamPos;
                transform.localRotation = Quaternion.identity;
                Camera.main.fieldOfView = 60;
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
