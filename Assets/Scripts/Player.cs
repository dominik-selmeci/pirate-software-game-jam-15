using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject _torch;

    [Header("Properties")]
    [SerializeField] float _speed = 3f;

    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    bool _canOpenDoor;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 desiredVelocity = _playerInput.actions["Move"].ReadValue<Vector2>() * _speed;
        _rigidBody.linearVelocity = desiredVelocity;

        if (_playerInput.actions["Attack"].triggered)
        {
            Instantiate(_torch, _rigidBody.position, Quaternion.identity);
        }
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
}
