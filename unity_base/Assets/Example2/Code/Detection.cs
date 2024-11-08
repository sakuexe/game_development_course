using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(EnemyLogic))]
public class Detection : MonoBehaviour
{
    [SerializeField]
    private Light flashlight;
    [SerializeField]
    private float coneAngle = 45f;
    [SerializeField]
    private int numberOfRays = 10;
    [SerializeField]
    private float flashlightRange = 10f;

    private EnemyLogic enemyLogic;
    private LayerMask playerMask;
    private float idleFlashlightTemperature;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        playerMask = LayerMask.GetMask("Player");
        Assert.IsNotNull(flashlight);
    }

    private void FixedUpdate()
    {
        DetectPlayer();

        // color the flashlight based on the enemy state
        if (enemyLogic.GetState() == EnemyState.Chasing)
        {
            flashlight.colorTemperature = 1500;
        }
        else
            flashlight.colorTemperature = 5600;
    }

    private void DetectPlayer()
    {
        Vector3 direction = transform.forward;

        // count the steps for the angles of the rays
        float angleStep = coneAngle / (numberOfRays - 1);

        for (int index = 0; index < numberOfRays; index++)
        {
            float angle = -coneAngle / 2 + index * angleStep;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rayDirection = rotation * direction;

            // if the ray did not find a player
            ShootRay(rayDirection);
        }
    }

    // shoot a ray to a direction and return true if it hit
    private void ShootRay(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, flashlightRange, playerMask))
        {
            // get the player, and count the distance to it
            GameObject player = hit.transform.gameObject;
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            // draw the ray red 
            Debug.DrawRay(transform.position, direction * distanceToPlayer, Color.red);
            // make the enemy aggro
            enemyLogic.SetState(EnemyState.Chasing);
            return;
        }

        // no player in ray, draw it red
        Debug.DrawRay(transform.position, direction * flashlightRange, Color.yellow);
    }
}
