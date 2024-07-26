using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Color selectedColor;
    [SerializeField] Color notSelectedColor;
    [SerializeField] Sprite baseImage;
    [SerializeField] Sprite mixImage;

    Image _image;

    public bool MarkedForCrafting = false;

    void Awake()
    {
        _image = GetComponent<Image>();
        Deselect();
    }

    public void Select()
    {
        _image.color = selectedColor;
    }

    public void Deselect()
    {
        _image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }

    public void toggleForCrafting()
    {
        MarkedForCrafting = !MarkedForCrafting;
        _image.sprite = MarkedForCrafting ? mixImage : baseImage;
    }
}
