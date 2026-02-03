using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float throwForce = 15.0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            LaunchGrenade();
        }
    }

    void LaunchGrenade()
    {
        GameObject go = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);
        
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
