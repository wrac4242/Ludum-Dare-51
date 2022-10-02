using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase3 : MonoBehaviour, IPhase
{
    bool isCurrentPhase = false;
    Controller controller;

    [SerializeField]
    GameObject phase3Grid;
    [SerializeField]
    GameObject playerPrefab;
    int xSize = 0, ySize = 0;
    bool hasDied = false;
    Phase3Tiles[] gridTiles;
    Phase3Player player;
    [SerializeField]
    float startingPointDelay = 1f;
    float pointDelay = 1;
    public void StartPhase(Controller controllerIn) {
        Debug.Log("3 started");
        controller = controllerIn;
        isCurrentPhase = true;
        hasDied = false;
        pointDelay = startingPointDelay / (controller.currentDifficulty / 3f) ;
        xSize = Mathf.RoundToInt(Mathf.Max(Mathf.Min(controller.currentDifficulty, 15), 3));
        ySize = Mathf.RoundToInt(Mathf.Max(Mathf.Min(controller.currentDifficulty, 10), 3));
        GenerateGreyGrid();

        // spawn player
        GameObject playerObj = Instantiate(playerPrefab, new Vector3(- xSize/2, - ySize/2, 0), Quaternion.identity, transform);
        player = playerObj.GetComponent<Phase3Player>();
    }
    public bool EndPhase() {
        isCurrentPhase = false;
        player = null;
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        gridTiles = null;

        return !hasDied;
    }

    float changeTime = 0;
    int lastIndex = 0;
    void Update() {
        if (isCurrentPhase )
        {
            changeTime += Time.deltaTime;
            if (changeTime >= pointDelay) {
                controller.IncreasePoints(5);
                changeTime -= pointDelay;


                int tileIndex = Random.Range(1, xSize*ySize);
                if (lastIndex == tileIndex) tileIndex = 0;
                lastIndex = tileIndex;
                StartCoroutine(gridTiles[tileIndex].MakeBad(pointDelay));
                

            }
        }
    }

    void GenerateGreyGrid() {
        gridTiles = new Phase3Tiles[xSize * ySize];
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject obj = Instantiate(phase3Grid, new Vector3(x* 1.2f - xSize/2, y * 1.2f - ySize/2, 0), Quaternion.identity, transform);
                gridTiles[x*ySize + y] = obj.GetComponent<Phase3Tiles>();
                gridTiles[x*ySize + y].Initialize(this);
            }
        }
    }

    public void KillPlayer() {
        if (!isCurrentPhase) {
            return;
        }
        GameObject.Destroy(player.gameObject);
        hasDied = true;
        controller.EndPhase();
    }
}
