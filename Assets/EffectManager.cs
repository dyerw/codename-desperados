using UnityEngine;

using Photon.Pun;

namespace Desperados.Game {
    public class EffectManager : MonoBehaviourPun
    {

        [SerializeField] AudioClip revolverShotClip;
        [SerializeField] AudioClip outOfAmmoClip;
        [SerializeField] AudioClip reloadClip;

        public static EffectManager Instance { get; private set; }

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else
            {
                Instance = this;
            }
        }

        #endregion

        #region ReloadEffect

        public void SyncReloadEffect(Vector3 originLocation)
        {
            DoReloadEffect(originLocation);
            photonView.RPC("RpcDoReloadEffect", RpcTarget.Others, originLocation);
        }

        [PunRPC]
        void RpcDoReloadEffect(Vector3 originLocation)
        {
            DoReloadEffect(originLocation);
        }

        private void DoReloadEffect(Vector3 originLocation)
        {
            AudioSource.PlayClipAtPoint(reloadClip, originLocation, 1.3f);
        }

        #endregion

        #region GunshotEffect
        public void SyncGunshotEffect(Vector3 originLocation, Vector3 hitLocation, string hitTag)
        {
            DoGunShotEffect(originLocation, hitLocation, hitTag);
            photonView.RPC("RpcDoGunShotEffect", RpcTarget.Others, originLocation, hitLocation, hitTag);
        }

        [PunRPC]
        void RpcDoGunShotEffect(Vector3 originLocation, Vector3 hitLocation, string hitTag)
        {
            DoGunShotEffect(originLocation, hitLocation, hitTag);
        }

        private void DoGunShotEffect(Vector3 originLocation, Vector3 hitLocation, string hitTag)
        {
            AudioSource.PlayClipAtPoint(revolverShotClip, originLocation);
        }

        #endregion

        #region OutOfAmmoEffect

        public void SyncOutOfAmmoEffect(Vector3 originLocation)
        {
            DoOutOfAmmoEffect(originLocation);
            photonView.RPC("RpcDoOutOfAmmoEffect", RpcTarget.Others, originLocation);
        }

        [PunRPC]
        void RpcDoOutOfAmmoEffect(Vector3 originLocation)
        {
            RpcDoOutOfAmmoEffect(originLocation);
        }

        private void DoOutOfAmmoEffect(Vector3 originLocation)
        {
            AudioSource.PlayClipAtPoint(outOfAmmoClip, originLocation);
        }

        #endregion
    }
}
