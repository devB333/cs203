using UnityEngine;
using System.Collections.Generic;

namespace StarterAssets
{
    /// <summary>
    /// Inventory system using a HashMap (Dictionary) data structure.
    /// Supports stacking items of the same type.
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        // HashMap: Key = Item ID, Value = Item data with quantity
        private Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

        public static InventorySystem Instance { get; private set; }

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Add an item to the inventory. If item exists, increment quantity.
        /// </summary>

public void AddItem(string itemId, string itemName, Sprite itemIcon, int quantity = 1)
{
    // Validate inputs
    if (string.IsNullOrEmpty(itemId))
    {
        Debug.LogError("Item ID is null or empty. Cannot add item to inventory.");
        return;
    }

    if (quantity <= 0)
    {
        Debug.LogError($"Invalid quantity ({quantity}) for item {itemName}. Cannot add item to inventory.");
        return;
    }

    if (inventory.ContainsKey(itemId))
    {
        // Item already exists, increase quantity
        inventory[itemId].quantity += quantity;
        Debug.Log($"Added {quantity} more {itemName}. Total: {inventory[itemId].quantity}");
    }
    else
    {
        // New item, add to inventory
        inventory[itemId] = new InventoryItem
        {
            itemId = itemId,
            itemName = itemName,
            itemIcon = itemIcon,
            quantity = quantity
        };
        Debug.Log($"Added {itemName} to inventory. Quantity: {quantity}");
    }

    // Notify listeners about inventory change
    OnInventoryChanged?.Invoke();
}

        /// <summary>
        /// Remove an item from inventory. Returns true if successful.
        /// </summary>
        public bool RemoveItem(string itemId, int quantity = 1)
        {
            if (!inventory.ContainsKey(itemId))
                return false;

            inventory[itemId].quantity -= quantity;

            if (inventory[itemId].quantity <= 0)
            {
                inventory.Remove(itemId);
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Get an item from inventory by ID.
        /// </summary>
        public InventoryItem GetItem(string itemId)
        {
            return inventory.ContainsKey(itemId) ? inventory[itemId] : null;
        }

        /// <summary>
        /// Get the quantity of an item.
        /// </summary>
        public int GetItemQuantity(string itemId)
        {
            return inventory.ContainsKey(itemId) ? inventory[itemId].quantity : 0;
        }

        /// <summary>
        /// Get all items in inventory.
        /// </summary>
        public Dictionary<string, InventoryItem> GetAllItems()
        {
            return new Dictionary<string, InventoryItem>(inventory);
        }

        /// <summary>
        /// Clear the entire inventory.
        /// </summary>
        public void ClearInventory()
        {
            inventory.Clear();
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Get inventory count.
        /// </summary>
        public int GetInventoryCount()
        {
            return inventory.Count;
        }

        // Event to notify when inventory changes
        public delegate void InventoryChangedDelegate();
        public event InventoryChangedDelegate OnInventoryChanged;
    }

    /// <summary>
    /// Data structure for inventory items.
    /// </summary>
    [System.Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string itemName;
        public Sprite itemIcon;
        public int quantity;
    }
}
