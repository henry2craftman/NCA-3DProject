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

    // �÷��̾ ���󰣴�.
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
                mat.color = recognizeColors[(int)NPCStage.Idle];

                state = NPCStage.Idle;
            }
            else
            {
                // �÷��̾ ���󰣴�.
                mat.color = recognizeColors[(int)NPCStage.follow];

                state = NPCStage.follow;
                UpdateNPCDialogue("Following player");
            }
        }
    }

    private void Idle()
    {
        // ������� �Է��� ���´ٸ�, �Ʒ��� ����


        // ���࿡ �÷��̾ ������ ����,
        float distance = Vector3.Distance(transform.position, player.position);
        //distance = (transform.position - player.position).magnitude;

        if(distance < recognizeDistance) // �ν� ���� ���� ������
        {
            // Hello ���ϰ� ���¸� ����
            mat.color = recognizeColors[(int)NPCStage.Talk];

            state = NPCStage.Talk;

            transform.LookAt(player);

            UpdateNPCDialogue("Hello, Welcome!");
        }
    }
}
