using UnityEngine;
using System.Linq;
using GameStateManagement;
using Assets.Scripts.GameStates;
using System.Collections;
using System;

namespace Assets.Scripts.Rose
{

    [RequireComponent(typeof(AxisInterpreter))]
    public class RoseMovementController : MonoBehaviour
    {
        private enum FaceDirection
        {
            Right,
            Left,
            Front
        }

        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";       

     
        #region EditorVariables
        public Rigidbody2D roseRigidBody2d;
        public RoseAnimationController animationController;
        public CircleCollider2D characterBottom;        
        public PhysicsMaterial2D stoppingMaterial;

        public LayerMask ground;
        public Transform groundCheck;
        public float groundCheckDiam = 3f;

        public float maxHorizontalV = 50.0f;
        public float maxHorizontalVFlowers = 30.0f;
        public float moveForceMag = 400.0f;
        private float jumpForce = 5000.0f;
        #endregion

        #region Private Variables
        private PhysicsMaterial2D _originalMaterial;
        private float _originalGravity;
        private Vector2 _currentFloorNormal;
        private float _lastHAxis;

        private float _lastFrameJumpAxis;
        private bool _canJump;
        private FaceDirection _faceDirection;
        private AxisInterpreter _axisInterpreter;
        #endregion


        #region Properties
        public Vector2 MoveDirection { get; private set; }
        public bool IsGrounded { get; private set; }
        #endregion

        private void Awake()
        {
             _originalMaterial = characterBottom.sharedMaterial;
            _originalGravity = roseRigidBody2d.gravityScale;
            _faceDirection = FaceDirection.Right;
        }

        private void Start()
        {
            _canJump = true;
            StartCoroutine(MonitorFaceDirection());
            _axisInterpreter = GetComponent<AxisInterpreter>();

            _axisInterpreter.OnJumpKeyDown += () => StartCoroutine(Jump());
        }

        private void Update()
        {
            float horizAxis = Input.GetAxis(Horizontal);
            float vertAxis = Input.GetAxis(Vertical);
                        
            switch (_faceDirection)
            {
                case FaceDirection.Right:
                    transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    break;
                case FaceDirection.Left:
                    transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                    break;
                default:
                    break;
            }

            CheckIsGrounded();

            if (IsGrounded)
                roseRigidBody2d.gravityScale = _originalGravity * 2.5f;
            else
                roseRigidBody2d.gravityScale = _originalGravity;

            // set up move direction
            if (horizAxis > 0)
                MoveDirection = new Vector2(_currentFloorNormal.y, -_currentFloorNormal.x).normalized * horizAxis;
            else if (horizAxis < 0)
                MoveDirection = new Vector2(-_currentFloorNormal.y, _currentFloorNormal.x).normalized * -horizAxis;

            //apply force in move direction
            if (IsGrounded)
                roseRigidBody2d.AddForce(MoveDirection * moveForceMag);

            Debug.DrawRay(transform.position, MoveDirection, Color.green);
            

            if ((Mathf.Abs(horizAxis) < _lastHAxis || horizAxis == 0) && IsGrounded)
            {
                characterBottom.sharedMaterial = stoppingMaterial;
                characterBottom.enabled = false;
                characterBottom.enabled = true;
            }
            else
            {
                characterBottom.sharedMaterial = _originalMaterial;
                characterBottom.enabled = false;
                characterBottom.enabled = true;
            }
            _lastHAxis = Mathf.Abs(horizAxis);


            CapVelocity(horizAxis);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != 12) return;

            // set current collision normal
            var points = collision.contacts.Where(c => c.otherCollider == characterBottom);
            if (!points.Any())
                _currentFloorNormal = Vector2.zero;

            if (points.Count() == 1)
                _currentFloorNormal = points.ElementAt(0).normal;
            else
                _currentFloorNormal = Vector2.up; // no way to know - multiple collisions.
            
        }


        #region Private methods
        private void CheckIsGrounded()
        {
            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDiam, ground);
            Debug.DrawLine(groundCheck.position, groundCheck.position + groundCheckDiam * Vector3.down, Color.red);
        }

        private void CapVelocity(float horizAxis)
        {
            // limit velocity only when grounded
            float maxV = GameStateManager.Instance.CurrentGameState.Is<FlowerPickingState>() ? maxHorizontalVFlowers : maxHorizontalV;
            float vX = roseRigidBody2d.velocity.x;
            if (Mathf.Abs(vX) > maxV && horizAxis != 0 && IsGrounded)
            {
                roseRigidBody2d.velocity = new Vector2(maxV * Mathf.Sign(vX), roseRigidBody2d.velocity.y);
                GetComponent<Animator>().SetFloat(RoseAnimationController.HorizontalSpeedVar, maxV);
            }
        }
        #endregion
        


        #region Coroutines
        IEnumerator MonitorFaceDirection()
        {
            while (true)
            {
                float horizontal = Input.GetAxis(Horizontal);

                if (horizontal > 0)
                    _faceDirection = FaceDirection.Right;
                if (horizontal < 0)
                    _faceDirection = FaceDirection.Left;

                yield return null;
            }
        }

        IEnumerator Jump()
        {
            if (IsGrounded && !_axisInterpreter.JumpKeyDown)
            {
                _canJump = false;

                roseRigidBody2d.AddForce(Vector2.up * jumpForce);
                GetComponent<Animator>().SetTrigger("jumped");
                

                yield return new WaitForSeconds(0.1f);
                
                _canJump = true;
            }
        }

       
        #endregion
    }
}
