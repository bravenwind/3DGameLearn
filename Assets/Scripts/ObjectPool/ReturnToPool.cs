using System.Collections;
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 2.0f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterLifeTime());
    }
    
    IEnumerator DisableAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        gameObject.SetActive(false);
    }
}
