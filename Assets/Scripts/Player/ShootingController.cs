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

    private void Awake()
    {
        hudController.SetEquippedWeaponText(equippedWeapon.name);
        lastShotTime = -100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Time.time - lastShotTime >= equippedWeapon.fireRate)
                {
                    Shoot();
                }
            }
        }
        
    }

    void Shoot()
    {
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
            Debug.Log("We hit " + _hit.collider.name);
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
