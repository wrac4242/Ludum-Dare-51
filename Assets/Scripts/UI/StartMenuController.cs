using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour, IUiController
{
    bool isActive = false;
    UIController controller;
    [SerializeField]
    GameObject main;
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    GameObject credit;
    public void Activate(UIController contr) {
        Debug.Log("Start UI activated");
        controller = contr;
        gameObject.SetActive(true);
        isActive = true;

        MainScreen();
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

    public void TutorialScreen() {
        controller.controller.PlayEffect(AudioController.SoundEffect.ButtonClick);
        main.SetActive(false);
        tutorial.SetActive(true);
        credit.SetActive(false);
    }

    public void CreditScreen() {
        controller.controller.PlayEffect(AudioController.SoundEffect.ButtonClick);
        main.SetActive(false);
        tutorial.SetActive(false);
        credit.SetActive(true);
    }

    public void MainScreen() {
        controller.controller.PlayEffect(AudioController.SoundEffect.ButtonClick);
        main.SetActive(true);
        tutorial.SetActive(false);
        credit.SetActive(false);
    }
}
