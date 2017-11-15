using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Controller : MonoBehaviour {

	private Animator _anim;
	private Rigidbody2D _rb;
	private BoxCollider2D _col;
	private SpriteRenderer _rend;
	public float speed;

	// Use this for initialization
	void Start () {
		_anim = GetComponent<Animator> ();
		_rb = GetComponent<Rigidbody2D> ();
		_col = GetComponent<BoxCollider2D> ();
		_rend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		_anim.SetBool ("attacking", false);
		float horiz = Input.GetAxis ("Horizontal");
		float jump = Input.GetAxis ("Jump");

		float attack = Input.GetAxis ("Fire1");

		Vector2 dir = new Vector2 (horiz, 0);

		if (_rb.velocity.x < 0) {
			_rend.flipX = true;
		} else if (_rb.velocity.x > 0) {
			_rend.flipX = false;
		}
		_anim.SetBool ("walking", (_rb.velocity.x != 0 || horiz != 0) ? true : false);
		if (attack > 0) {
			_anim.SetBool ("attacking", true);
		}

		_rb.AddForce (dir * speed, ForceMode2D.Impulse);
	}

}
