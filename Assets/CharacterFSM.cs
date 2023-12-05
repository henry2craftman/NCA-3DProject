using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

// 유한상태머신(FSM)에 따라서 동작시키고 싶다.
// 1.Idle: 대기상태
// 2.Recognize: 인식상태(가까워 질 때)
// 3.Behave: 반응상태(사용자가 특정 행동을 했을 때)
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

    // 3초 동안 춤을 춘다.
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

    // 플레이어를 따라간다.
    private void Follow()
    {
        // 캐릭터의 Forward가 플레이어를 향하고 있지 않다면, Turn 애니메이션 시작
        // 캐릭터의 Forward 방향을 플레이어의 방향으로.

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
        // 회전 천천히 할 수 있도록 변경
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
        // 말풍선이 뜨며 말을 한다. 말이 끝나면, 다른 UI가 나타난다.
        currentTime += Time.deltaTime;

        if(currentTime > talkTime)
        {
            currentTime = 0;

            // 만약에 플레이어가 가까이 오면,
            float distance = Vector3.Distance(transform.position, player.position);
            //distance = (transform.position - player.position).magnitude;

            // 인식 범위 내로 밖으로 나가면, 원상태(Idle)로 복귀
            if (distance > recognizeDistance) 
            {
                mat.color = recognizeColors[(int)NPCState.Idle];

                state = NPCState.Idle; // Trigger(Talk -> Idle)
                animator.SetTrigger("TalkToIdle");
            }
            else
            {
                // 플레이어를 따라간다.
                mat.color = recognizeColors[(int)NPCState.Follow];

                state = NPCState.Follow; // Trigger(Talk -> Follow)
                animator.SetTrigger("TalkToFollow");

                UpdateNPCDialogue("Following player");
            }
        }
    }

    private void Idle()
    {
        // 만약에 플레이어가 가까이 오면,
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance < recognizeDistance) // 인식 범위 내로 들어오면
        {
            // Hello 말하고 상태를 변경
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
