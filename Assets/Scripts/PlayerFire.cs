using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 플레이어가 키보드의 스페이스 버튼을 눌렀을 때 물체를 던진다.
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
            // 폭탄을 생성
            GameObject bomb = Instantiate(throwObject);
            bomb.transform.position = Camera.main.transform.position;

            // 방향을 설정
            Vector3 direction = Camera.main.transform.forward;

            // 힘을 전달
            bomb.GetComponent<Rigidbody>().AddForce(direction * throwPower, ForceMode.Impulse);
        }

        if(Input.GetMouseButtonDown(0)) // 0번 왼쪽, 1번 오른쪽, 2번 휠
        {
            //print(Input.mousePosition);
            //Vector3 touchWorldPos = Camera.main.ScreenPointToRay(Input.mousePosition);
            //print(touchWorldPos);

            // 발사할 광선을 선언
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100, Color.red, 5);

            // 부딫힌 정보를 저장
            RaycastHit hitInfo;

            // 레이를 발사
            if(Physics.Raycast(ray, out hitInfo))
            {
                GameObject effect = Instantiate(rayEffect);
                effect.transform.position = hitInfo.point;
                effect.transform.forward = hitInfo.normal; // 법선벡터, 노말벡터
            }
        }
    }
}
