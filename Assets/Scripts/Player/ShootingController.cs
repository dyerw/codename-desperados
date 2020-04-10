using UnityEngine;
using UnityEngine.Networking;

public class ShootingController : NetworkBehaviour
{
    [SerializeField]
    Camera fpsCam;

    public Weapon equippedWeapon;

    [SerializeField]
    LayerMask shootableMask;

    [SerializeField]
    FirstPersonHUDController hudController;

    private void Awake()
    {
        hudController.SetEquippedWeaponText(equippedWeapon.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
    }

    [Client]
    void Shoot()
    {
        RaycastHit _hit;
        if (
            Physics.Raycast(
                fpsCam.transform.position,
                fpsCam.transform.forward,
                out _hit,
                equippedWeapon.range,
                shootableMask
            )
        )
        {
            Debug.Log("We hit " + _hit.collider.name);
            string hitTag = _hit.collider.tag;
            Transform hitTransform = _hit.transform;
            bool hitPlayer = hitTag == "PlayerHead" || hitTag == "PlayerBody" || hitTag == "PlayerLegs";
            if (hitPlayer)
            {
                switch (hitTag)
                {
                    case "PlayerHead":
                        CmdPlayerShot(hitTransform.parent.parent.name, equippedWeapon.headDamage);
                        break;
                    case "PlayerBody":
                        CmdPlayerShot(hitTransform.parent.parent.name, equippedWeapon.bodyDamage);
                        break;
                    case "PlayerLegs":
                        CmdPlayerShot(hitTransform.parent.parent.name, equippedWeapon.legDamage);
                        break;
                }
            }
        }
    }

    [Command]
    void CmdPlayerShot(string _ID, int damage)
    {
        Debug.Log(_ID + " has been shot for " + damage);
        GameObject hitPlayer = GameManagerSingleton.Instance.GetPlayer(_ID);
        HealthController healthController = hitPlayer.GetComponent<HealthController>();
        if (!healthController)
        {
            Debug.LogError(_ID + " got hit but does not have a health controller!");
        }
        healthController.RpcTakeDamage(damage);
    }
}
