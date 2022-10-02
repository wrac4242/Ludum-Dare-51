using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndMenuController : MonoBehaviour, IUiController
{
    bool isActive = false;
    UIController controller;
    int currentPoints = 0;
    [SerializeField]
    TextMeshProUGUI scoreText;
    public void Activate(UIController contr) {
        Debug.Log("End UI activated");
        controller = contr;
        gameObject.SetActive(true);
        isActive = true;
        UpdateDisplay();
    }
	public void End() {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void SetPoints(int points) {
        currentPoints = points;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        scoreText.text = "Score:\n" + currentPoints;
    }

    public void PlayAgain() {
        if (!isActive || controller == null) return;
        controller.StartGame();
        isActive = false;
    }

    public void StartMenu() {
        if (!isActive || controller == null) return;
        controller.SetState(UIController.State.Start);
    }
}
