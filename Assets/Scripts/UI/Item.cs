using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    [Header("Only gameplay")]
    public string itemName;
    public TileBase tile;
    public GameObject gameObject;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);
    public int quantity = 1;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
}

public enum ItemType
{
    Material,
    Tool,
    Droppable,
    Consumable
}

public enum ActionType
{
    Mine,
    Cure,
    Protect
}