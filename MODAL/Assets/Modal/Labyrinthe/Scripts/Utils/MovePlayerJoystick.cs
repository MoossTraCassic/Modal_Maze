using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ModalFunctions.Controller
{
    public class MovePlayerJoystick : MonoBehaviour
    {
   
        PlayerControl controls;
        Vector2 move;

        private void Awake()
        {
            controls = new PlayerControl();

            controls.MovePlayer.MovePlayer.performed += ctx => move = ctx.ReadValue<Vector2>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Debug.Log(move.x);
        }

        private void OnEnable()
        {
            controls.MovePlayer.Enable();
        }

        private void OnDisable()
        {
            controls.MovePlayer.Disable();
        }

    }
}