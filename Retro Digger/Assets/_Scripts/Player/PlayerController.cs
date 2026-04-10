using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.RenderGraphModule;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The movement speed of the player.
    /// </summary>
    public float speed;

    /// <summary>
    /// The rigid body of the player.
    /// </summary>
    public Rigidbody rb;
    /// <summary>
    /// The Player's body GameObject.
    /// </summary>
    public GameObject playerBody;

    public InputActionReference move;
    /// <summary>
    /// The input action for digging. We use the "started" and "canceled" events. 
    /// </summary>
    public InputActionReference space;

    /// <summary>
    /// The player character's animator, used to trigger running/digging animations.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Represents whether the player is currently digging. 
    /// </summary>
    private bool _isDigging = false;
    void SetIsDigging(bool value) => animator.SetBool("IsDigging", _isDigging = value);

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        space.action.started += _ => OnStartDigging();
        space.action.canceled += _ => OnStopDigging();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        // No movement while digging, but still allow turning and similar. 
        Vector3 input = _isDigging
            ? Vector3.zero
            : move.action.ReadValue<Vector3>();
        animator.SetBool("IsRunning", input is not { x: 0, z: 0 });

        var moveDir = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.z), 1f);
        rb.linearVelocity = speed * Time.deltaTime * moveDir;

        if (playerBody != null && moveDir != Vector3.zero)
        {
            // reverse look direction to match sprite facing, and rotate 180° to match art orientation
            Quaternion look = Quaternion.LookRotation(moveDir, Vector3.up);
            playerBody.transform.localRotation = look * Quaternion.Euler(0f, 180F, 0f);
        }
    }

    void OnStartDigging()
    {
        RockCollider collider;
        float rayDistance = .75f;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = playerBody.transform.TransformDirection(Vector3.back);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 60 * 20);
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance)
            && (collider = hit.collider.GetComponent<RockCollider>()) != null)
        {
            collider.HitByPlayer(this);
            Debug.Log("Ray hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Ray did not hit anything.");
        }

        SetIsDigging(true);
    }

    void OnStopDigging() => SetIsDigging(false);
}
