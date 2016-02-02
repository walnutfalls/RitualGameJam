using Assets.Scripts.GameStates;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        #region Editor Variables
        public Transform rose;
        public RectTransform targetCharPos;
        public Transform leftWall;
        public Transform rightWall;
        #endregion

        private Camera _thisCamera;
        private float _originalZ;

        private void Awake()
        {
            _thisCamera = GetComponent<Camera>();
            _originalZ = transform.position.z;
        }

        private void Start()
        {
            FightingState.Instance.OnEnter += () =>
            {
                targetCharPos.GetComponent<Animator>().SetTrigger("Shift");
                GetComponent<Animator>().SetTrigger("zoomOut");
            };
        }

        private void Update()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(targetCharPos.position);
            Vector2 toChar = (Vector2)rose.position - pos;
            Vector2 toCenter = (Vector2)transform.position - pos;
            Vector2 camCen = pos + toCenter + toChar;

            transform.position = new Vector3(camCen.x, camCen.y, _originalZ);

            float halfWidth = _thisCamera.orthographicSize * _thisCamera.aspect;

            float cameraWordLeft = _thisCamera.transform.position.x - halfWidth;
            float cameraWorldRight = _thisCamera.transform.position.x + halfWidth;

            if (cameraWordLeft < leftWall.transform.position.x)
                _thisCamera.transform.position = new Vector3(leftWall.transform.position.x + halfWidth,
                    _thisCamera.transform.position.y,
                    _thisCamera.transform.position.z);

            if(cameraWorldRight > rightWall.transform.position.x)
                _thisCamera.transform.position = new Vector3(rightWall.transform.position.x - halfWidth,
                    _thisCamera.transform.position.y,
                    _thisCamera.transform.position.z);

        }
    }
}
