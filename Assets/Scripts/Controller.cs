using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    IPhase currentPhase;
    [HideInInspector]
    public int phase = 0; // starts at phase 0 
    [SerializeField]
    GameObject phase1Prefab;
    [SerializeField]
    GameObject phase2Prefab;
    [SerializeField]
    GameObject phase3Prefab;

    IPhase phase1;
    IPhase phase2;
    IPhase phase3;

    public bool devMode = false;

    [SerializeField]
    int startingDifficulty = 1;
    [HideInInspector]
    public float currentDifficulty = 1;
    int pointCounter = 0;
    bool gameStarted = false;
    [SerializeField]
    float difficultyIncreaseRate = 0.5f;
    [SerializeField]
    UIController uiController;
    void Start()
    {
        uiController.Initialize(this, UIController.State.Start);
    }

    public void StartGame() {
        uiController.SetState(UIController.State.Playing);
        gameStarted = true;
        currentDifficulty = Mathf.Max(startingDifficulty, 1);
        phase1 = Instantiate(phase1Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<IPhase>();
        phase2 = Instantiate(phase2Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<IPhase>();
        phase3 = Instantiate(phase3Prefab, Vector3.zero, Quaternion.identity, transform).GetComponent<IPhase>();

        if (!devMode) {
            currentPhase = phase1;
            phase = 0;
        } else {
            // dev mode starting phase
            currentPhase = phase3;
        }
        currentPhase.StartPhase(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        }
        if ( gameStarted) UpdateGame(Time.deltaTime);
    }

    float time = 0f;
    void UpdateGame(float timeDiff) {
        if (!gameStarted) return;
        time += Time.deltaTime;

        uiController.UpdatePlayState(time, pointCounter);
        if (time >= 10 && !devMode)
        {
            time -= 10;
            currentDifficulty+= difficultyIncreaseRate;
            SwitchPhase();
        }

    }


    void SwitchPhase() {
        if (!gameStarted) return;
        phase = (++phase) % 3;

        if (currentPhase != null) {
            if (EndPhase()) {
                return; // end of game
            }
        }
        switch (phase)
        {
            case 0:
                currentPhase = phase1;
                break;
            case 1:
                currentPhase = phase2;
                break;
            case 2:
                currentPhase = phase3;
                break;
            default:
                Debug.LogError("Invalid phase");
                break;
        }
        currentPhase.StartPhase(this);
    }

    public bool EndPhase() {
        if (!gameStarted) return true;
        bool phaseResult = currentPhase.EndPhase();
        if (!phaseResult) {
            // clean up, game over
            gameStarted = false;
            Debug.Log("game over");
            phase1 = null;
            phase2 = null;
            phase3 = null;
            phase = 0;
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
            uiController.setPoints(pointCounter);
            uiController.SetState(UIController.State.GameOver);

            return true;
        } 
        return false;
    }

    public void IncreasePoints(int increaseBy) {
        pointCounter += increaseBy;
    }
}
