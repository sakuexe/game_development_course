using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToTarget))]
public class ListenForMouseInput : MonoBehaviour
{

    private MoveToTarget moveComponent;
    public float movementSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        moveComponent = GetComponent<MoveToTarget>();
        moveComponent.SetSpeed(movementSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // only handle mouse left click
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // if raycast did not a valid object, return
        if (!Physics.Raycast(ray, out hit))
            return;

        // Debug.Log("Hit: " + hit.point);
        moveComponent.MoveTo(hit.point);
    }
}
