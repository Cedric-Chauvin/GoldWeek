using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public float y = 4;
    public int nbCardInHand = 5;
    public List<Transform> hand;
    public List<Transform> noHand;
    public List<Transform> pointPair;
    public List<Transform> pointImpaire;
    public List<Transform> cards;
    private List<Transform> deck; 
    public float cardSpeed;

    private int nbCard;

    // Start is called before the first frame update
    void Start()
    {
        deck = new List<Transform>();
        nbCard = cards.Count;
        for (int i = 0; i < nbCard; i++)
        {
            int var = Random.Range(0, cards.Count);
            deck.Add(cards[var]);
            cards.Remove(cards[var]);
        }
        for(int i =0; i < nbCardInHand; i++)
        {
            Transform instance = Instantiate(deck[0]);
            instance.tag = transform.name;
            hand.Add(instance);
            deck.Remove(deck[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveCard();
        
    }





    void MoveCard()
    {
        Vector3 dir;
        switch (hand.Count)
        {
            case 1:
                dir  = (pointImpaire[2].position - hand[0].position);
                if (dir.magnitude > 0.5)
                    dir.Normalize();
                hand[0].Translate( dir * Time.deltaTime * cardSpeed);

                break;

            case 2:
                for(int i = 0; i < 2; i++)
                {
                    dir = (pointPair[i+1].position - hand[i].position);
                    if (dir.magnitude > 0.5)
                        dir.Normalize();
                    hand[i].Translate(dir * Time.deltaTime * cardSpeed);
                }

                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    dir = (pointImpaire[i + 1].position - hand[i].position);
                    if (dir.magnitude > 0.5)
                        dir.Normalize();
                    hand[i].Translate(dir * Time.deltaTime * cardSpeed);
                }

                break;

            case 4:
                for (int i = 0; i < 4; i++)
                {
                    dir = (pointPair[i].position - hand[i].position);
                    if (dir.magnitude > 0.5)
                        dir.Normalize();
                    hand[i].Translate(dir * Time.deltaTime * cardSpeed);
                }

                break;

            case 5:
                for (int i = 0; i < 5; i++)
                {
                    dir = (pointImpaire[i].position - hand[i].position);
                    if (dir.magnitude > 0.5)
                        dir.Normalize();
                    hand[i].Translate(dir * Time.deltaTime * cardSpeed);
                }

                break;
        }

    }

    public void PlayCard()
    {
        List<Transform> list = new List<Transform>();
        foreach (var item in noHand)
        {
            if (transform.name == "P1")
            {
                if (item.position.y < 2)
                {
                    list.Add(item);
                }
                    
            }
            else
            {
                if (item.position.y > -2)
                {
                    list.Add(item);
                }  
            }
        }
        if (list.Count != 1)
        {
            foreach (var item in noHand)
            {
                hand.Add(item);
                if(transform.name == "P1")
                {
                    if (item.transform.position.y < 2)
                    {
                        item.GetComponent<Card>().PlayVisuNeg();
                    }
                }
                else
                {
                    if (item.transform.position.y > -2)
                    {
                        item.GetComponent<Card>().PlayVisuNeg();
                    }
                }
            }
            noHand.Clear();
            int random = Random.Range(0, hand.Count);
            hand[random].GetComponent<Card>().PlayCard();
            Destroy(hand[random].gameObject);
            hand.RemoveAt(random);

        }
        else
        {
            list[0].GetComponent<Card>().PlayVisuNeg();
            list[0].GetComponent<Card>().PlayCard();
            Destroy(list[0].gameObject);
            noHand.Remove(list[0]);
        }
        if (deck.Count != 0)
        {
            Transform instance = Instantiate(deck[0]);
            instance.tag = transform.name;
            hand.Add(instance);
            deck.Remove(deck[0]);
        }

    }
}
