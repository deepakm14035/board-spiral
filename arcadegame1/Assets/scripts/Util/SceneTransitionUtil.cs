using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransitionUtil
{

    public static void fadeObjects(MaskableGraphic[] objects, float time, float endAlpha)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i])
            {
                objects[i].GetComponent<MaskableGraphic>().CrossFadeAlpha(endAlpha, time, true);
            }
        }
    }
}
