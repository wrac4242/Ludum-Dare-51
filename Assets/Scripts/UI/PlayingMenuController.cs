using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayingMenuController : MonoBehaviour, IUiController
{
    bool isActive = false;
    UIController controller;
    [SerializeField]
    Image timeBar;
    [SerializeField]
    TextMeshProUGUI scoreBar;
    float currentTime = 0;
    int currentPoints = 0;
    public void Activate(UIController contr) {
        Debug.Log("Playing UI activated");
        controller = contr;
        gameObject.SetActive(true);
        isActive = true;
    }
	public void End() {
        isActive = false;
        gameObject.SetActive(false);
    }

    void Update() {
        if (!isActive) return;
        timeBar.fillAmount = (10-currentTime) / 10;
        // update point counter
        scoreBar.text = "Score:\n" + currentPoints;
    }

    public void UpdateState(float time, int points) {
        if (!isActive) {
            Debug.LogError("Playing menu updated while not active");
            return;
        }
        currentTime = time;
        currentPoints = points;
    }
}
