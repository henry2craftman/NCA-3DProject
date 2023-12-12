using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
}
