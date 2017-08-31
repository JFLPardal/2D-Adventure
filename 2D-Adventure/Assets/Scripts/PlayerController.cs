using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour{

	//direction the player is facing
	public enum FACEDIRECTION {FACELEFT = -1, FACERIGHT = 1};
	public FACEDIRECTION facing = FACEDIRECTION.FACERIGHT;
	public LayerMask groundLayer;

	private Rigidbody2D rb = null;
	private Transform transform = null;

	public CircleCollider2D feetCollider = null;
	public bool isGrounded = false;

	public string horzAxis = "Horizontal";
	public string jumpButton = "Jump";

	public float maxSpeed = 50f;
	public float JumpPower = 600;
	public float jumpTimeOut = 0.6f;

	private bool canJump = true;
	public bool canControl = true;
	public static PlayerController playerInstance = null;

	public static float health
	{
		get
		{
			return _health;
		}
		set 
		{ 
			_health = value;
			if (_health <= 0)
				Die ();
		}
	}

	[SerializeField]
	private static float _health = 100f;

	//-----------------------------------------------------------
	void Awake()
	{
		rb = GetComponent<Rigidbody2D> ();
		transform = GetComponent<Transform> ();

		playerInstance = this;
	}
	//-------------------------------------------------------------
	//is player grounded
	private bool GetGrounded()
	{
		Vector2 circleCenter = new Vector2 (transform.position.x, transform.position.y) + feetCollider.offset;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll (circleCenter, feetCollider.radius, groundLayer);

		if (hitColliders.Length > 0)
			return true;
		return false;
	}
	//-----------------------------------------------------------
	//flips character direction
	private void FlipDirection()
	{
		facing = (FACEDIRECTION)((int)facing * -1f);
		Vector3 localScale = transform.localScale;
		localScale.x *= -1f;
		transform.localScale = localScale;
	}
	//---------------------------------------------------------
	//engage jump
	private void Jump()
	{
		if (!isGrounded || !canJump)
			return;

		rb.AddForce (Vector2.up * JumpPower);
		canJump = false;
		Invoke ("ActivateJump", jumpTimeOut);
	}
	//----------------------------------------------------
	//activates can jump variable after jump timeout 
	//prevents double jump
	private void ActivateJump()
	{
		canJump = true;
	}
	//----------------------------------------------------
	void FixedUpdate()
	{
		if (!canControl || health <= 0f)
			return;

		//update grounded status
		isGrounded = GetGrounded();
		float horz = CrossPlatformInputManager.GetAxis (horzAxis);
		rb.AddForce (Vector2.right * horz * maxSpeed);

		if (CrossPlatformInputManager.GetButton (jumpButton))
			Jump ();

		//clamp velocity
		rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -Mathf.Infinity, JumpPower));

		if((horz < 0f && facing != FACEDIRECTION.FACELEFT) || 
		   (horz > 0f && facing != FACEDIRECTION.FACERIGHT))
			FlipDirection();
	}
	//--------------------------------------------------------
	void OnDestroy()
	{
		playerInstance = null;
	}
	//-------------------------------------------------------
	static void Die()
	{
		Destroy (PlayerController.playerInstance.gameObject);
	}
	//------------------------------------------------------
	//resets player back to defaults
	public static void Reset()
	{
		health = 100f;
	}
}
