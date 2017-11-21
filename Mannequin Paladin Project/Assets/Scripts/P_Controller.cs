using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controller : MonoBehaviour {

	private Animator _anim;
	private Rigidbody2D _rb;
	private CapsuleCollider2D _col;
	private SpriteRenderer _rend;
	public float speed;
	public float jumpPow;
	public bool isGrounded;
	public float gravity;
	public float gravMod;
	public float maxVel;

    public Transform[] groundPoints;
    public float groundRadius;
    private LayerMask whatIsGround = 1<<8;

	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator> ();
		_rb = GetComponent<Rigidbody2D> ();
		_col = GetComponent<CapsuleCollider2D> ();
		_rend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        isGrounded = checkGrounded();
        _anim.SetBool("isGrounded", isGrounded);
        _anim.SetFloat ("y_velo", _rb.velocity.y);

		_anim.SetBool ("attacking", false);

		float horiz = Input.GetAxis ("Horizontal");
		float jump = Input.GetAxis ("Jump");

		float attack = Input.GetAxis ("Fire1");

        HandleMovement(horiz, jump);

        HandleAnim(horiz, jump, attack);
		
		
	}

    private void HandleMovement(float xInput, float jumpInput)
    {
        Vector2 dir = new Vector2(xInput, 0);

        Debug.Log(dir.magnitude);

        if (isGrounded && jumpInput > 0)
        {
            //perform jump
            isGrounded = false;
            _rb.AddForce(new Vector2(0, jumpPow), ForceMode2D.Impulse);
        }

        if ((_rb.velocity.x <= maxVel && _rb.velocity.x >= -maxVel))
        {
            _rb.AddForce(dir * speed, ForceMode2D.Impulse);
        }

        synthGravity();
        LinearDrag();
    }

    private void synthGravity()
    {
        if (_rb.velocity.y > 0)
        {
            _rb.AddForce(new Vector2(0, -gravity));
        }
        else
        {
            _rb.AddForce(new Vector2(0, -gravity * gravMod));
        }
    }

    private void LinearDrag()
    {
        _rb.AddForce(new Vector2(-_rb.velocity.x/2, 0), ForceMode2D.Impulse);
    }

    private void HandleAnim(float xInput, float jumpInput, float attackInput)
    {
        if (_rb.velocity.x < 0)
        {
            _rend.flipX = true;
        }
        else if (_rb.velocity.x > 0)
        {
            _rend.flipX = false;
        }
        _anim.SetBool("walking", (_rb.velocity.x != 0 || xInput != 0) ? true : false);
        if (attackInput > 0)
        {
            _anim.SetBool("attacking", true);
        }
    }

	private bool checkGrounded() {
		if (_rb.velocity.y <= 0) {
            foreach (Transform point in groundPoints)
            {
                //store colliders that overlap from the points
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                

                //iterate through colliders
                for (int i = 0; i < colliders.Length; i++)
                {
                    //if curCollider is not the THIS gameObject (the player)
                    if (colliders[i].gameObject != gameObject)
                    {
                        Debug.Log("grounded");
                        return true;
                    }
                }
            }
		}
        Debug.Log("not grounded");
        return false;
	}

}
