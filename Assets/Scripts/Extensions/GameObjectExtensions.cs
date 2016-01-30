
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        //thanks to darbotron: http://answers.unity3d.com/questions/555101/possible-to-make-gameobjectgetcomponentinchildren.html
        ///////////////////////////////////////////////////////////
        // Essentially a reimplementation of 
        // GameObject.GetComponentInChildren< T >()
        // Major difference is that this DOES NOT skip deactivated game objects
        ///////////////////////////////////////////////////////////        
        public static T GetComponentInChildrenWithInactive<T>(this GameObject objRoot) where T : Component
        {
            // if we don't find the component in this object 
            // recursively iterate children until we do
            T tRetComponent = objRoot.GetComponent<T>();

            if (null == tRetComponent)
            {
                // transform is what makes the hierarchy of GameObjects, so 
                // need to access it to iterate children
                Transform trnsRoot = objRoot.transform;
                int iNumChildren = trnsRoot.childCount;

                // could have used foreach(), but it causes GC churn
                for (int iChild = 0; iChild < iNumChildren; ++iChild)
                {
                    // recursive call to this function for each child
                    // break out of the loop and return as soon as we find 
                    // a component of the specified type
                    tRetComponent = GetComponentInChildrenWithInactive<T>(trnsRoot.GetChild(iChild).gameObject);
                    if (null != tRetComponent)
                    {
                        break;
                    }
                }
            }

            return tRetComponent;
        }
    }
}
