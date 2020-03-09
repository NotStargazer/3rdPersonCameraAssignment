//©©©©©©©Samuel Gustafsson©©©©©©©2020©©©©©©©©

using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerInput : MonoBehaviour
{
    //Input Names
    const string mouseX = "Mouse X";
    const string mouseY = "Mouse Y";
    const string movementZ = "Vertical";
    const string MouseWheel = "Mouse ScrollWheel";

    //Components
    PlayerCharacter player;

    private void Awake()
    {
        player = GetComponent<PlayerCharacter>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Assert.IsNotNull(player.cameraMovement, "Camera attachment missing.");
        player.cameraMovement.UpdateCameraCoordinates(new Vector3(Input.GetAxis(mouseX), Input.GetAxis(mouseY), Input.GetAxis(MouseWheel)));

        player.desiredMovement = Input.GetAxisRaw(movementZ);
    }
}
