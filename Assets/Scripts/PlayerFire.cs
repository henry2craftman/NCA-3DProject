using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// �÷��̾ Ű������ �����̽� ��ư�� ������ �� ��ü�� ������.
[ExecuteAlways]
public class PlayerFire : MonoBehaviour
{
    [SerializeField] GameObject throwObject;
    [SerializeField] GameObject rayEffect;
    [SerializeField] float throwPower = 5;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            // ��ź�� ����
            GameObject bomb = Instantiate(throwObject);
            bomb.transform.position = Camera.main.transform.position;

            // ������ ����
            Vector3 direction = Camera.main.transform.forward;

            // ���� ����
            bomb.GetComponent<Rigidbody>().AddForce(direction * throwPower, ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0)) // 0�� ����, 1�� ������, 2�� ��
        {
            //print(Input.mousePosition);
            //Vector3 touchWorldPos = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print(touchWorldPos);

            // �߻��� ������ ����
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100, Color.red, 5);

            // �΋H�� ������ ����
            RaycastHit hitInfo;

            // ���̸� �߻�
            if(Physics.Raycast(ray, out hitInfo))
            {
                GameObject effect = Instantiate(rayEffect);
                effect.transform.position = hitInfo.point;
                effect.transform.forward = hitInfo.normal; // ��������, �븻����
            }
        }
    }
}
