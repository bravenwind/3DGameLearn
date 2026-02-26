using UnityEngine;

public class Magazine : MonoBehaviour, IInteractable
{
    [SerializeField]
    private int ammoAmount = 20;

    public string GetInteractText()
    {
        return "[F] Åº¾à »ç¿ë " + ammoAmount.ToString() + " HP";
    }

    public void Interact(GameObject player)
    {
        HitScanWeapon playerWeapon = player.GetComponent<HitScanWeapon>();
        if (playerWeapon != null)
        {
            playerWeapon.GetAmmo(ammoAmount);
            Destroy(gameObject);
        }
    }
}
