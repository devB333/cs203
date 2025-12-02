using UnityEngine;
using StarterAssets;

public class coinCollect : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private string itemId = "coin_gold_001";
    [SerializeField] private string itemName = "Gold Coin";
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private int quantity = 1;

    private SphereCollider sphereCollider;
    private bool isCollected = false;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if colliding with player and not already collected
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Add to inventory using HashMap system
            if (InventorySystem.Instance != null)
            {
                InventorySystem.Instance.AddItem(itemId, itemName, itemIcon, quantity);
                Debug.Log($"Collected: {itemName}");
            }
            else
            {
                Debug.LogWarning("InventorySystem not found!");
            }

            // Destroy the coin
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
