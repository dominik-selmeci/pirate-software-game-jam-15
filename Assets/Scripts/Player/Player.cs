using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _shield;
    [SerializeField] DamagePostProcessing damagePostProcessing;

    [Header("Properties")]
    [SerializeField] int _maxHealth = 120;
    int _currentHealth;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _floatingText;
    [SerializeField] Animator _floatingTextAnimator;
    [SerializeField] HealthBar _healthBar;

    public event Action UseItemAction;
    public event Action SelectPreviousItem;
    public event Action SelectNextItem;
    public event Action CollectMaterialAction;

    [HideInInspector] public CollectableItem _itemThatCanBeCollected;
    [HideInInspector] public DialogTrigger _dialogTrigger;

    Rigidbody2D _rigidBody;
    PlayerInput _playerInput;
    PlayerAudio _playerAudio;
    PlayerMovement _playerMovement;
    public bool _canMove = true;
    public bool _shieldActive = false;
    DialogManager _dialogManager;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAudio = GetComponent<PlayerAudio>();
        _playerMovement = GetComponent<PlayerMovement>();
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        _dialogManager = FindFirstObjectByType<DialogManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            bool isRunning = _playerInput.actions["Sprint"].IsPressed();
            _rigidBody.linearVelocity = _playerMovement.GetDesiredVelocity(isRunning);
        }

        if (_playerInput.actions["UseItem"].triggered && !_dialogManager.isOpen)
        {
            UseItemAction?.Invoke();
        }

        if (_playerInput.actions["Previous"].triggered)
            SelectPreviousItem?.Invoke();

        if (_playerInput.actions["Next"].triggered)
            SelectNextItem?.Invoke();

        if (_playerInput.actions["Interact"].WasPressedThisFrame() && _dialogManager.isOpen)
        {
            _dialogManager.DisplayNextSentence();
            return;
        }

        if (_playerInput.actions["Interact"].WasPressedThisFrame() && !_dialogManager.isOpen && _dialogTrigger != null)
            _dialogTrigger.TriggerDialogue();

        if (_playerInput.actions["Interact"].triggered && !_dialogManager.isOpen)
        {
            CollectMaterial();
            _playerAudio.PlayCollectSound();
        }
    }

    public void RestoreMovement()
    {
        _canMove = true;
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
            _floatingText.text = "Hold [Space] to collect \n" + collectableItem.item.itemName;
            _floatingTextAnimator.SetBool("IsVisible", true);
            _itemThatCanBeCollected = collectableItem;
        }

        DialogTrigger dialogTrigger = collider.GetComponent<DialogTrigger>();
        if (dialogTrigger != null)
        {
            _floatingText.text = "Press [Space] to read";
            _floatingTextAnimator.SetBool("IsVisible", true);
            _dialogTrigger = dialogTrigger;
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

        DialogTrigger dialogTrigger = collider.GetComponent<DialogTrigger>();
        if (dialogTrigger != null)
        {
            _floatingTextAnimator.SetBool("IsVisible", false);
            _dialogTrigger = null;
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
            _playerAudio.PlayPlaceItemSound();
            Vector2 dropPosition = new Vector2(_rigidBody.position.x, _rigidBody.position.y - 1);
            Instantiate(itemInSlot.item.gameObject, dropPosition, Quaternion.identity);
            _canMove = false;
            _playerMovement.PlayDropAnimation(_rigidBody.linearVelocity.x);
            _rigidBody.linearVelocity = Vector2.zero;
        }
        else if (isConsumable)
        {
            Debug.Log(itemInSlot.item.actionType);
            _playerAudio.PlayDrinkSound();
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

    public void TakeDamage(int _damage)
    {
        if (!_shieldActive)
        {
            if (_currentHealth <= 0)
            {
                Die();
            }

            if (damagePostProcessing != null)
            {
                damagePostProcessing.PlayDamageEffect();

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
