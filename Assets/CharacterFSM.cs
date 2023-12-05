using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

// ���ѻ��¸ӽ�(FSM)�� ���� ���۽�Ű�� �ʹ�.
// 1.Idle: ������
// 2.Recognize: �νĻ���(����� �� ��)
// 3.Behave: ��������(����ڰ� Ư�� �ൿ�� ���� ��)
public class CharacterFSM : MonoBehaviour
{
    public enum NPCState
    {
        Idle,
        Talk,
        Return,
        Follow,
        Turn,
        Dance
    }

    [SerializeField] NPCState state = NPCState.Idle;
    [SerializeField] float speed = 3;
    [SerializeField] float recognizeDistance = 5;
    [SerializeField] Color[] recognizeColors = new Color[4] { new Color(0, 0.45f, 1f, 1f), Color.red, Color.green, Color.blue };
    [SerializeField] GameObject npcDialogue;
    [SerializeField] TextMeshProUGUI npcDialogueText;
    [SerializeField] float talkTime = 3;

    Transform player;
    Material mat;
    CharacterController characterController;
    float currentTime;
    private float yVelocity;
    private float gravity = -10;
    Vector3 originPos;
    Animator animator;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        mat = GetComponent<MeshRenderer>().material;
        characterController = GetComponent<CharacterController>();
        originPos = transform.position;
        animator = GetComponentInChildren<Animator>();

        UpdateNPCDialogue();
    }

    void UpdateNPCDialogue(string dialogue = "")
    {
        if (dialogue == "")
        {
            npcDialogue.SetActive(false);
            npcDialogueText.text = dialogue;
            return;
        }

        npcDialogue.SetActive(true);
        npcDialogueText.text = dialogue;
    }


    void Update()
    {
        switch(state)
        {
            case NPCState.Idle:
                Idle();
                break;
            case NPCState.Talk:
                Talk();
                break;
            case NPCState.Return:
                Return();
                break;
            case NPCState.Follow:
                Follow();
                break;
            case NPCState.Dance:
                Dance();
                break;
        }
    }

    public void OnStateChangeEvent(NPCState _state)
    {
        state = _state;
        animator.SetTrigger("Dance");
    }

    // 3�� ���� ���� ���.
    private void Dance()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 3)
        {
            currentTime = 0;

            state = NPCState.Return;
            animator.SetTrigger("DanceToReturn");
        }
    }

    // �÷��̾ ���󰣴�.
    private void Follow()
    {
        // ĳ������ Forward�� �÷��̾ ���ϰ� ���� �ʴٸ�, Turn �ִϸ��̼� ����
        // ĳ������ Forward ������ �÷��̾��� ��������.

        float distance = (player.transform.position - transform.position).magnitude;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        transform.forward = direction;

        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        characterController.Move(direction * speed * Time.deltaTime);

        if (distance > recognizeDistance)
        {
            mat.color = recognizeColors[(int)NPCState.Return];

            state = NPCState.Return; // Trigger(Follow -> Return)
            animator.SetTrigger("FollowToReturn");

            UpdateNPCDialogue("Oh No~~ Too fast!");
        }
    }

    private void Return()
    {
        // ȸ�� õõ�� �� �� �ֵ��� ����
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        Vector3 direction = (originPos - transform.position).normalized;

        if (distanceToPlayer < recognizeDistance)
        {
            mat.color = recognizeColors[(int)NPCState.Follow];

            state = NPCState.Follow; // Trigger(Return -> Follow)
            animator.SetTrigger("ReturnToFollow");

            UpdateNPCDialogue("Stop! I'm here!");
        }
        else if (distanceToOriginPos <= 0.3f)
        {
            state = NPCState.Idle; // Trigger(Return -> Idle)
            animator.SetTrigger("ReturnToIdle");

            mat.color = recognizeColors[(int)NPCState.Idle];

            UpdateNPCDialogue();

            return;
        }


        transform.forward = direction;

        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        characterController.Move(direction * speed * Time.deltaTime);
    }

    private void Talk()
    {
        // ��ǳ���� �߸� ���� �Ѵ�. ���� ������, �ٸ� UI�� ��Ÿ����.
        currentTime += Time.deltaTime;

        if(currentTime > talkTime)
        {
            currentTime = 0;

            // ���࿡ �÷��̾ ������ ����,
            float distance = Vector3.Distance(transform.position, player.position);
            //distance = (transform.position - player.position).magnitude;

            // �ν� ���� ���� ������ ������, ������(Idle)�� ����
            if (distance > recognizeDistance) 
            {
                mat.color = recognizeColors[(int)NPCState.Idle];

                state = NPCState.Idle; // Trigger(Talk -> Idle)
                animator.SetTrigger("TalkToIdle");
            }
            else
            {
                // �÷��̾ ���󰣴�.
                mat.color = recognizeColors[(int)NPCState.Follow];

                state = NPCState.Follow; // Trigger(Talk -> Follow)
                animator.SetTrigger("TalkToFollow");

                UpdateNPCDialogue("Following player");
            }
        }
    }

    private void Idle()
    {
        // ���࿡ �÷��̾ ������ ����,
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < recognizeDistance) // �ν� ���� ���� ������
        {
            // Hello ���ϰ� ���¸� ����
            mat.color = recognizeColors[(int)NPCState.Talk];

            state = NPCState.Talk; // Trigger(Idle -> Talk)
            animator.SetTrigger("IdleToTalk");

            transform.LookAt(player);

            UpdateNPCDialogue("Hello, Welcome!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6) // Bomb Layer
        {
            state = NPCState.Dance;
            animator.SetTrigger("Dance");
        }
    }
}
