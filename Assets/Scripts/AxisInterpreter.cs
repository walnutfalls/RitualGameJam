using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AxisInterpreter : MonoBehaviour
    {
        private const string JumpAxis = "Jump";
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";


        public event Action OnJumpKeyDown;
        public event Action OnJumpKeyUp;

        public bool JumpKeyDown { get; private set; }


        private void Start()
        {
            StartCoroutine(MonitorJump());
        }



        IEnumerator MonitorJump()
        {
            float lastFrameJumpAxis = -1;

            while (true)
            {
                float jump = Input.GetAxisRaw(JumpAxis);
                float horizAxis = Input.GetAxis(Horizontal);
                float vertAxis = Input.GetAxis(Vertical);
              //  Debug.Log(horizAxis.ToString() + ", " + vertAxis.ToString() + ". jump: " + jump.ToString());


                if (Mathf.Abs(jump) < lastFrameJumpAxis && lastFrameJumpAxis != -1)
                {
                    if (OnJumpKeyUp != null) OnJumpKeyUp();

                    JumpKeyDown = false;
                    lastFrameJumpAxis = -1;
                }
                else if (Mathf.Abs(jump) == 1 && lastFrameJumpAxis == -1)
                {
                    if (OnJumpKeyDown != null) OnJumpKeyDown();

                    JumpKeyDown = true;
                    lastFrameJumpAxis = Mathf.Abs(jump);
                }

                yield return null;
            }
        }
    }
}
