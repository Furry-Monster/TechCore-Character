using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace MonsterTP
{
    public class ThirdPersonController : MonoBehaviour
    {
        private ThirdPersonActions _inputActions;

        [SerializeField] private Rigidbody rb;

        private void Awake()
        {
            _inputActions = new ThirdPersonActions();
            rb ??= GetComponent<Rigidbody>();

            _inputActions.Player.Jump.performed += ctx => Jump();
            _inputActions.Enable();
        }

        private void Update()
        {
            var moveInput = _inputActions.Player.Move.ReadValue<Vector2>();
            var direction = new Vector3(moveInput.x, 0, moveInput.y) * (5.0f * Time.deltaTime);
            transform.Translate(direction, Space.World);
        }

        private void Jump()
        {
            rb.AddForce(transform.up * 5.0f, ForceMode.Impulse);
        }
    }
}