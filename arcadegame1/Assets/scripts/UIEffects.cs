using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    public Image objects;
    public Text messageObject;
    // Start is called before the first frame update
    void Start()
    {
        Fade(0f, 0f);
        messageObject.gameObject.SetActive(false);

    }

    void Fade(float targetVal,float time) {
        objects.gameObject.GetComponent<MaskableGraphic>().CrossFadeAlpha(targetVal,time,true);
    }

    IEnumerator loseAnim()
    {
        objects.gameObject.SetActive(true);
        Fade(0.7f,0.2f);
        yield return new WaitForSeconds(0.1f);
        Fade(0.1f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        Fade(0.7f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        Fade(0f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        Fade(0.7f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        messageObject.gameObject.SetActive(true);
    }
    public void playLoseAnimation() {
        objects.color = new Color32(173,9,9,180);
        messageObject.text = "YOU LOSE!";
        StartCoroutine(loseAnim());
    }
    public void playWinAnimation()
    {

        objects.color = new Color32(0, 96, 38, 180);
        messageObject.text = "WIN!";
        StartCoroutine(loseAnim());
    }
    public void disableObjects() {
        StartCoroutine(DisableAfterDelay());
    }
    IEnumerator DisableAfterDelay() {
        yield return new WaitForSeconds(3f);
        objects.gameObject.SetActive(false);
        messageObject.gameObject.SetActive(false);
    }
}
