using UnityEngine;
using UnityEngine.Networking;

public class HealthController : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private FirstPersonHUDController hudController;

    [SyncVar(hook ="OnHealthChange")]
    private int currentHealth;

    private void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

    private void OnHealthChange(int _currentHealth)
    {
        hudController.SetHealthText(_currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health.");
    }
}
