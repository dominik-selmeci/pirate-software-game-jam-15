using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Player player;

    [Header("Testing Items")]
    [SerializeField] Item item1;
    [SerializeField] Item item2;


    int selectedSlot = -1;

    void Start()
    {
        AddItem(item1);
        AddItem(item1);
        AddItem(item2);
        AddItem(item1);
        ChangeSelectedSlot(0);
        player.UseItemAction += UseItem;
    }

    void Update()
    {
        if (Math.Abs(Input.mouseScrollDelta.y) == 1)
        {
            int increment = (int)Input.mouseScrollDelta.y;
            int newSelectedSlot = selectedSlot + increment;
            if (newSelectedSlot > 9) newSelectedSlot = 0;
            if (newSelectedSlot < 0) newSelectedSlot = 9;

            ChangeSelectedSlot(newSelectedSlot);
        }

        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number >= 0 && number <= 9)
            {
                ChangeSelectedSlot(number == 0 ? 9 : number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int slotIndex)
    {
        if (selectedSlot != -1)
            inventorySlots[selectedSlot].Deselect();

        inventorySlots[slotIndex].Select();
        selectedSlot = slotIndex;
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGameObject = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGameObject.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    void UseItem()
    {
        InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null) return;

        player.UseItem(itemInSlot);
        Destroy(itemInSlot.gameObject);
    }
}
