using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    private GameInputActions _inputActions;
    private Vector2 _moveDirection;
    private Player _player;

    private void Awake()
    {
        _inputActions = new GameInputActions();
        _player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (_player.playerState == Player.PlayerState.Prepare)
        {
            _moveDirection = _inputActions.Player.Move.ReadValue<Vector2>();
            Move();
        }
        
    }

    private void OnEnable()
    {
        _inputActions.Player.Move.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    void Move()
    {
        transform.Translate(transform.forward * Time.deltaTime * playerSpeed * _moveDirection.y);
    }
}
