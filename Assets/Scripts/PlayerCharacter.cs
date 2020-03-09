//©©©©©©©Samuel Gustafsson©©©©©©©2020©©©©©©©©

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCharacter : MonoBehaviour
{
    //Private
    Vector3 desiredDirection;
    Vector3 directionSmoothing;

    //Public
    [System.NonSerialized] public float desiredMovement;

    //Properties
    [Header("Character")]
    [SerializeField] float characterMoveSpeed;
    [Tooltip("Time it takes for character to smooth desired direction.")]
    [SerializeField] float characterRotationSmoothing;

    //Components
    Rigidbody playerRigidBody;
    internal CameraMovement cameraMovement;

    void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(desiredMovement) > 0f)
        {
            Vector2 direction = cameraMovement.Direction;
            desiredDirection = new Vector3(direction.x, 0, direction.y);

            playerRigidBody.AddForce(desiredMovement * transform.up * characterMoveSpeed);
        }

        if (playerRigidBody.velocity.magnitude < 2f)
        {
            desiredDirection = Vector3.Lerp(desiredDirection, transform.up, 0.10f);
        }

        transform.up = Vector3.SmoothDamp(transform.up, desiredDirection, ref directionSmoothing, characterRotationSmoothing);
    }
}
