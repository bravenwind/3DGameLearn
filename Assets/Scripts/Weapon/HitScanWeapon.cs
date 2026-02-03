using System;
using UnityEngine;

public class HitScanWeapon : MonoBehaviour
{
    [SerializeField]
    private float damage = 10.0f;

    [SerializeField]
    private float range = 100.0f;

    [SerializeField]
    private float fireRate = 0.2f;

    [SerializeField]
<<<<<<< Updated upstream
    private int currentAmmo;

    [SerializeField]
    private int maxAmmo = 20;

    public event Action<int, int> OnAmmoChanged;

    [SerializeField]
=======
>>>>>>> Stashed changes
    private LayerMask hitScanLayerMask;

    [SerializeField]
    private Camera fpsCamera;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private GameObject hitEffectPrefab;

    private float nextTimetoFire = 0.0f;
    private bool isHit = false;


    private void Start()
    {
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + fireRate;

            Shoot();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            return;
        }

        currentAmmo -= 1;

        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        if (muzzleFlash != null) 
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;

        isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, hitScanLayerMask);

        if (isHit) 
        {
            Debug.Log("맞은 대상: " + hit.transform.name);
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            if (damageable != null) 
            {
                damageable.TakeDamage(damage);
            }

            if (hitEffectPrefab != null) 
            {
                GameObject go = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(go, 2.0f);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range);

    }
}
