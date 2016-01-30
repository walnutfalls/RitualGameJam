using UnityEngine;

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
        #endregion

        private PhysicsMaterial2D _originalMaterial;

        private void Awake()
        {
             _originalMaterial = characterBottom.sharedMaterial;
        }


        private void Update()
        {
            float horizAxis = Input.GetAxis(Horizontal);
            float vertAxis = Input.GetAxis(Vertical);

            roseRigidBody2d.AddForce(new Vector2(horizAxis, 0) * 20.0f);

            if(Input.GetAxis(Jump) > 0 && animationController.IsGrounded)
                roseRigidBody2d.AddForce(Vector2.up * 200.0f);


            if (horizAxis == 0 && animationController.IsGrounded)
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
        }
    }
}
