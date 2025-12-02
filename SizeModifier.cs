using UnityEngine;

namespace StarterAssets
{
    /// <summary>
    /// Handles player size modification mechanics (shrinking and size increase).
    /// Scales both visual and collision components.
    /// </summary>
    public class SizeModifier : MonoBehaviour
    {
        [Header("Size Modification")]
        [SerializeField] private float minScale = 0.5f;
        [SerializeField] private float maxScale = 2f;
        [SerializeField] private float normalScale = 1f;
        [SerializeField] private float scaleChangeRate = 1f;

        [Header("Input Settings")]
        [SerializeField] private KeyCode shrinkKey = KeyCode.Q;
        [SerializeField] private KeyCode growKey = KeyCode.E;

        private float currentScale;
        private CharacterController characterController;
        private Vector3 originalControllerCenter;
        private float originalControllerHeight;
        private float originalControllerRadius;

        private void Start()
        {
            currentScale = normalScale;

            // Auto-find CharacterController on this GameObject
            characterController = GetComponent<CharacterController>();
            
            if (characterController != null)
            {
                originalControllerHeight = characterController.height;
                originalControllerRadius = characterController.radius;
                originalControllerCenter = characterController.center;
                Debug.Log("CharacterController found and stored!");
            }
            else
            {
                Debug.LogWarning("CharacterController not found on Player! Size modification will only affect visuals.");
            }
        }

        private void Update()
        {
            HandleSizeInput();
        }

        private void HandleSizeInput()
        {
            float targetScale = currentScale;

            // Shrink: Hold Q to decrease size
            if (Input.GetKey(shrinkKey))
            {
                targetScale -= scaleChangeRate * Time.deltaTime;
            }

            // Grow: Hold E to increase size
            if (Input.GetKey(growKey))
            {
                targetScale += scaleChangeRate * Time.deltaTime;
            }

            // Clamp scale between min and max
            targetScale = Mathf.Clamp(targetScale, minScale, maxScale);

            // Apply size change if different
            if (Mathf.Abs(targetScale - currentScale) > 0.01f)
            {
                SetPlayerScale(targetScale);
            }
        }

        /// <summary>
        /// Set the player's scale (visual and collision).
        /// </summary>
        private void SetPlayerScale(float newScale)
        {
            currentScale = newScale;

            // Scale the player visually
            transform.localScale = new Vector3(newScale, newScale, newScale);

            // Scale the CharacterController collision
            if (characterController != null)
            {
                characterController.enabled = false;
                characterController.height = originalControllerHeight * currentScale;
                characterController.radius = originalControllerRadius * currentScale;
                characterController.center = originalControllerCenter * currentScale;
                characterController.enabled = true;
            }

            Debug.Log($"Player scaled to: {newScale:F2}");
        }

        /// <summary>
        /// Get current player scale.
        /// </summary>
        public float GetCurrentScale()
        {
            return currentScale;
        }

        /// <summary>
        /// Reset to normal scale.
        /// </summary>
        public void ResetScale()
        {
            SetPlayerScale(normalScale);
        }
    }
}
