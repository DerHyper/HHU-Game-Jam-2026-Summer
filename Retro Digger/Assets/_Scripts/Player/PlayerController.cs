using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;
    public GameObject playerBody;

    public InputActionReference move;
    public InputActionReference space;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        space.action.performed += _ => CastRay();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer)
            && hit.collider != null)
        {
            Vector3 movePos = transform.position;
            movePos.y = hit.point.y + groundDist;
            transform.position = movePos;
        }

        Vector3 input = move.action.ReadValue<Vector3>();
        var moveDir = Vector3.ClampMagnitude(new Vector3(input.x, 0, input.z), 1f);
        rb.linearVelocity = speed * Time.deltaTime * moveDir;

        if (sr != null)
            sr.flipX = input.x == 0 ? sr.flipX : input.x < 0;
        if (playerBody != null && moveDir != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(moveDir, Vector3.up);
            playerBody.transform.localRotation = look * Quaternion.Euler(0f, 180F, 0f);
        }
    }

    void CastRay()
    {
        Collider collider;
        float rayDistance = .75f;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = playerBody.transform.TransformDirection(Vector3.back);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red, 60 * 20);
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance)
            && (collider = hit.collider.GetComponentInParent<Collider>()) != null)
        {
            collider.OnTriggerEnter(null);
            Debug.Log("Ray hit: " + hit.collider.name);
        }
        else
        {
            Debug.Log("Ray did not hit anything.");
        }
    }
}
