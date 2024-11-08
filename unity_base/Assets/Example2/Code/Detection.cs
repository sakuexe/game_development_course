using UnityEngine;

[RequireComponent(typeof(EnemyLogic))]
public class Detection : MonoBehaviour
{
    private EnemyLogic enemyLogic;
    private LayerMask playerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyLogic = GetComponent<EnemyLogic>();
        playerMask = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        // does the raycast hit the player
        float range = Mathf.Infinity;
        Debug.DrawRay(transform.position, transform.forward * 100, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, playerMask))
        {
            Debug.DrawRay(transform.position, hit.point, Color.red);
        }
    }
}
