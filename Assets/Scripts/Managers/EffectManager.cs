using UnityEngine;

using Photon.Pun;

namespace Desperados.Game {
    public class EffectManager : MonoBehaviourPun
    {

        [SerializeField] AudioClip revolverShotClip;
        [SerializeField] AudioClip outOfAmmoClip;
        [SerializeField] AudioClip reloadClip;
        [SerializeField] AudioClip ricochetClip;
        [SerializeField] AudioClip playerHitClip;

        public static EffectManager Instance { get; private set; }

        AudioSource PlaySoundFromLocation(Vector3 location, AudioClip clip)
        {
            GameObject tmpGO = new GameObject("One shot audio");
            tmpGO.transform.position = location;
            AudioSource audioSource = tmpGO.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(tmpGO, audioSource.clip.length);
            return audioSource;
        }

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
            PlaySoundFromLocation(originLocation, reloadClip);
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
            PlaySoundFromLocation(originLocation, revolverShotClip);
        }

        #endregion

        #region GunHitEffect

        public void SyncGunHitEffect(Vector3 position, Shootable.ShootableType shootableType)
        {
            DoGunHitEffect(position, shootableType);
            photonView.RPC("RpcDoGunHitEffect", RpcTarget.Others, position, shootableType);
        }

        [PunRPC]
        void RpcDoGunHitEffect(Vector3 position, Shootable.ShootableType shootableType) 
        {
            DoGunHitEffect(position, shootableType);
        }

        void DoGunHitEffect(Vector3 position, Shootable.ShootableType shootableType)
        {
            AudioClip hitClip = null;
            switch (shootableType) 
            {
                case Shootable.ShootableType.Terrain:
                    hitClip = ricochetClip;
                    break;
                case Shootable.ShootableType.Player:
                    hitClip = playerHitClip;
                    break;
            }
            PlaySoundFromLocation(position, hitClip);
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
            PlaySoundFromLocation(originLocation, outOfAmmoClip);
        }

        #endregion
    }
}
