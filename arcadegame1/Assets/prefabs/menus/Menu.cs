using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public abstract class Menu<T> : Menu where T : Menu<T>
    {
        private static T instance;

        public static T Instance { get => instance; set => instance = value; }

        protected virtual void Awake()
        {
            //Debug.Log(gameObject.name);
            if (instance != null)
                Destroy(gameObject);
            else
                instance = (T)this;
        }
        protected virtual void OnDestroy()
        {
            instance = null;
        }


    }
    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        public virtual void OnBackPressed()
        {
            MenuManager menuManager = MenuManager.Instance;
            if (menuManager != null)
                menuManager.goBack();
        }

        public virtual void GoHome() {
            MenuManager menuManager = MenuManager.Instance;
            if (menuManager != null)
            {

            }
        }

    }
}