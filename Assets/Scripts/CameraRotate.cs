using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. ������� ���콺 �Է��� �޾Ƽ� ��(ī�޶�)�� ������.
// 2. Ű���� 3�� Ű�� ������ 3��Ī ��� ��ȯ�Ѵ�. 3��Ī ī�޶� �������� x�� 20�� ����

public class CameraRotate : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool isThirdPersonView = false;
    public bool IsThirdPersonView { get { return isThirdPersonView; } }
    [SerializeField] GameObject thirdPersonObject;
    [SerializeField] Vector3 thirdPersonPosition; // 3��Ī �� ������ ��
    [SerializeField] Vector3 thirdPersonRotation; // 3��Ī �� ����
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

            if (isThirdPersonView) // 3��Ī
            {
                transform.SetParent(thirdPersonObject.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                Camera.main.fieldOfView = 80;
            }
            else // 1��Ī
            {
                transform.SetParent(thirdPersonObject.transform.parent);
                transform.localPosition = originCamPos;
                transform.localRotation = Quaternion.identity;
                Camera.main.fieldOfView = 60;
            }
        }


        if(!isThirdPersonView) // 1��Ī�� ���
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            mouseX += horizontalInput * speed * Time.deltaTime;
            mouseY += verticalInput * speed * Time.deltaTime;

            mouseY = Mathf.Clamp(mouseY, -90, 90);

            Vector3 rotationValue = new Vector3(-mouseY, mouseX, 0);

            transform.eulerAngles = rotationValue;
        }
        else // 3��Ī�� ���
        {
            transform.LookAt(target);
        }
    }
}
