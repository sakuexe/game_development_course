using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float sensitivity = 1;
    private Vector3 lastMousePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null) return;
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

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
    }
}
