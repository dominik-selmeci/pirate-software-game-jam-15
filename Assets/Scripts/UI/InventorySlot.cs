using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] Color selectedColor;
    [SerializeField] Color notSelectedColor;
    [SerializeField] Image baseImage;
    [SerializeField] Image mixImage;

    Image _image;

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
        Debug.Log("ON DROP" + (transform.childCount == 0).ToString());
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
