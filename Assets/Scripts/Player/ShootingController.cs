using UnityEngine;

using Photon.Pun;

public class ShootingController : MonoBehaviourPun
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
        if (photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("I am shooting");
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        photonView.RPC("PlayGunshot", RpcTarget.All);
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
