using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum State
    {
        Start, 
        Playing, 
        GameOver,
        Blank
    }

    [SerializeField]
    GameObject StartMenu;
    [SerializeField]
    GameObject PlayingMenu;
    [SerializeField]
    GameObject EndMenu;

    StartMenuController startMenuController;
    PlayingMenuController playingMenuController;
    EndMenuController endMenuController;

    public Controller controller;

    public void Initialize(Controller contr, State sta) {
        controller = contr;
        startMenuController = StartMenu.GetComponent<StartMenuController>();
        playingMenuController = PlayingMenu.GetComponent<PlayingMenuController>();
        endMenuController = EndMenu.GetComponent<EndMenuController>();

        startMenuController.End();
        playingMenuController.End();
        endMenuController.End();
        SetState(sta);
    }

    State currentState = State.Blank;
    IUiController currentStateController;
    public void SetState(State nextState) {
        if (currentStateController != null) {
            currentStateController.End();
        }
        currentState = nextState;
        switch (currentState)
        {
            case State.Start:
                currentStateController = startMenuController;
                break;
            case State.Playing:
                currentStateController = playingMenuController;
                break;
            case State.GameOver:
                currentStateController = endMenuController;
                break;
            case State.Blank:
                currentStateController = null;
                break;
            default:
                Debug.LogError("Unknown state passed");
                break;
        }
        if (currentStateController != null) {
            currentStateController.Activate(this);
        }
    }

    public void StartGame() {
        if (currentState != State.Playing || controller == null) {
            controller.StartGame();
        }
    }

    public void UpdatePlayState(float time, int points) {
        if (currentState != State.Playing) {
            Debug.LogError("Play state updated outside of playing");
            return;
        }
        playingMenuController.UpdateState(time, points);
    }

    public void setPoints(int points) {
        // pass down
        endMenuController.SetPoints(points);

    }
}
