using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    PlayerInput playerInput;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 desiredMovement = playerInput.actions["Move"].ReadValue<Vector2>() * 8;
        rigidBody2D.linearVelocity = desiredMovement;
    }
}
