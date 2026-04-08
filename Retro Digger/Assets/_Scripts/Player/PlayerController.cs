using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float groundDist;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    public InputActionReference move;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
        Vector3 moveDir = new(input.x, 0, input.z);
        rb.linearVelocity = moveDir * speed * Time.deltaTime;

        if (sr != null)
            sr.flipX = input.x == 0 ? sr.flipX : input.x < 0;
    }
}
