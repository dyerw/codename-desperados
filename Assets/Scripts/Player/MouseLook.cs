using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviourPun
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform playerBody;
    [SerializeField] PhotonView _photonView;

    float xRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
