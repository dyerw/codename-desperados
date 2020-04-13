using UnityEngine;

using Photon.Pun;

namespace Desperados.Game {
    public class EffectManager : MonoBehaviourPun
    {

        [SerializeField] AudioClip revolverShotClip;

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
    }
}
