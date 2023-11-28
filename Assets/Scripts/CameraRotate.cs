using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. ������� ���콺 �Է��� �޾Ƽ� ��(ī�޶�)�� ������.
// 2. Ű���� 3�� Ű�� ������ 3��Ī ��� ��ȯ�Ѵ�.
public class CameraRotate : MonoBehaviour
{
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

        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        mouseX += horizontalInput * speed * Time.deltaTime;
        mouseY += verticalInput * speed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -90, 90);

        Vector3 rotationValue = new Vector3(-mouseY, mouseX, 0);

        transform.eulerAngles = rotationValue;
    }
}
