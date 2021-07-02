using UnityEngine;
using UnityEngine.Events;

namespace GameProtoProject
{
	[RequireComponent(typeof(CharacterProperties))]
	public class CharacterController : MonoBehaviour
	{
		[SerializeField] private float m_JumpForce = 800f;
		[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
		[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
		[SerializeField] private bool m_AirControl = false;
		[SerializeField] private LayerMask m_WhatIsGround;
		[SerializeField] private Transform m_GroundCheck;
		[SerializeField] private Transform m_WallJumpCheck;
		[SerializeField] private float maxVelocity = 100f;

		private CharacterProperties properties;

		const float k_GroundedRadius = .2f;
		private bool m_Grounded;
		private bool canWallJump;
		private bool wallJumpedOnce;
		const float wallJumpRadius = .2f;
		private Rigidbody2D m_Rigidbody2D;
		public bool FacingRight { get; private set; } = true;
		private Vector3 m_Velocity = Vector3.zero;

		void Start()
		{
			properties = GetComponent<CharacterProperties>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			bool wasGrounded = m_Grounded;
			m_Grounded = false;
			canWallJump = false;

			Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders[i].gameObject != gameObject)
				{
					m_Grounded = true;
				}
			}

			if (wasGrounded != m_Grounded)
			{
				GfxEventHandler.Instance.groundedStateChanged.Invoke(m_Grounded);
			}

			if (m_Grounded) return;

			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(m_WallJumpCheck.position, wallJumpRadius, m_WhatIsGround);
			for (int i = 0; i < collidersWall.Length; i++)
			{
				if (collidersWall[i].gameObject != gameObject)
				{
					canWallJump = true;
				}
			}

			m_Rigidbody2D.velocity = Vector2.ClampMagnitude(m_Rigidbody2D.velocity, maxVelocity);
		}


		public void Move(float move, bool jump)
		{
			if (properties.isDead) return;

			if (m_Grounded || m_AirControl)
			{
				Vector3 targetVelocity = new Vector2(move * properties.speed, m_Rigidbody2D.velocity.y);
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				if (move > 0 && !FacingRight)
				{
					Flip();
				}
				else if (move < 0 && FacingRight)
				{
					Flip();
				}
			}
			if (m_Grounded && jump)
			{
				GfxEventHandler.Instance.OnJump.Invoke();
				m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				wallJumpedOnce = false;
				GfxEventHandler.Instance.groundedStateChanged.Invoke(m_Grounded);
			}
			else
			{
				if (canWallJump && jump && !wallJumpedOnce)
				{
					GfxEventHandler.Instance.OnWallJump.Invoke();
					m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
					wallJumpedOnce = true;
				}
			}
		}

		private void Flip()
		{
			FacingRight = !FacingRight;

			transform.Rotate(0f, 180f, 0f);
		}
	}
}