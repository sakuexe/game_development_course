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
    private string playerTag;
    private float idleFlashlightTemperature;

    private void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        // make sure that a visual indication for the cone of detection is there
        Assert.IsNotNull(flashlight);
    }

    // no need to check on every frame
    private void FixedUpdate()
    {
        KeepEyeForPlayer();

        // color the flashlight based on the enemy state
        if (enemyLogic.GetState() == EnemyState.Chasing)
            flashlight.colorTemperature = 1500;  // anger!!!
        else
            flashlight.colorTemperature = 5600; // peace...
    }

    private void KeepEyeForPlayer()
    {
        Vector3 direction = transform.forward;

        // count the steps to make a triangle of n amount of rays
        float angleStep = coneAngle / (numberOfRays - 1);

        for (int index = 0; index < numberOfRays; index++)
        {
            float angle = -coneAngle / 2 + index * angleStep;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rayDirection = rotation * direction;

            ShootRay(rayDirection);
        }
    }

    // shoot a ray to a direction and check for player detection
    private void ShootRay(Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, flashlightRange))
        {
            // if the ray hits something that is not a player
            if (!hit.transform.gameObject.CompareTag("Player"))
            {
                Vector3 objectHitDirection = direction * hit.distance;
                Debug.DrawRay(transform.position, objectHitDirection, Color.yellow);
                return;
            }

            // if the ray hits a player
            Vector3 hitDirection = direction * hit.distance;
            // draw the ray red 
            Debug.DrawRay(transform.position, hitDirection, Color.red);
            // make the enemy aggro
            enemyLogic.SetState(EnemyState.Chasing);
            return;
        }

        // if the ray hits nothing
        Debug.DrawRay(transform.position, direction * flashlightRange, Color.yellow);
    }
}
