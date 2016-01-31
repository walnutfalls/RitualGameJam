using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Rose
{
    [RequireComponent(typeof(Animator))]
    public class RoseAnimationController : MonoBehaviour
    {
        public const string HorizontalSpeedVar = "horizontalSpeed";
        private const string VerticalSpeedVar = "verticalSpeed";
        private const string GroundedVar = "grounded";

        #region Editor Variables
        public Rigidbody2D characterRigidBody2d;
        public LayerMask ground;
        public Transform groundCheck;
        public float groundCheckDiam = 3f;
        #endregion

        #region private
        private Animator _animator;
        #endregion

        #region Properties
        public bool IsGrounded { get; private set; }
        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();            
        }
            
       
        // Update is called once per frame
        void Update()
        {
            Vector2 vel = characterRigidBody2d.velocity;
            Vector2 normVelocity = characterRigidBody2d.velocity.normalized;

            float horizontalSpeed = Mathf.Abs(vel.x);
            float verticalSpeed = Mathf.Abs(vel.y);

            
            _animator.SetFloat(HorizontalSpeedVar, horizontalSpeed);

            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDiam, ground);
            Debug.DrawLine(groundCheck.position, groundCheck.position + groundCheckDiam * Vector3.down, Color.red);

            _animator.SetBool(GroundedVar, IsGrounded);
        }
    }
}