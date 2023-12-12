using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. ĳ���� �浹 ����Ȯ��
// 2. ���Ǿ� ĳ��Ʈ�� �߻�
// 3. �浹ü Ȯ��
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
            // 1. ĳ���� �浹 ����Ȯ��
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
                                // ĳ���Ͱ� ���� ���� �ִϸ��̼� ����
                                animator.SetTrigger("Open");
                                // �� �����ΰ� ���� ���� �ִϸ��̼� ����
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
                                // ������ Ư�� ��ġ�� ĳ���Ͱ� �̵�

                                // ������ ����

                                // ĳ���Ͱ� �ɴ� �ִϸ��̼� ����
                                break;
                        }
                    }
                }
            }
        }

    }
}
