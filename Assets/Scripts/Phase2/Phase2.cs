using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2 : MonoBehaviour, IPhase
{
    bool isCurrentPhase = false;
    Controller controller;
    [SerializeField]
    float difficultyMultiplier = 1f;
    [SerializeField]
    GameObject greyGridPrefab;
    [SerializeField]
    GameObject carPrefab;
    [SerializeField]
    GameObject objectivePrefab;
    int xSize = 0, ySize = 0;
    int carCount = 0;
    int reachedObjective = 0;

    [SerializeField]
    [Range(0.1f, 0.99f)]
    float completedPercent = 0.8f;
    Phase2Car currentCar;
    public void StartPhase(Controller controllerIn) {
        Debug.Log("2 started");
        controller = controllerIn;
        isCurrentPhase = true;
        xSize = Mathf.RoundToInt(Mathf.Max(Mathf.Min(controller.currentDifficulty, 15), 4));
        ySize = Mathf.RoundToInt(Mathf.Max(Mathf.Min(controller.currentDifficulty, 10), 4));
        GenerateGreyGrid();
        SpawnCarsAndObjectives(controller.currentDifficulty);
        currentCar = null;
    }
    public bool EndPhase() {
        isCurrentPhase = false;
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        xSize = 0;
        ySize = 0;

        currentCar = null;

        bool wasSuccessful = carCount * completedPercent <= reachedObjective;
        carCount = 0;
        reachedObjective = 0;

        return wasSuccessful;
    }

    [SerializeField]
    float movementDelay = 0.5f;
    float movementTime = 0;
    Vector2 toMove;
    void Update()
    {
        if (isCurrentPhase && Input.GetMouseButton(0))
        {

            Collider2D[] GridCheck = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (GridCheck.Length != 0) {
                bool carFound = false;
                foreach (var item in GridCheck)
                {
                    if (item.gameObject.tag == "Car" && !carFound)
                    {
                        Phase2Car car = item.GetComponent<Phase2Car>();
                        carFound = true;
                        if (currentCar != null) {
                            currentCar.IsActive(false);
                        }
                        car.IsActive(true);
                        currentCar = car;
                    }
                }
            }
        }

        movementTime += Time.time / 1000;

        // out of if to allow queueing of actions almost
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        if (vertical != 0) {
            toMove = new Vector2(0, vertical);
        } else {
            toMove = new Vector2(horizontal, 0);
        }

        if (currentCar != null && movementTime >= movementDelay) {

            if (currentCar.Move(toMove * 1.2f)) {
                reachedObjective++;
                controller.IncreasePoints(15);
                currentCar = null;
            }
            movementTime -= movementDelay;
        }
    }

    void SpawnCarsAndObjectives(float difficulty) {
        for (int i = 0; i < Mathf.Min(difficulty*difficultyMultiplier, xSize * ySize -1); i++)
        {
            carCount++;
            Color col = new Color(Random.Range(0f,1f), Random.Range(0f,1f), Random.Range(0f,1f), 1f);
            bool placing = true;
            Vector3 intendedPos = Vector3.zero;
            while (placing) {
                intendedPos = new Vector3(Random.Range(0, xSize)* 1.2f - xSize/2, Random.Range(0, ySize) * 1.2f - ySize/2, 0);
                Collider2D[] GridCheck = Physics2D.OverlapPointAll(intendedPos + transform.position);
                placing = false;
                bool gridCheck = false;
                foreach (var item in GridCheck)
                {
                    if (item.gameObject.tag == "Car" || item.gameObject.tag == "Objective") {
                        placing = true;
                    }
                    if (item.gameObject.tag == "Grid") {
                        gridCheck = true;
                    }
                }
                if (!gridCheck) {
                    placing = true;
                }
            }
            var car = Instantiate(carPrefab, intendedPos, Quaternion.identity, transform);
            car.GetComponent<SpriteRenderer>().color = col;
            car.GetComponent<Phase2Car>().id = i;

            placing = true;
            intendedPos = Vector3.zero;
            while (placing) {
                intendedPos =  new Vector3(Random.Range(0, xSize)* 1.2f - xSize/2, Random.Range(0, ySize) * 1.2f - ySize/2, 0);
                Collider2D[] GridCheck = Physics2D.OverlapPointAll(transform.position +intendedPos);
                placing = false;
                bool gridCheck = false;
                foreach (var item in GridCheck)
                {
                    if (item.gameObject.tag == "Car" || item.gameObject.tag == "Objective") {
                        placing = true;
                    }
                    if (item.gameObject.tag == "Grid") {
                        gridCheck = true;
                    }
                }
                if (!gridCheck) {
                    placing = true;
                }
            }
            var obj = Instantiate(objectivePrefab, intendedPos, Quaternion.identity, transform);
            obj.GetComponent<SpriteRenderer>().color = col;
            obj.GetComponent<Phase2Objective>().id = i;
        }
    }

    void GenerateGreyGrid() {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Instantiate(greyGridPrefab, new Vector3(x* 1.2f - xSize/2, y * 1.2f - ySize/2, 0), Quaternion.identity, transform);
            }
        }
    }
}
