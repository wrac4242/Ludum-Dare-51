using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase3Tiles : MonoBehaviour
{
    [SerializeField]
    Color safeCol;
    [SerializeField]
    Color badCol;

    [HideInInspector]
    Phase3 phase3;
    new SpriteRenderer renderer;
    public void Initialize(Phase3 a) {
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = safeCol;
        phase3 = a;
    }
    bool exists = true;
    void OnDestroy()
    {
        exists = false;
    }
    public IEnumerator MakeBad(float delay) {
        renderer.color = badCol;
        yield return new WaitForSeconds(delay);
        if (renderer == null || !exists) {
            yield break;
        }
        renderer.color = safeCol;
        if (playerOverlap()) {
            phase3.KillPlayer();
        }
    }

    bool playerOverlap() {
        Collider2D[] playerCheck = Physics2D.OverlapPointAll(transform.position);
        foreach (var item in playerCheck)
        {
            if (item.gameObject.tag == "Player") {
                return true;
            } 
        }
        return false;
    }
}
