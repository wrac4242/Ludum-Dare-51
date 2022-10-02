using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2Car : MonoBehaviour
{
    new SpriteRenderer renderer;
    [SerializeField]
    Color selectedCol;
    [SerializeField]
    Color completedCol;
    public Color normalCol = new Color(1, 1, 1, 1f);
    bool isActive = false;
    bool isCompleted = false;
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        normalCol = renderer.color;
    }


    public bool Move(Vector2 moveDir) {

        // check next position for grid, must contain Grid and cannot contain Car
        Vector2 moveLocation = (Vector2) transform.position + moveDir;

        Collider2D[] GridCheck = Physics2D.OverlapPointAll(moveLocation);
        if (GridCheck.Length == 0) {
            return false;
        }
        bool isGridded = false;
        foreach (var item in GridCheck)
        {
            if (item.gameObject.tag == "Objective") {
                if (item.gameObject.GetComponent<Phase2Objective>().id == id) {
                    GameObject.Destroy(item.gameObject);
                    isCompleted = true;
                    transform.position = moveLocation;
                    renderer.color = completedCol;
                    return true;
                }
            } else if (item.gameObject.tag == "Car") {
                return false;
            } else if (item.gameObject.tag == "Grid") {
                isGridded = true;
            }
        }

        if (isGridded) {
            transform.position = moveLocation;
        } 
        return false;
    }

    public void IsActive(bool active) {
        if (isCompleted) return;
        isActive = active;

        if (isActive) {
            renderer.color = selectedCol;
        } else {
            renderer.color = normalCol;
        }
    }
}
