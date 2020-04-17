using UnityEngine;
using UnityEngine.VFX;

using Photon.Pun;

namespace Desperados.Game {
    public class EffectManager : MonoBehaviourPun
    {

        [SerializeField] AudioClip revolverShotClip;
        [SerializeField] AudioClip outOfAmmoClip;
        [SerializeField] AudioClip reloadClip;
        [SerializeField] AudioClip playerHitClip;
        [SerializeField] AudioClip[] ricochetClips;
        [SerializeField] AudioSource effectAudioSourcePrefab;

        [SerializeField] VisualEffect gunsmokeEffect;

        public static EffectManager Instance { get; private set; }

        AudioSource PlaySoundFromLocation(Vector3 location, AudioClip clip)
        {
            AudioSource audioSource = Instantiate(effectAudioSourcePrefab, location, Quaternion.identity);
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource.gameObject, audioSource.clip.length);
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
        public void SyncGunshotEffect(Vector3 originLocation, Quaternion gunRotation, Vector3 hitLocation, string hitTag)
        {
            DoGunShotEffect(originLocation, gunRotation, hitLocation, hitTag);
            photonView.RPC("RpcDoGunShotEffect", RpcTarget.Others, originLocation, gunRotation, hitLocation, hitTag);
        }

        [PunRPC]
        void RpcDoGunShotEffect(Vector3 originLocation, Quaternion gunRotation, Vector3 hitLocation, string hitTag)
        {
            DoGunShotEffect(originLocation, gunRotation, hitLocation, hitTag);
        }

        private void DoGunShotEffect(Vector3 originLocation, Quaternion gunRotation, Vector3 hitLocation, string hitTag)
        {
            PlaySoundFromLocation(originLocation, revolverShotClip);
            VisualEffect tmpGunsmoke = Instantiate(gunsmokeEffect, originLocation, gunRotation);
            Destroy(tmpGunsmoke.gameObject, 4.0f);
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
                    AudioClip ricochetClip = ricochetClips[Random.Range(0, ricochetClips.Length)];
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
