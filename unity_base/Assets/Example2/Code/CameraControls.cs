using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float sensitivity = 8;
    [SerializeField]
    private Vector3 cameraOffset = new (10, 5, 0);

    // states
    private Vector3 lastMousePosition;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player != null) return;
        player = GameObject.FindWithTag("Player"); // get the normal platypus
        lastPlayerPosition = player.transform.position;
    }

    void Update()
    {
        FollowPlayer();

        // roblox camera
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            // Vector3 delta = Input.mousePosition - lastMousePosition;
            // MoveCamera(delta.x, delta.y);
            RotateCamera(disableY: true);  // dont rotate camera up and down
            lastMousePosition = Input.mousePosition;
        }
    }

    // keep up with the player, since the camera is not child of the player
    // if it was the child, it would rotate with the player and that was painful
    private void FollowPlayer()
    {
        Vector3 distanceDifference = player.transform.position - lastPlayerPosition;
        transform.position += distanceDifference;
        lastPlayerPosition = player.transform.position;
        UpdateCameraPosition();
    }

    // This is a free movement camera, I did not end up using this
    // I did also not come up with whatever the fuck is happening here
    // so I take no responsibility of any eye, brain or soul strain caused
    // by excessive exposures to math
    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
    }

    // this rotates the camera around the player based on the mouse movement
    // you can disable axises, so that the angle stays the same
    private void RotateCamera(bool disableX = false, bool disableY = false)
    {
        if (!disableX)
        {
            float moveHorizontal = Input.GetAxis("Mouse X");
            transform.RotateAround(player.transform.position, -Vector3.up, moveHorizontal * sensitivity); 
        }
        if (!disableY)
        {
            float moveVertical = Input.GetAxis("Mouse Y");
            transform.RotateAround(Vector3.zero, transform.right, moveVertical * sensitivity);
        }

        UpdateCameraPosition();
    }

    // makes sure that the camera stays orbiting the player nicely
    // safe space and all that
    private void UpdateCameraPosition()
    {
        // make sure that the camera keeps up
        Vector3 direction = (transform.position - player.transform.position).normalized;
        Vector3 desiredPosition = player.transform.position + direction * cameraOffset.magnitude;

        desiredPosition.y = player.transform.position.y + cameraOffset.y;

        transform.position = desiredPosition;
        transform.LookAt(player.transform);
    }
}
