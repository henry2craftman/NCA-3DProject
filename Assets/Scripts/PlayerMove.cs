using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// �÷��̾ ������� Ű���� �Է��� �޾� Ư�� �ӵ��� �̵��Ѵ�.
public class PlayerMove : MonoBehaviour
{
    [SerializeField] CameraRotate cameraRotate;
    [SerializeField] float speed = 10;
    [SerializeField] float gravity = -10;
    [SerializeField] float jumpPower = 10;
    float yVelocity;
    bool isJumping = true;
    CharacterController controller;
    float mouseX;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        direction = Camera.main.transform.TransformDirection(direction);

        if(isJumping && controller.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;

            yVelocity = 0;
        }

        if(Input.GetButtonDown("Jump") && !isJumping) // Space bar�� ������
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        controller.Move(direction * speed * Time.deltaTime);

        if(cameraRotate.IsThirdPersonView)
            RotatePlayer();


        void RotatePlayer()
        {
            float horizontalInput = Input.GetAxis("Mouse X");

            mouseX += horizontalInput * speed * Time.deltaTime;

            Vector3 rotationValue = new Vector3(0, mouseX, 0);

            transform.eulerAngles = rotationValue;
        }
    }

}
