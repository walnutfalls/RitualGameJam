using UnityEngine;
using System.Collections;
namespace Assets.Scripts.Rose
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(RoseMovementController))]
    public class RoseAnimationController : MonoBehaviour
    {
        public const string HorizontalSpeedVar = "horizontalSpeed";
        private const string VerticalSpeedVar = "verticalSpeed";
        private const string GroundedVar = "grounded";

        #region Editor Variables
        public Rigidbody2D characterRigidBody2d;                
        #endregion

        #region private
        private Animator _animator;
        private RoseMovementController _movementController;
        #endregion

        #region Properties
        public bool IsGrounded { get; private set; }
        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _movementController = GetComponent<RoseMovementController>();
        }
            
       
        // Update is called once per frame
        void Update()
        {
            Vector2 vel = characterRigidBody2d.velocity;
            Vector2 normVelocity = characterRigidBody2d.velocity.normalized;

            float horizontalSpeed = Mathf.Abs(vel.x);
            float verticalSpeed = Mathf.Abs(vel.y);

            
            _animator.SetFloat(HorizontalSpeedVar, horizontalSpeed);
            _animator.SetFloat(VerticalSpeedVar, verticalSpeed);
            _animator.SetBool(GroundedVar, _movementController.IsGrounded);
        }
    }
}