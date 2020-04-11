using UnityEngine;
using Photon.Pun;

public class AudioController : MonoBehaviourPun
{
    [SerializeField]
    AudioClip revolverSound;

    [SerializeField]
    AudioClip hitSound;

    [PunRPC]
    void PlayGunshot()
    {
        AudioSource.PlayClipAtPoint(revolverSound, transform.position);
    }

    [PunRPC]
    void PlayHitSound()
    {
        AudioSource.PlayClipAtPoint(hitSound, transform.position);
    }
}
