using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ٴڿ� ��ź�� ������ ����Ʈ�� ����� �������.
public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;
    [SerializeField] float radius = 5;
    [SerializeField] LayerMask layerMask;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(bombEffect);

        effect.transform.position = this.transform.position;

        // � ��ü���� �ִ��� Ȯ���ϰ� �ʹ�.
        CheckColliders();

        Destroy(this.gameObject);
    }

    private void CheckColliders()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, layerMask);

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<CharacterFSM>().OnStateChangeEvent(CharacterFSM.NPCState.Dance);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0.45f, 1, 0.5f);
        Gizmos.DrawSphere(transform.position, radius);
    }

    private void OnParticleCollision(GameObject other)
    {
        print(other.name);
    }
}
