using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace StarterAssets
{
    /// <summary>
    /// Inventory UI system. Displays items in a grid with quantity counters using prefab-based items.
    /// Uses a HashMap-style lookup for O(1) slot access and implements empty slot finding.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private GameObject inventorySlotPrefab;
        [SerializeField] private int maxSlots = 12;

        private Dictionary<string, InventorySlotUI> slotUIMap = new Dictionary<string, InventorySlotUI>();
        private List<InventorySlotUI> allSlots = new List<InventorySlotUI>();

        private void Start()
        {
            // Subscribe to inventory changes
            if (InventorySystem.Instance != null)
            {
                InventorySystem.Instance.OnInventoryChanged += UpdateInventoryDisplay;
            }

            // Initialize empty slots
            InitializeSlots();
        }

        private void OnDestroy()
        {
            // Unsubscribe from inventory changes
            if (InventorySystem.Instance != null)
            {
                InventorySystem.Instance.OnInventoryChanged -= UpdateInventoryDisplay;
            }
        }

        /// <summary>
        /// Create empty inventory slots.
        /// </summary>
        private void InitializeSlots()
        {
            if (inventorySlotPrefab == null)
            {
                Debug.LogError("Inventory Slot Prefab is not assigned! Please assign it in the Inspector.");
                return;
            }

            if (inventoryContainer == null)
            {
                Debug.LogError("Inventory Container is not assigned! Please assign it in the Inspector.");
                return;
            }

            for (int i = 0; i < maxSlots; i++)
            {
                GameObject slotGO = Instantiate(inventorySlotPrefab, inventoryContainer);
                InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
                if (slotUI != null)
                {
                    slotUI.Initialize();
                    allSlots.Add(slotUI);
                }
                else
                {
                    Debug.LogWarning("InventorySlotUI component not found on prefab!");
                }
            }

            Debug.Log($"Initialized {allSlots.Count} inventory slots.");
        }

        /// <summary>
        /// Find the first empty slot available.
        /// Returns null if no empty slots are available.
        /// </summary>
        private InventorySlotUI FindEmptySlot()
        {
            for (int i = 0; i < allSlots.Count; i++)
            {
                if (allSlots[i].IsEmpty())
                {
                    return allSlots[i];
                }
            }

            Debug.LogWarning($"No empty inventory slots available! Max slots: {allSlots.Count}");
            return null;
        }

        /// <summary>
        /// Find a slot containing a specific item by ID (HashMap-style O(1) lookup).
        /// </summary>
        private InventorySlotUI FindSlotByItemId(string itemId)
        {
            if (slotUIMap.ContainsKey(itemId))
            {
                return slotUIMap[itemId];
            }
            return null;
        }

        /// <summary>
        /// Update the inventory display based on InventorySystem data.
        /// Uses empty slot finding loop to fill items sequentially.
        /// </summary>
        private void UpdateInventoryDisplay()
        {
            // Safety check - make sure slots were initialized
            if (allSlots.Count == 0)
            {
                Debug.LogWarning("No inventory slots created! InitializeSlots may have failed.");
                return;
            }

            // Clear current display
            foreach (InventorySlotUI slot in allSlots)
            {
                slot.Clear();
            }
            slotUIMap.Clear();

            // Get items from inventory
            if (InventorySystem.Instance == null)
                return;

            Dictionary<string, InventoryItem> items = InventorySystem.Instance.GetAllItems();

            // Loop through inventory items and find empty slots for each
            foreach (var kvp in items)
            {
                InventoryItem item = kvp.Value;

                // Find an empty slot for this item
                InventorySlotUI emptySlot = FindEmptySlot();

                if (emptySlot == null)
                {
                    Debug.LogWarning($"Inventory full! Could not add item: {item.itemName}");
                    break;
                }

                // Set item in the empty slot
                emptySlot.SetItem(item.itemId, item.itemName, item.itemIcon, item.quantity);
                
                // Store in HashMap for O(1) lookup
                slotUIMap[item.itemId] = emptySlot;
            }

            Debug.Log($"Inventory display updated. Items displayed: {slotUIMap.Count}/{allSlots.Count}");
        }

        /// <summary>
        /// Force refresh the inventory display.
        /// </summary>
        public void RefreshDisplay()
        {
            UpdateInventoryDisplay();
        }
    }

    /// <summary>
    /// Individual inventory slot UI element with TextMeshPro quantity display.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMPro.TextMeshProUGUI quantityText; // TextMeshPro for better text rendering
        [SerializeField] private CanvasGroup canvasGroup;

        private string currentItemId;
        private bool isEmpty = true;

        public void Initialize()
        {
            Clear();
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Check if this slot is empty.
        /// </summary>
        public bool IsEmpty()
        {
            return isEmpty;
        }

        /// <summary>
        /// Set item in this slot with full data.
        /// </summary>
        public void SetItem(string itemId, string itemName, Sprite icon, int quantity)
        {
            currentItemId = itemId;
            isEmpty = false;

            if (itemImage != null)
            {
                itemImage.sprite = icon;
                itemImage.enabled = true;
            }

            if (quantityText != null)
            {
                // Show quantity only if more than 1
                quantityText.text = quantity > 1 ? quantity.ToString() : "";
                quantityText.enabled = quantity > 1;
            }

            if (canvasGroup != null)
                canvasGroup.alpha = 1f;

            Debug.Log($"Slot set with item: {itemName} (x{quantity})");
        }

        /// <summary>
        /// Clear this slot.
        /// </summary>
        public void Clear()
        {
            currentItemId = null;
            isEmpty = true;

            if (itemImage != null)
            {
                itemImage.sprite = null;
                itemImage.enabled = false;
            }

            if (quantityText != null)
            {
                quantityText.text = "";
                quantityText.enabled = false;
            }

            if (canvasGroup != null)
                canvasGroup.alpha = 0.5f;
        }

        /// <summary>
        /// Update just the quantity text (for stacking items).
        /// </summary>
        public void UpdateQuantity(int newQuantity)
        {
            if (quantityText != null)
            {
                quantityText.text = newQuantity > 1 ? newQuantity.ToString() : "";
                quantityText.enabled = newQuantity > 1;
            }
        }

        /// <summary>
        /// Get the current item ID in this slot.
        /// </summary>
        public string GetItemId()
        {
            return currentItemId;
        }
    }
}
