using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject _torch;

    [Header("Properties")]
    [SerializeField] float _speed = 3f;

    public event Action UseItemAction;
    public event Action SelectPreviousItem;
    public event Action SelectNextItem;

    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    Animator _animator;
    bool _canOpenDoor;
    bool _canMove = true;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update()
	{
		Vector2 desiredVelocity = _playerInput.actions["Move"].ReadValue<Vector2>() * _speed;
		if (_canMove)
		{
			_rigidBody.linearVelocity = desiredVelocity;
			HandleWalkingAnimation(desiredVelocity);
		}

		if (_playerInput.actions["UseItem"].triggered)
		{
			_canMove = false;
			_rigidBody.linearVelocity = Vector2.zero;

			if (desiredVelocity.x >= 0)
			{
				_animator.SetTrigger("DropItem");
			}
			else
			{
				_animator.SetTrigger("DropItemFlipped");
			}
			UseItemAction?.Invoke();
		}

		if (_playerInput.actions["Previous"].triggered)
			SelectPreviousItem?.Invoke();

		if (_playerInput.actions["Next"].triggered)
			SelectNextItem?.Invoke();
	}

	void OnTriggerEnter2D(Collider2D collider)
    {
        Key key = collider.GetComponent<Key>();
        if (key != null)
        {
            _canOpenDoor = true;
            Destroy(key.gameObject);
        }

        Door door = collider.GetComponent<Door>();
        if (door != null && _canOpenDoor)
        {
            SceneManager.LoadScene("Level 2");
            Debug.Log("OPEN DOOOOR");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Door door = collision.collider.GetComponent<Door>();
        if (door != null && _canOpenDoor)
        {
            Destroy(door.gameObject);
            Debug.Log("OPEN DOOOOR");
        }
    }

    public void UseItem(InventoryItem itemInSlot)
    {
        bool isDroppable = itemInSlot.item.type == ItemType.Droppable;
        if (isDroppable)
        {
            Vector2 dropPosition = new Vector2(_rigidBody.position.x, _rigidBody.position.y - 1);
            Instantiate(itemInSlot.item.gameObject, dropPosition, Quaternion.identity);
        }
    }

    void HandleWalkingAnimation(Vector2 desiredVelocity)
	{
		_animator.SetFloat("horizontal", desiredVelocity.x);
		_animator.SetFloat("vertical", desiredVelocity.y);
		_animator.SetFloat("speed", desiredVelocity.sqrMagnitude);
	}

    public void RestoreMovement()
	{
        _canMove = true;
	}
}
