using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Timeline;
using System.Collections;

public class RagdollController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private PlayerHealth playerHealth;

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

        if (playerHealth != null) 
        {
            playerHealth.OnDeath += EnableRagdoll;
        }
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

        if (playerHealth != null)
        {
            playerHealth.OnDeath -= EnableRagdoll;
            StartCoroutine(Co_DisableRagdoll());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            EnableRagdoll();
        }
    }

    IEnumerator Co_DisableRagdoll()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        for (int i = 0; i < ragdollRigidbodies.Count; i++)
        {
            ragdollRigidbodies[i].isKinematic = true;
        }
    }
}
