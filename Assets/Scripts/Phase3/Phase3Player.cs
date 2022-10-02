using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase3Player : MonoBehaviour
{
    [SerializeField]
    float movementDelay = 0.8f;
    float movementTime = 0;
    Vector2 toMove;
    void Update()
    {
        movementTime += Time.time / 1000;

        // out of if to allow queueing of actions almost
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        if (vertical != 0) {
            toMove = new Vector2(0, vertical);
        } else {
            toMove = new Vector2(horizontal, 0);
        }

        if (movementTime >= movementDelay) {

            Move(toMove * 1.2f);
            movementTime -= movementDelay;
        }
    }
    public void Move(Vector2 moveDir) {
        // check next position for grid, must contain Grid and cannot contain Car
        Vector2 moveLocation = (Vector2) transform.position + moveDir;

        Collider2D[] GridCheck = Physics2D.OverlapPointAll(moveLocation);
        if (GridCheck.Length == 0) {
            return;
        }
        bool isGridded = false;
        foreach (var item in GridCheck)
        {
            if (item.gameObject.tag == "Grid") {
                isGridded = true;
            }
        }
        if (isGridded) {
            transform.position = moveLocation;
        } 
        return;
    }
}
