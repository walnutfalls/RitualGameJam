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
        #endregion

        private void Update()
        {
            float horizAxis = Input.GetAxis(Horizontal);
            float vertAxis = Input.GetAxis(Vertical);

            roseRigidBody2d.AddForce(new Vector2(horizAxis, 0) * 20.0f);

            if(Input.GetAxis(Jump) > 0 && animationController.IsGrounded)
            {
                roseRigidBody2d.AddForce(Vector2.up * 200.0f);
            }

            if(horizAxis == 0 && animationController.IsGrounded)
            {
                roseRigidBody2d.velocity *= 0.3f;
            }
        }
    }
}
