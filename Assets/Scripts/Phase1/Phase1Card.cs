using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1Card : MonoBehaviour
{
    
    public int type;
    public bool completed;
    public bool flipped;

    new SpriteRenderer renderer;

    [SerializeField]
    Color flippedCol;
    Color normalCol;

    public void Initialize(int typeIn, Phase1 controller, Color col) {
        type = typeIn;

        renderer = GetComponent<SpriteRenderer>();

        // set colour
        normalCol = col;
        renderer.color = flippedCol;
    }

    public void SetFlip(bool flippedIn ) {
        flipped = flippedIn;

        // change card colour to what it is now
        if (flipped)
        {
            renderer.color = normalCol;
        } else {
            renderer.color = flippedCol;
        }
    }

    public void Complete() {
        completed = true;
    }
}
