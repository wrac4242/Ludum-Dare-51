using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour, IUiController
{
    bool isActive = false;
    UIController controller;
    public void Activate(UIController contr) {
        Debug.Log("Start UI activated");
        controller = contr;
        gameObject.SetActive(true);
        isActive = true;
    }
	public void End() {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void StartButtonPressed() {
        if (!isActive || controller == null) return;
        controller.StartGame();
        isActive = false;
    }
}
