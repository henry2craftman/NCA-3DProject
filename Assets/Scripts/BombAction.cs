using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 바닥에 폭탄이 닿으면 이펙트를 만들고 사라진다.
public class BombAction : MonoBehaviour
{
    [SerializeField] GameObject bombEffect;
    [SerializeField] float radius = 5;
    [SerializeField] LayerMask layerMask;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject effect = Instantiate(bombEffect);

        effect.transform.position = this.transform.position;

        // 어떤 물체들이 있는지 확인하고 싶다.
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
