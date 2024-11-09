using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float sensitivity = 1;
    [SerializeField]
    private Vector3 cameraOffset = new (10, 5, 0);
    // states
    private Vector3 lastMousePosition;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player != null) return;
        player = GameObject.FindWithTag("Player");
        lastPlayerPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();

        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1))
        {
            // Vector3 delta = Input.mousePosition - lastMousePosition;
            // MoveCamera(delta.x, delta.y);
            RotateCamera(disableY: true);
            lastMousePosition = Input.mousePosition;
        }
    }

    private void FollowPlayer()
    {
        Vector3 distanceDifference = player.transform.position - lastPlayerPosition;
        transform.position += distanceDifference;
        lastPlayerPosition = player.transform.position;
        UpdateCameraPosition();
    }

    // This is a free movement camera
    private void MoveCamera(float xInput, float zInput)
    {
        float zMove = Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * zInput - Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * xInput;
        float xMove = Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180) * zInput + Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180) * xInput;

        transform.position = transform.position + new Vector3(xMove, 0, zMove);
    }

    // this only rotates the camera
    private void RotateCamera(bool disableX = false, bool disableY = false)
    {
        if (!disableX)
        {
            float moveHorizontal = Input.GetAxis("Mouse X");
            transform.RotateAround(player.transform.position, -Vector3.up, moveHorizontal * sensitivity); //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
        }
        if (!disableY)
        {
            float moveVertical = Input.GetAxis("Mouse Y");
            transform.RotateAround(Vector3.zero, transform.right, moveVertical * sensitivity); // again, use transform.Rotate(transform.right * rotateVertical * sensitivity) if you don't want the camera to rotate around the player
        }

        UpdateCameraPosition();
    }

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
