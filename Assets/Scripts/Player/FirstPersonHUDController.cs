using UnityEngine;
using UnityEngine.UI;

public class FirstPersonHUDController : MonoBehaviour
{
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Text healthText;
    [SerializeField] Text equippedWeaponText;
    [SerializeField] Text currentAmmoText;
    [SerializeField] Text maxAmmoText;

    public void EnableHUD()
    {

        hudCanvas.enabled = true;
    }

    public void DisableHUD()
    {
        hudCanvas.enabled = false;
    }

    public void SetHealthText(int amount)
    {
        healthText.text = amount.ToString();
    }

    public void SetEquippedWeaponInfo(Weapon weapon)
    {
        equippedWeaponText.text = weapon.name;
        maxAmmoText.text = weapon.bulletCapacity.ToString();
        currentAmmoText.text = weapon.bulletCapacity.ToString();
    }

    public void SetCurrentAmmo(int amount)
    {
        currentAmmoText.text = amount.ToString();
    }
}
