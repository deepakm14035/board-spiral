using MenuManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu<PauseMenu>
{
    public void onResume() {
        Time.timeScale = 1f;
        base.OnBackPressed();
    }
}
