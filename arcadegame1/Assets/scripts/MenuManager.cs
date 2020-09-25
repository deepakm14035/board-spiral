using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MenuManagement
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _winMenu;
        [SerializeField]
        private GameObject _loseMenu;
        [SerializeField]
        private GameObject _mainMenu;
        [SerializeField]
        private GameObject _gameMenu;
        [SerializeField]
        private GameObject _pauseMenu;
        [SerializeField]
        private GameObject _levelSelecterMenu;

        [SerializeField]
        private GameObject _loadingScreen;


        private static MenuManager _instance;
        private Stack<Menu> menuStack;

        public static MenuManager Instance { get => _instance; set => _instance = value; }

        protected virtual void Awake()
        {
            //Debug.Log(gameObject.name);
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
        }
        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            menuStack = new Stack<Menu>();
            GameObject menu1 = Instantiate(_mainMenu, transform.forward,Quaternion.identity);
            GameObject menu2 = Instantiate(_loseMenu, transform.forward, Quaternion.identity);
            GameObject menu3 = Instantiate(_winMenu, transform.forward, Quaternion.identity);
            GameObject menu4 = Instantiate(_gameMenu, transform.forward, Quaternion.identity);
            GameObject menu5 = Instantiate(_pauseMenu, transform.forward, Quaternion.identity);
            GameObject menu6 = Instantiate(_levelSelecterMenu, transform.forward, Quaternion.identity);

            menu2.SetActive(false);
            menu3.SetActive(false);
            menu4.SetActive(false);
            menu5.SetActive(false);
            menu6.SetActive(false);

            menuStack.Push(menu1.GetComponent<Menu>());
        }

        public void loadMenu(Menu menu) {
            menuStack.Peek().gameObject.SetActive(false);
            menuStack.Push(menu);
            menu.gameObject.SetActive(true);

        }

        public void loadMenu(Menu menu, float delay, bool isLoadingRequired)
        {
            StartCoroutine(loadAfterDelay(menu, delay, isLoadingRequired));
        }

        IEnumerator loadAfterDelay(Menu menu, float delay, bool isLoadingRequired) {
            GameObject loadingScreen=new GameObject();
            if (isLoadingRequired)
            {
                GameObject.Destroy(loadingScreen);
                loadingScreen = null;
                loadingScreen = Instantiate(_loadingScreen, Vector3.zero, Quaternion.identity);
                yield return null;
            }
            Debug.Log("loadingsc");
            SceneTransitionUtil.fadeObjects(menuStack.Peek().gameObject.GetComponentsInChildren<MaskableGraphic>(),0.5f,0f);
            yield return new WaitForSeconds(delay);
            SceneTransitionUtil.fadeObjects(menuStack.Peek().gameObject.GetComponentsInChildren<MaskableGraphic>(), 0f, 1f);
            menuStack.Peek().gameObject.SetActive(false);
            menuStack.Push(menu);
            menu.gameObject.SetActive(true);
            Debug.Log("loadingsc des");
            GameObject.Destroy(loadingScreen);
        }

        

        public void goBack(bool delayRequired)
        {
            if (delayRequired)
                StartCoroutine(LoadAndGoBack());
            else
            {
                menuStack.Peek().gameObject.SetActive(false);
                menuStack.Pop();
                menuStack.Peek().gameObject.SetActive(true);
            }
        }

        IEnumerator LoadAndGoBack()
        {
            GameObject loadingScreen = Instantiate(_loadingScreen, Vector3.zero, Quaternion.identity);
            yield return null;
            SceneTransitionUtil.fadeObjects(menuStack.Peek().gameObject.GetComponentsInChildren<MaskableGraphic>(), 0.5f, 0f);
            yield return new WaitForSeconds(1.2f);
            SceneTransitionUtil.fadeObjects(menuStack.Peek().gameObject.GetComponentsInChildren<MaskableGraphic>(), 0f, 1f);
            menuStack.Peek().gameObject.SetActive(false);
            menuStack.Pop();
            menuStack.Peek().gameObject.SetActive(true);
            GameObject.Destroy(loadingScreen);
        }
    }
}