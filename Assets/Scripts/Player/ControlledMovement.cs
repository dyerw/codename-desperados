﻿using UnityEngine;

using Photon.Pun;

public class ControlledMovement : MonoBehaviourPun
{

    [SerializeField] CharacterController characterController;
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpSpeed;

    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (characterController.isGrounded)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            moveDirection = transform.right * x + transform.forward * z;
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
        
    }
}
