using UnityEngine;

namespace Assets.Scripts
{

    public class Flower : MonoBehaviour
    {
        #region Editor Variables
        public AudioSource player;
        #endregion



        public void PlaySound()
        {
            player.gameObject.transform.parent = transform.parent;
            player.Play();
        }
    }
}
