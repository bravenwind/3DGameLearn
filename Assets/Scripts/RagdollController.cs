using UnityEngine;
using System.Collections.Generic;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>(); 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            if (rigidbodies[i].gameObject == gameObject)
            {
                continue;
            }

            ragdollRigidbodies.Add(rigidbodies[i]);
        }

        DIsableRagdoll();
    }

    public void DIsableRagdoll()
    {
        for (int i = 0; i < ragdollRigidbodies.Count; i++)
        {
            ragdollRigidbodies[i].isKinematic = true;
        }

        if (animator != null)
        {
            animator.enabled = true;
        }
    }

    public void EnableRagdoll()
    {
        for (int i = 0; i < ragdollRigidbodies.Count; i++)
        {
            ragdollRigidbodies[i].isKinematic = false;
        }

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnableRagdoll();
        }
    }
}
