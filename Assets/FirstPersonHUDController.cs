using UnityEngine;
using UnityEngine.UI;

public class FirstPersonHUDController : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text equippedWeaponText;

    public void SetHealthText(int amount)
    {
        healthText.text = amount.ToString();
    }

    public void SetEquippedWeaponText(string weaponName)
    {
        equippedWeaponText.text = weaponName;
    }
}
