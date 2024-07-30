using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject _torch;
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _shield;
    [SerializeField] GameObject _damagePanel;

    [Header("Properties")]
    [SerializeField] float _baseSpeed = 3f;
    [SerializeField] int _maxHealth = 100;
    int _currentHealth;
    float _speed = 3f;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _floatingText;
    [SerializeField] Animator _floatingTextAnimator;
    [SerializeField] HealthBar _healthBar;

    [Header("Audio")]
    [SerializeField] AudioClip _drinkPotionSFX;
    [SerializeField] AudioClip _placeItemSFX;
    [SerializeField] AudioClip _collectMaterialSFX;

    [Header("Animators")]
    [SerializeField] Animator damageAnimator;


    public event Action UseItemAction;
    public event Action SelectPreviousItem;
    public event Action SelectNextItem;
    public event Action CollectMaterialAction;

    [HideInInspector] public CollectableItem _itemThatCanBeCollected;

    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    Animator _animator;
    AudioSource _audioSource;

    public bool _canMove = true;
    public bool _shieldActive = false;
    DialogManager _dialogManager;

    void Start()
    {
        _speed = _baseSpeed;
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        if(_damagePanel != null) 
            damageAnimator = _damagePanel.GetComponent<Animator>();
        _dialogManager = FindFirstObjectByType<DialogManager>();
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
            if (_dialogManager.isOpen)
            {
                _dialogManager.DisplayNextSentence();
                return;
            }

            UseItemAction?.Invoke();
        }

        if (_playerInput.actions["Previous"].triggered)
            SelectPreviousItem?.Invoke();

        if (_playerInput.actions["Next"].triggered)
            SelectNextItem?.Invoke();

        if (_playerInput.actions["Interact"].WasPressedThisFrame() && _itemThatCanBeCollected != null)
            _audioSource.PlayOneShot(_collectMaterialSFX);

        if (_playerInput.actions["Interact"].triggered)
            CollectMaterial();

        if (_playerInput.actions["Sprint"].IsPressed())
            _speed = 2 * _baseSpeed;
        else _speed = _baseSpeed;
    }

    void CollectMaterial()
    {
        if (_itemThatCanBeCollected == null) return;

        _floatingTextAnimator.SetTrigger("Reset");
        CollectMaterialAction?.Invoke();
        Destroy(_itemThatCanBeCollected.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        CollectableItem collectableItem = collider.GetComponent<CollectableItem>();
        if (collectableItem != null)
        {
            _floatingText.text = "Hold [E] to collect \n" + collectableItem.item.itemName;
            _floatingTextAnimator.SetBool("IsVisible", true);
            _itemThatCanBeCollected = collectableItem;
        }

        if (collider.CompareTag("DamageZone"))
            InvokeRepeating(nameof(ReceiveDamage), 0, 0.33f);

        if (collider.CompareTag("HealthZone"))
            InvokeRepeating(nameof(Heal), 0, 0.33f);

        if (collider.CompareTag("Enemy"))
            TakeDamage(3);

    }

    void ReceiveDamage()
    {
        TakeDamage(2);
    }

    void Heal()
    {
        RestoreHealth(25);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        CollectableItem collectableItem = collider.GetComponent<CollectableItem>();
        if (collectableItem != null && collectableItem == _itemThatCanBeCollected)
        {
            _floatingTextAnimator.SetBool("IsVisible", false);
            _itemThatCanBeCollected = null;
        }

        if (collider.CompareTag("DamageZone"))
            CancelInvoke(nameof(ReceiveDamage));

        if (collider.CompareTag("HealthZone"))
            CancelInvoke(nameof(Heal));
    }

    public void UseItem(InventoryItem itemInSlot)
    {
        bool isDroppable = itemInSlot.item.type == ItemType.Droppable;
        bool isConsumable = itemInSlot.item.type == ItemType.Consumable;
        if (isDroppable)
        {
            _audioSource.PlayOneShot(_placeItemSFX);
            Vector2 dropPosition = new Vector2(_rigidBody.position.x, _rigidBody.position.y - 1);
            Instantiate(itemInSlot.item.gameObject, dropPosition, Quaternion.identity);
            PlayDropAnimation();
        }
        else if (isConsumable)
        {
            Debug.Log(itemInSlot.item.actionType);
            _audioSource.PlayOneShot(_drinkPotionSFX);
            if (itemInSlot.item.actionType.ToString() == "Cure")
            {
                Heal();
            }
            else if (itemInSlot.item.actionType.ToString() == "Protect")
            {
                StartCoroutine(ActiveShield());
            }
        }

    }

    IEnumerator ActiveShield()
    {
        _shield.SetActive(true);
        _shieldActive = true;
        yield return new WaitForSeconds(15);
        _shieldActive = false;
        _shield.SetActive(false);
    }

    void PlayDropAnimation()
    {
        _canMove = false;
        float linearVelocityX = _rigidBody.linearVelocity.x;
        _rigidBody.linearVelocity = Vector2.zero;

        if (linearVelocityX >= 0)
            _animator.SetTrigger("DropItem");
        else
            _animator.SetTrigger("DropItemFlipped");
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

    public void TakeDamage(int _damage)
    {
        if (!_shieldActive)
        {
            if (_currentHealth <= 0)
            {
                Die();
            }

            if(_damagePanel != null)
			{
               
                _damagePanel.SetActive(true);
                damageAnimator.SetTrigger("Play");
               
			}
            _currentHealth -= _damage;
            _healthBar.SetHealth(_currentHealth);
        }
    }

    private void RestoreHealth(int _health)
    {
        if (_currentHealth >= _maxHealth) return;

        _currentHealth += _health;
        _healthBar.SetHealth(_currentHealth);
    }

    private void Die()
    {
        Instantiate(_enemy.gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject.GetComponent<SpriteRenderer>());
        Destroy(this);
        GameObject.FindFirstObjectByType<GameOver>().gameOver();
    }
}
