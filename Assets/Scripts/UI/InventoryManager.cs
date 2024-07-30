using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] InventorySlot[] inventorySlots;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Player player;
    [SerializeField] Button combineButton;
    [SerializeField] int maxStackedItems = 64;

    [Header("Starting Items")]
    [SerializeField] List<Item> startingItems;

    [Header("Recipes")]
    [SerializeField] List<Recipe> recipes;

    [Header("Dialog")]
    [SerializeField] Dialog dialog;
    [SerializeField] DialogTrigger dialogTrigger;

    int selectedSlot = -1;
    InventoryMode mode = InventoryMode.Normal;

    void Start()
    {
        startingItems.ForEach(item => AddItem(item));

        ChangeSelectedSlot(0);
        player.UseItemAction += UseItem;
        player.SelectPreviousItem += MoveSelectedSlotToTheLeft;
        player.SelectNextItem += MoveSelectedSlotToTheRight;
        player.CollectMaterialAction += CollectMaterial;
        dialogTrigger._dialog = dialog;
    }

    void CollectMaterial()
    {
        if (player._itemThatCanBeCollected == null) return;

        if (player._itemThatCanBeCollected.item != null)
        {
            Item item = player._itemThatCanBeCollected.item;
            for (int i = 0; i < item.quantity; i++)
            {
                AddItem(player._itemThatCanBeCollected.item);
            }
        }


        List<Item> randomItems = player._itemThatCanBeCollected.randomItems;
        if (randomItems.Count > 0)
            AddItem(randomItems[Random.Range(0, randomItems.Count)]);
    }

    void MoveSelectedSlotToTheRight()
    {
        MoveSelectedSlot(1);
    }

    void MoveSelectedSlotToTheLeft()
    {
        MoveSelectedSlot(-1);
    }

    void Update()
    {
        if (Math.Abs(Input.mouseScrollDelta.y) == 1)
        {
            int increment = (int)Input.mouseScrollDelta.y;
            MoveSelectedSlot(-increment);
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

    void MoveSelectedSlot(int increment)
    {
        int newSelectedSlot = selectedSlot + increment;
        if (newSelectedSlot > 9) newSelectedSlot = 0;
        if (newSelectedSlot < 0) newSelectedSlot = 9;

        ChangeSelectedSlot(newSelectedSlot);
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
        // check if any slot has the same item
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (
                itemInSlot != null
                && itemInSlot.item == item
                && itemInSlot.count < maxStackedItems
                && itemInSlot.item.stackable == true
                )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return;
            }
        }

        // find any empty slot
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

    public void CraftItems()
    {
        if (mode == InventoryMode.Normal) return;

        // get selected items
        List<Item> selectedItems = new List<Item>();
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.MarkedForCrafting)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                selectedItems.Add(itemInSlot.item);
            }
        }

        // find corresponding recipe
        Recipe choseRecipe = null;
        recipes.ForEach(recipe =>
        {
            if (recipe.items.Count != selectedItems.Count) return;

            bool hasAllItems = true;
            recipe.items.ForEach(item =>
            {
                if (!selectedItems.Contains(item)) hasAllItems = false;
            });

            if (hasAllItems) choseRecipe = recipe;
        });

        if (choseRecipe == null)
        {
            dialogTrigger.TriggerDialogue();
            StartCoroutine(CloseDialog());
            return;
        }

        // remove count of everyone from selected items
        bool shouldUnselectAll = false;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.MarkedForCrafting)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot.count == 1)
                    shouldUnselectAll = true;

                lowerCountInSlot(itemInSlot);
            }
        }

        if (shouldUnselectAll)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].MarkedForCrafting)
                    inventorySlots[i].toggleForCrafting();
            }

            combineButton.interactable = false;
        }

        AddItem(choseRecipe.craftedItem);
        Debug.Log("You successfully crafted " + choseRecipe.craftedItem.name);
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

        // select for crafting
        bool isMaterial = itemInSlot.item.type == ItemType.Material;
        if (isMaterial)
            inventorySlots[selectedSlot].toggleForCrafting();

        SetInventoryMode();

        // drop or consume
        player.UseItem(itemInSlot);

        if (!isMaterial)
        {
            lowerCountInSlot(itemInSlot);
        }
    }

    void lowerCountInSlot(InventoryItem inventoryItem)
    {
        if (inventoryItem.count == 1)
            Destroy(inventoryItem.gameObject);
        else
        {
            inventoryItem.count--;
            inventoryItem.RefreshCount();
        }
    }

    void SetInventoryMode()
    {
        bool isCraftingMode = false;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            if (slot.MarkedForCrafting) isCraftingMode = true;
        }

        mode = isCraftingMode ? InventoryMode.Crafting : InventoryMode.Normal;
        combineButton.interactable = isCraftingMode;
    }

    IEnumerator CloseDialog()
    {
        yield return new WaitForSeconds(1);
        dialogTrigger.TriggerEndDialog();
    }
}

public enum InventoryMode
{
    Normal,
    Crafting
}