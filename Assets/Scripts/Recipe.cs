using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Object/Recipe")]
public class Recipe : ScriptableObject
{
  [SerializeField] public List<Item> items;
  [SerializeField] public Item craftedItem;
}
