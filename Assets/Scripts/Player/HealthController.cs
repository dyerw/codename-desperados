using UnityEngine;

using Photon.Pun;
using Desperados.Game;

public class HealthController : MonoBehaviourPun 
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private FirstPersonHUDController hudController;

    [SerializeField]
    PlayerController playerController;

    private int currentHealth;

    private void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
        hudController.SetHealthText(currentHealth);
    }

    [PunRPC]
    public void RpcGetHit(string weaponName, string bodyLocation)
    {
        // Just for client
        if (photonView.IsMine)
        {
            Weapon weapon = GameManagerSingleton.Instance.weapons[weaponName];
            int damage = weapon.calculateDamage(bodyLocation);
            photonView.RPC("RpcTakeDamage", RpcTarget.All, damage);
        }
    }

    [PunRPC]
    public void RpcTakeDamage(int amount)
    {
        currentHealth -= amount;

        hudController.SetHealthText(currentHealth);

        if (currentHealth <= 0)
        {
            playerController.Die();
        }
    }
}
