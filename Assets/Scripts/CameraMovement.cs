//©©©©©©©Samuel Gustafsson©©©©©©©2020©©©©©©©©

using UnityEngine;
using UnityEngine.Assertions;

public class CameraMovement : MonoBehaviour
{
    //Public
    public Vector2 Direction 
    { 
        get
        { return new Vector2(Mathf.Cos(desiredCameraX * Mathf.Deg2Rad), Mathf.Sin(desiredCameraX * Mathf.Deg2Rad)); }
    }

    //Properties
    [Header("Camera Movement")]
    [SerializeField] float cameraSensitivity;
    [Tooltip("Time it takes for camera to smooth desired position.")]
    [SerializeField] float cameraSmoothness;
    [SerializeField] float minimumCameraYClamp;
    [SerializeField] float maximumCameraYClamp;

    [Header("Camera Offset")]
    [SerializeField] float minimumAdditiveZoomClamp;
    [SerializeField] float maximumAdditiveZoomClamp;
    [SerializeField] float zoomSenstivity;
    [Tooltip("Time it takes for camera to smooth desired zoom.")]
    [SerializeField] float zoomSmootness;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] LayerMask collisionLayers = default;

    //Script Attachments
    [Header("Attachments")]
    [SerializeField] PlayerCharacter target;

    //Private
    float desiredCameraX = 270;
    float desiredCameraY = 0;
    float desiredCameraZoom = 0;
    float cameraX = 270;
    float cameraY = 0;
    float cameraZoom;
    float cameraXSmoothVelocity;
    float cameraYSmoothVelocity;
    float cameraZoomVelocity;
    Vector3 cameraLocation;

    private void Awake()
    {
        Assert.IsNotNull(target, "Camera attachment missing.");
        SetCameraLocation();
        target.cameraMovement = this;
    }

    internal void UpdateCameraCoordinates(Vector3 mouseScrollDelta)
    {
        desiredCameraX -= mouseScrollDelta.x * cameraSensitivity * Time.deltaTime;
        desiredCameraY -= mouseScrollDelta.y * cameraSensitivity * Time.deltaTime;
        desiredCameraZoom -= mouseScrollDelta.z * zoomSenstivity * Time.deltaTime;        

        if (desiredCameraX > 360)
        {
            desiredCameraX -= 360;
            cameraX -= 360;
        }

        if (desiredCameraX < 0)
        {
            desiredCameraX += 360;
            cameraX += 360;
        }

        cameraX = Mathf.SmoothDamp(cameraX, desiredCameraX, ref cameraXSmoothVelocity, cameraSmoothness);

        desiredCameraY = Mathf.Clamp(desiredCameraY, minimumCameraYClamp, maximumCameraYClamp);

        cameraY = Mathf.SmoothDamp(cameraY, desiredCameraY, ref cameraYSmoothVelocity, cameraSmoothness);

        desiredCameraZoom = Mathf.Clamp(desiredCameraZoom, minimumAdditiveZoomClamp, maximumAdditiveZoomClamp);

        cameraZoom = Mathf.SmoothDamp(cameraZoom, desiredCameraZoom, ref cameraZoomVelocity, zoomSmootness);

        SetCameraLocation();
    }

    private void SetCameraLocation()
    {
        float zoom = cameraOffset.z - cameraZoom;
        Vector3 targetPosition = target.transform.position;
        Vector2 rotationXNormal = new Vector2(Mathf.Cos(cameraX * Mathf.Deg2Rad), Mathf.Sin(cameraX * Mathf.Deg2Rad));
        Vector2 rotationYNormal = new Vector2(Mathf.Cos(cameraY * Mathf.Deg2Rad), -Mathf.Sin(cameraY * Mathf.Deg2Rad));
        Vector3 newDirection = new Vector3(rotationXNormal.x * rotationYNormal.x, rotationYNormal.y, rotationXNormal.y * rotationYNormal.x).normalized;

        float collisionOffset = 0;

        if (Physics.Linecast(target.transform.position, target.transform.position + newDirection * zoom, out RaycastHit hit, collisionLayers))
        {
            collisionOffset = zoom + hit.distance;
        }

        newDirection *= zoom - collisionOffset;

        cameraLocation = new Vector3(newDirection.x + cameraOffset.x, newDirection.y + cameraOffset.y, newDirection.z) + targetPosition;

        transform.position = cameraLocation;
        transform.LookAt(new Vector3(targetPosition.x + cameraOffset.x, targetPosition.y + cameraOffset.y, targetPosition.z), Vector3.up);
    }
}
