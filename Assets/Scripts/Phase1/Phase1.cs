using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase1 : MonoBehaviour, IPhase
{

    bool isCurrentPhase = false;
    Controller controller;

    public GameObject cardInstance;
    List<GameObject> cardList;
    [SerializeField]
    float cardMultiplier = 0.5f;
    [SerializeField]
    float timeDelay = 1;
    int completed = 0;
    [SerializeField]
    [Range(0.1f, 0.99f)]
    float completedPercent = 0.8f;

    void Start()
    {
        
    }

    Phase1Card lastClicked = null;
    float nextClickTime = 0;

    void Update()
    {
        if (isCurrentPhase && Input.GetMouseButton(0) && Time.time > nextClickTime)
        {
            Collider2D[] GridCheck = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (GridCheck.Length != 0) {
                bool cardFound = false;
                foreach (var item in GridCheck)
                {
                    if (item.gameObject.tag == "Card" && !cardFound)
                    {
                        Phase1Card card = item.GetComponent<Phase1Card>();
                        cardFound = true;
                        if (!card.completed)
                        {
                            card.SetFlip(true);
                            
                            if (lastClicked == null || lastClicked == card) {
                                lastClicked = card;
                            } else {
                                if (card.type == lastClicked.type) {
                                    card.Complete();
                                    lastClicked.Complete();
                                    controller.IncreasePoints(10);
                                    completed++;
                                    lastClicked = null;
                                    controller.PlayEffect(AudioController.SoundEffect.Phase1Correct);
                                } else {
                                    StartCoroutine(unflip(timeDelay * 0.9f, lastClicked));
                                    StartCoroutine(unflip(timeDelay * 0.9f, card));
                                    nextClickTime = Time.time + timeDelay;
                                    lastClicked = null;
                                    controller.PlayEffect(AudioController.SoundEffect.Phase1Incorrect);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator unflip(float delay, Phase1Card card) {
        yield return new WaitForSeconds(delay);
        if (card == null) {
            Debug.Log("Last click is null");
        } else {
            card.SetFlip(false);
        }
    }

    public void StartPhase(Controller controllerIn) {
        Debug.Log("1 started");
        controller = controllerIn;
        isCurrentPhase = true;
        generateCards(controller.currentDifficulty);
        DisplayCards();
    }

    public bool EndPhase() {
        isCurrentPhase = false;
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
        bool successful = completed >= cardList.Count * completedPercent / 2 ? true : false;
        cardList = new List<GameObject>();
        return successful;
    }

    void generateCards(float difficulty) {
        cardList = new List<GameObject>();
        
        // difficulty is amount of pairs
        int cardCount = Mathf.RoundToInt(Mathf.Min(difficulty*cardMultiplier, 3));
        for (int i = 0; i < cardCount; i++)
        {
            // i is type count
            Color col = new Color((i * 3 / cardCount + Random.Range(0f, 0.2f))%1f, (i / cardCount + Random.Range(0f, 0.2f))%1f, (i * 2 / cardCount + Random.Range(0f, 0.2f))%1f);

            GameObject card = Instantiate(cardInstance, transform);
            card.GetComponent<Phase1Card>().Initialize(i, this, col);
            cardList.Add(card);
            GameObject card2 = Instantiate(cardInstance, transform);
            card2.GetComponent<Phase1Card>().Initialize(i, this, col);
            cardList.Add(card2);
        }

        cardList.Sort((a, b)=> 1 - 2 * Random.Range(0, 2));
    }

    void DisplayCards() {
        int maxX = 0;
        int maxY = 0;

        if (cardList.Count % 4 == 0) {
            maxY = 4;
        } else if (cardList.Count % 3 == 0) {
            maxY = 3;
        } else {
            // is divisible by 2
            maxY = 2;
        }
        maxX = cardList.Count / maxY;

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                cardList[x*maxY + y].transform.position = transform.position + new Vector3(x* 1.2f - maxX/2, y * 1.2f - maxY/2, 0);
            }
        }
    }
}
