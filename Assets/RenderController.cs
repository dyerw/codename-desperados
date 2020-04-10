using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Networking;


public class RenderController : NetworkBehaviour
{
    [SerializeField] GameObject firstPersonView;
    [SerializeField] GameObject thirdPersonView;

    Renderer[] thirdPersonRenderers;
    Renderer[] firstPersonRenderers;

    void Awake()
    {
        thirdPersonRenderers = thirdPersonView.GetComponentsInChildren<Renderer>();
        firstPersonRenderers = firstPersonView.GetComponentsInChildren<Renderer>();
    }

    public void HideFirstPerson()
    {
        foreach (Renderer renderer in firstPersonRenderers)
        {
            renderer.enabled = false;
        }
    }
    public void ShowFirstPerson()
    {
        foreach (Renderer renderer in firstPersonRenderers)
        {
            renderer.enabled = true;
        }
    }

    public void HideThirdPerson()
    {
        foreach(Renderer renderer in thirdPersonRenderers)
        {
            renderer.enabled = false;
        }
    }
    public void ShowThirdPerson()
    {
        foreach(Renderer renderer in thirdPersonRenderers)
        {
            renderer.enabled = true;
        }
    }

    public void ThirdPersonShadowsOnly()
    {
        foreach (Renderer renderer in thirdPersonRenderers)
        {
            renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

    }

    public void DisableFirstPersonShadows()
    {
        foreach (Renderer renderer in firstPersonRenderers)
        {
            renderer.shadowCastingMode = ShadowCastingMode.Off;
        }
    }
}
