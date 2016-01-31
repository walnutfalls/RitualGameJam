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
            };
        }

        private void Update()
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(targetCharPos.position);
            Vector2 toChar = (Vector2)rose.position - pos;
            Vector2 toCenter = (Vector2)transform.position - pos;
            Vector2 camCen = pos + toCenter + toChar;

            transform.position = new Vector3(camCen.x, camCen.y, _originalZ);
        }

        IEnumerator ShangeSize()
        {
            yield return new WaitForSeconds(2.0f);
            _thisCamera.orthographicSize += 2;
        }
    }
}
