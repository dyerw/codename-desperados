using UnityEngine;
using System.Collections;

using Photon.Pun;
using Desperados.Game;

public class HealthController : MonoBehaviourPun 
{
    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private FirstPersonHUDController hudController;

    [SerializeField]
    private RenderController renderController;

    [SerializeField]
    private Camera myCamera;

    [SerializeField]
    private AudioListener myListener;

    [SerializeField]
    private Canvas firstPersonHUD;

    [SerializeField]
    private PlayerSetup playerSetup;

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

    private void OnHealthChange(int _currentHealth)
    {
        hudController.SetHealthText(_currentHealth);
    }

    private void Die()
    {
        renderController.HideFirstPerson();
        renderController.HideThirdPerson();
        if (photonView.IsMine)
        {
            myCamera.enabled = false;
            myListener.enabled = false;
            LobbySingleton.Instance.EnableLobby();
            firstPersonHUD.enabled = false;
        }
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        // Move me to a spawn point
        GameObject[] spawnPoints = GameManagerSingleton.Instance.spawnPoints;
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnTransform = spawnPoints[index].transform;
        transform.position = spawnTransform.position;

        yield return new WaitForSeconds(3f);


        // Gimme all my health baby
        SetDefaults();
        playerSetup.SpawnSetup();        
    }

    [PunRPC]
    public void RpcGetHit(string weaponName, string bodyLocation)
    {
        // Just for client
        if (photonView.IsMine)
        {
            Weapon weapon = GameManagerSingleton.Instance.weapons[weaponName];
            int damage = weapon.calculateDamage(bodyLocation);
            Debug.Log("I got hit in the " + bodyLocation + " for " + damage.ToString());
            photonView.RPC("RpcTakeDamage", RpcTarget.All, damage);
            photonView.RPC("PlayHitSound", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RpcTakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
