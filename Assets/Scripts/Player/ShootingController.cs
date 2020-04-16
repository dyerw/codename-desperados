using System.Collections;

using UnityEngine;

using Photon.Pun;

using Desperados.Game;

public class ShootingController : MonoBehaviourPun
{
    [SerializeField]
    Camera fpsCam;

    public Weapon equippedWeapon;

    [SerializeField]
    LayerMask shootableMask;

    [SerializeField]
    FirstPersonHUDController hudController;

    [SerializeField]
    Animator gunAnimator;

    private float lastShotTime;
    private int currentBullets;
    private bool isReloading = false;

    private void Awake()
    {
        hudController.SetEquippedWeaponInfo(equippedWeapon);
        lastShotTime = -100f;
        currentBullets = equippedWeapon.bulletCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1") && !isReloading)
            {
                if (Time.time - lastShotTime >= equippedWeapon.fireRate)
                {
                    if (currentBullets > 0)
                    {
                        Shoot();
                    } else
                    {
                        EffectManager.Instance.SyncOutOfAmmoEffect(transform.position);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
        
    }

    IEnumerator Reload()
    {
        isReloading = true;
        EffectManager.Instance.SyncReloadEffect(transform.position);

        yield return new WaitForSecondsRealtime(equippedWeapon.reloadTime); 

        currentBullets = equippedWeapon.bulletCapacity;
        hudController.SetCurrentAmmo(currentBullets);
        isReloading = false;
    }

    void Shoot()
    {
        currentBullets--;
        hudController.SetCurrentAmmo(currentBullets);
        EffectManager.Instance.SyncGunshotEffect(transform.position, Vector3.zero, "foo");
        gunAnimator.SetTrigger("ShotFired");
        lastShotTime = Time.time;
        RaycastHit _hit;
        if (
            Physics.Raycast(
                fpsCam.transform.position,
                fpsCam.transform.forward,
                out _hit,
                Mathf.Infinity,
                shootableMask
            )
        )
        {
            Shootable.ShootableType shootableType = _hit.transform.gameObject.GetComponent<Shootable>().shootableType;
            EffectManager.Instance.SyncGunHitEffect(_hit.point, shootableType);

            string hitTag = _hit.collider.tag;
            bool hitPlayer = hitTag == "PlayerHead" || hitTag == "PlayerBody" || hitTag == "PlayerLegs";
            if (hitPlayer)
            {
                PhotonView photonView = PhotonView.Get(_hit.transform.parent.parent.gameObject);
                photonView.RPC("RpcGetHit", RpcTarget.All, equippedWeapon.name, hitTag);
            }
        }
    }
}
