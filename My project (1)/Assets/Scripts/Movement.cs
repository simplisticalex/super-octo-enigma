using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.0f;
    private Rigidbody rb;
    private Vector3 moveDir;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical"); ;
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveSpeed * moveDir * Time.fixedDeltaTime);
    }
}
