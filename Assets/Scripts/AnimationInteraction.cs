using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 캐릭터 충돌 여부확인
// 2. 스피어 캐스트를 발사
// 3. 충돌체 확인
public class AnimationInteraction : MonoBehaviour
{
    CharacterController cc;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            // 1. 캐릭터 충돌 여부확인
            //if ((cc.collisionFlags & CollisionFlags.Sides) != 0)
            {
                RaycastHit hit;
                Vector3 startPoint = transform.position + cc.center + Vector3.up * -cc.height * 0.5f;
                Vector3 endPoint = startPoint + Vector3.up * cc.height;

                Debug.DrawLine(startPoint, endPoint, Color.green, 3);


                if (Physics.CapsuleCast(startPoint, endPoint, cc.radius, transform.forward, out hit))
                {
                    if(hit.collider.tag == "InteractiveObject")
                    {
                        switch(hit.collider.name)
                        {
                            case "Door":
                                // 캐릭터가 문을 여는 애니메이션 실행
                                animator.SetTrigger("Open");
                                // 문 스스로가 문을 여는 애니메이션 실행
                                Door door = hit.collider.GetComponentInParent<Door>();

                                if(!door.isOpen)
                                {
                                    door.animator.SetTrigger("Open");
                                    door.isOpen = true;
                                }
                                else
                                {
                                    door.animator.SetTrigger("Close");
                                    door.isOpen = false;
                                }
                            
                                break;
                            case "Sofa":
                                // 소파의 특정 위치로 캐릭터가 이동

                                // 방향을 변경

                                // 캐릭터가 앉는 애니메이션 실행
                                break;
                        }
                    }
                }
            }
        }

    }
}
