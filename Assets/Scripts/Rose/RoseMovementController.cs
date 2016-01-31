using UnityEngine;
using System.Linq;
using GameStateManagement;
using Assets.Scripts.GameStates;

namespace Assets.Scripts.Rose
{
    public class RoseMovementController : MonoBehaviour
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private const string Jump = "Jump";

        #region EditorVariables
        public Rigidbody2D roseRigidBody2d;
        public RoseAnimationController animationController;
        public CircleCollider2D characterBottom;        
        public PhysicsMaterial2D stoppingMaterial;
        public float maxHorizontalV = 50.0f;
        public float maxHorizontalVFlowers = 30.0f;
        public float moveForceMag = 400.0f;
        #endregion

        #region Private Variables
        private PhysicsMaterial2D _originalMaterial;
        private float _originalGravity;
        private Vector2 _currentFloorNormal;
        private float _lastHAxis;

        private bool _lastFrameJumpAxis;
        #endregion


        #region Properties
        public Vector2 MoveDirection { get; private set; }
        #endregion

        private void Awake()
        {
             _originalMaterial = characterBottom.sharedMaterial;
            _originalGravity = roseRigidBody2d.gravityScale;
        }


        private void Update()
        {
            float horizAxis = Input.GetAxis(Horizontal);
            float vertAxis = Input.GetAxis(Vertical);

            if (roseRigidBody2d.velocity.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            else if(roseRigidBody2d.velocity.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);


            bool grounded = animationController.IsGrounded;



            if (Input.GetAxis(Jump) > 0 && grounded && !_lastFrameJumpAxis)
            {
                grounded = false;
                roseRigidBody2d.AddForce(Vector2.up * 5000.0f);
                _lastFrameJumpAxis = true;
                GetComponent<Animator>().SetTrigger("jumped");
            }
            else if (Input.GetAxis(Jump) == 0 && grounded)
                _lastFrameJumpAxis = false;

            if (grounded)
                roseRigidBody2d.gravityScale = _originalGravity * 2;
            else
                roseRigidBody2d.gravityScale = _originalGravity;

            // set up move direction
            if (horizAxis > 0)
                MoveDirection = new Vector2(_currentFloorNormal.y, -_currentFloorNormal.x).normalized;
            else if (horizAxis < 0)
                MoveDirection = new Vector2(-_currentFloorNormal.y, _currentFloorNormal.x).normalized;

            //apply force in move direction
            if(grounded)
                roseRigidBody2d.AddForce(MoveDirection * moveForceMag);

            Debug.DrawRay(transform.position, MoveDirection, Color.green);
            

            if ((Mathf.Abs(horizAxis) < _lastHAxis || horizAxis == 0) && grounded)
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

            // limit velocity only when grounded
            float maxV = GameStateManager.Instance.CurrentGameState.Is<FlowerPickingState>() ? maxHorizontalVFlowers : maxHorizontalV;
            float vX = roseRigidBody2d.velocity.x;
            if(Mathf.Abs(vX) > maxV && horizAxis != 0 && grounded)
            {                
                roseRigidBody2d.velocity = new Vector2(maxV * Mathf.Sign(vX), roseRigidBody2d.velocity.y);
            }

            _lastHAxis = Mathf.Abs(horizAxis);                   
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // set current collision normal
            var points = collision.contacts.Where(c => c.otherCollider == characterBottom);
            if (!points.Any())
                _currentFloorNormal = Vector2.zero;

            if (points.Count() == 1)
                _currentFloorNormal = points.ElementAt(0).normal;
            else
                _currentFloorNormal = Vector2.up; // no way to know - multiple collisions.
            
        }
    }
}
