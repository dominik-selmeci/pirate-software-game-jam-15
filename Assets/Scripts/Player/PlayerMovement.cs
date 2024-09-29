using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _baseSpeed = 3f;

    PlayerInput _playerInput;
    Animator _animator;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    public Vector2 GetDesiredVelocity(bool isRunning)
    {
        float speed = isRunning ? 6 : _baseSpeed;
        Vector2 move = _playerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 desiredVelocity = move * speed;
        HandleWalkingAnimation(desiredVelocity);

        return desiredVelocity;
    }

    void HandleWalkingAnimation(Vector2 desiredVelocity)
    {
        _animator.SetFloat("horizontal", desiredVelocity.x);
        _animator.SetFloat("vertical", desiredVelocity.y);
        _animator.SetFloat("speed", desiredVelocity.sqrMagnitude);
    }

    public void PlayDropAnimation(float linearVelocityX)
    {
        if (linearVelocityX >= 0)
            _animator.SetTrigger("DropItem");
        else
            _animator.SetTrigger("DropItemFlipped");
    }
}