using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour
{
    public Transform player; // Reference to the player to follow
    public float rotationSpeed = 3f; // Speed of rotation adjustment
    public float maxRotationAdjustment = 1f; // Maximum degrees of adjustment per frame

    private void Start()
    {
        // Find the player tagged object if not assigned
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player tagged object not found.");
            }
        }
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            SmoothLookAtPlayer();
        }
    }

    private void SmoothLookAtPlayer()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Determine the target rotation to look at the player, only adjusting the horizontal and vertical axes
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            maxRotationAdjustment * rotationSpeed * Time.deltaTime
        );
    }
}
