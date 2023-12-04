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
    enum NPCStage
    {
        Idle,
        Talk,
        Return,
        follow
    }

    [SerializeField] NPCStage state = NPCStage.Idle;
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

    void Start()
    {
        player = GameObject.Find("Player").transform;
        mat = GetComponent<MeshRenderer>().material;
        characterController = GetComponent<CharacterController>();
        originPos = transform.position;

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
            case NPCStage.Idle:
                Idle();
                break;
            case NPCStage.Talk:
                Talk();
                break;
            case NPCStage.Return:
                Return();
                break;
            case NPCStage.follow:
                Follow();
                break;
        }
    }

    // 플레이어를 따라간다.
    private void Follow()
    {
        float distance = (player.transform.position - transform.position).magnitude;
        Vector3 direction = (player.transform.position - transform.position).normalized;

        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        characterController.Move(direction * speed * Time.deltaTime);

        if (distance > recognizeDistance)
        {
            mat.color = recognizeColors[(int)NPCStage.Return];

            state = NPCStage.Return;

            UpdateNPCDialogue("Oh No~~ Too fast!");
        }
    }

    private void Return()
    {
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        Vector3 direction = (originPos - transform.position).normalized;

        yVelocity += gravity * Time.deltaTime;
        direction.y = yVelocity;

        characterController.Move(direction * speed * Time.deltaTime);


        if (distanceToPlayer < recognizeDistance)
        {
            mat.color = recognizeColors[(int)NPCStage.follow];

            state = NPCStage.follow;

            UpdateNPCDialogue("Stop! I'm here!");
        }
        else if(distanceToOriginPos <= 0.3f)
        {
            state = NPCStage.Idle;
            UpdateNPCDialogue();
        }
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
                mat.color = recognizeColors[(int)NPCStage.Idle];

                state = NPCStage.Idle;
            }
            else
            {
                // 플레이어를 따라간다.
                mat.color = recognizeColors[(int)NPCStage.follow];

                state = NPCStage.follow;
                UpdateNPCDialogue("Following player");
            }
        }
    }

    private void Idle()
    {
        // 사용자의 입력이 들어온다면, 아래로 진행


        // 만약에 플레이어가 가까이 오면,
        float distance = Vector3.Distance(transform.position, player.position);
        //distance = (transform.position - player.position).magnitude;

        if(distance < recognizeDistance) // 인식 범위 내로 들어오면
        {
            // Hello 말하고 상태를 변경
            mat.color = recognizeColors[(int)NPCStage.Talk];

            state = NPCStage.Talk;

            transform.LookAt(player);

            UpdateNPCDialogue("Hello, Welcome!");
        }
    }
}
