using System;
using UnityEngine;

public class HitScanWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private float damage = 10.0f;
    [SerializeField] private float range = 100.0f;
    [SerializeField] private float fireRate = 0.2f;

    [Header("Ammo Settings")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo = 20;

    // UI 매니저 등이 구독하여 남은 탄약을 표시할 수 있게 하는 이벤트
    public event Action<int, int> OnAmmoChanged;

    [Header("References & Effects")]
    [SerializeField] private LayerMask hitScanLayerMask;
    [SerializeField] private Camera fpsCamera;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject hitEffectPrefab;

    private float nextTimetoFire = 0.0f;
    private bool isHit = false;

    private void Start()
    {
        // 시작 시 탄약 풀충전 및 이벤트 발생
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
    }

    private void Update()
    {
        // 마우스 왼쪽 클릭 유지 시 연사 속도에 맞춰 발사
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimetoFire)
        {
            nextTimetoFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // 탄약이 없으면 발사 불가
        if (currentAmmo <= 0)
        {
            Debug.Log("탄약 부족!");
            return;
        }

        // 탄약 감소 및 UI 업데이트 알림
        currentAmmo -= 1;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);

        // 총구 화염 효과
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        RaycastHit hit;
        // 카메라 중심으로부터 레이캐스트 발사
        isHit = Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range, hitScanLayerMask);

        if (isHit)
        {
            Debug.Log("맞은 대상: " + hit.transform.name);

            // 상대방이 데미지를 입을 수 있는 객체인지 확인 (ZombieFSM 등)
            IDamageable damageable = hit.transform.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            //GameObject go = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject go1 = PoolManager.Instance.SpawnFromPool("HitEffect_Everywhere", hit.point, Quaternion.LookRotation(hit.normal));
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                GameObject go2 = PoolManager.Instance.SpawnFromPool("HitEffect_OnEnemy", hit.point, Quaternion.LookRotation(hit.normal));
                go2.GetComponent<ParticleSystem>().Play();
            }
            //Destroy(go, 2.0f);
        }
    }

    void OnDrawGizmos()
    {
        if (fpsCamera == null) return;

        // 에디터 뷰에서 사거리를 시각적으로 표시
        Gizmos.color = Color.green;
        Gizmos.DrawRay(fpsCamera.transform.position, fpsCamera.transform.forward * range);
    }
}