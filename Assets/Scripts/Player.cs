using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Variables")]
    public float MoveSpeed = 10f;
    public float JumpSpeed = 3f;
    public Vector2 direction;
    private bool FacingRight = true;

    [Header("Components")]
    public Rigidbody2D RB;
    public Animator Animator;
    public LayerMask groundLayer;

    [Header("physics")]
    public float XMaxSpeed = 3f;
    public float YMaxSpeed = 3f;
    public float LinearDrag = 1;
    public float gravity = 1;
    public float fallmultplier = 5;


    [Header("Colision")]
    public bool OnGround;
    public float GroundLength = 0.6f;
    public Vector3 offset;




    // Start is called before the first frame update
    void Start()
    {
        
    }




    // Update is called once per frame
    void Update()
    {
        OnGround = Physics2D.Raycast(transform.position, Vector2.down,GroundLength, groundLayer);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && OnGround)
        {
            Jump();
        }

    }

    private void FixedUpdate()
    {
        Move(direction.x);
        ModifyPhysics();
    }

    void Move(float Horizontal)
    {
        RB.AddForce(Vector2.right * Horizontal * MoveSpeed);
        Animator.SetFloat("Horizontal", Mathf.Abs(RB.velocity.x));
        if(Horizontal>0 && !FacingRight || Horizontal<0 && FacingRight)
        {
            Flip();
        }
        if (Mathf.Abs(RB.velocity.x) > XMaxSpeed)
        {
            RB.velocity = new Vector2(Mathf.Sign(RB.velocity.x) * XMaxSpeed, RB.velocity.y);
        }
        if (Mathf.Abs(RB.velocity.y) > YMaxSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Sign(RB.velocity.y) * YMaxSpeed);
        }

    }

    void ModifyPhysics()
    {
        if (OnGround)
        {
            if (Mathf.Abs(direction.x) < 0.4)
            {
                RB.drag = LinearDrag;
            }
            else
            {
                RB.drag = 0;
            }
            RB.gravityScale = 0;
        }
        else
        {
            RB.gravityScale = gravity;
            RB.drag = LinearDrag * 0.01f;
            if(RB.velocity.y < 0)
            {
                RB.gravityScale = gravity * fallmultplier;
            }
            //else if(RB.velocity.y>0 && !Input.GetButton("Jump"))
            //{
            //    RB.gravityScale = gravity * (fallmultplier / 2);
            //}
        }
        
    }

    void Jump()
    {
        RB.velocity = new Vector2(RB.velocity.x,0);
        RB.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
    }

    void Flip()
    {
        FacingRight = !FacingRight;
        transform.rotation = Quaternion.Euler(0,FacingRight? 0 : 180 ,0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.down * GroundLength);
        Gizmos.DrawLine(transform.position - offset, transform.position - offset + Vector3.down * GroundLength);

    }
}
