using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("Drops exact Item")]
    [SerializeField] public Item item;

    [Header("Drops random Item from the list")]
    [SerializeField] public List<Item> randomItems;
}
