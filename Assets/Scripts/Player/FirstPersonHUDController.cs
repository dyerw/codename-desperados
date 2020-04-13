using UnityEngine;
using UnityEngine.UI;

public class FirstPersonHUDController : MonoBehaviour
{
    [SerializeField] Canvas hudCanvas;
    [SerializeField] Text healthText;
    [SerializeField] Text equippedWeaponText;

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

    public void SetEquippedWeaponText(string weaponName)
    {
        equippedWeaponText.text = weaponName;
    }
}
