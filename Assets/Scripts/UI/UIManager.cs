using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image healthImage;

    [SerializeField]
    private PlayerHealth playerHealth;

    [SerializeField]
    private HitScanWeapon hitScanWeapon;

    [SerializeField]
    private DamageVignette damageVignette;

    [SerializeField]
    private TMP_Text currentAmmoText;

    [SerializeField]
    private TMP_Text maxAmmoText;

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
            playerHealth.OnHealthChanged += damageVignette.UpdateVignette;
        }
        
        if (hitScanWeapon != null)
        {
            hitScanWeapon.OnAmmoChanged += UpdateAmmoUI;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
            playerHealth.OnHealthChanged -= damageVignette.UpdateVignette;
        }

        if (hitScanWeapon != null)
        {
            hitScanWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }
    }

    public GameObject optionUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionUI.SetActive(!optionUI.activeInHierarchy);
            if (optionUI.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void UpdateHealthUI(float percent)
    {
        if (healthImage != null) 
        {
            healthImage.fillAmount = percent;
        }
    }

    void UpdateAmmoUI(int current, int max)
    {
        if (currentAmmoText != null)
        {
            currentAmmoText.text = current.ToString();
        }

        // 여기서 MaxAmmo도 같이 갱신해줍니다.
        // 매번 갱신하는 게 낭비 같아 보일 수 있지만, 
        // 텍스트 할당 연산은 매우 가볍기 때문에 코드를 분리하는 것보다 훨씬 깔끔합니다.
        if (maxAmmoText != null)
        {
            maxAmmoText.text = max.ToString();
        }
    }
}
