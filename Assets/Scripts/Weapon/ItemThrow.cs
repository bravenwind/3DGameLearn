using UnityEngine;

public class ItemThrow : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private GameObject stonePrefab;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float throwForce = 15.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Throw(grenadePrefab);
        }

        if (Input.GetKeyDown(KeyCode.G)) 
        {
            Throw(stonePrefab);
        }
    }

    void Throw(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, firePoint.position, firePoint.rotation);
        
        if (go != null) 
        {
            Rigidbody rb = go.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(firePoint.forward * throwForce, ForceMode.Impulse);
            }
        }
    }
}
