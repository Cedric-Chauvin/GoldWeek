using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testtouch : MonoBehaviour
{
    public static Testtouch game;
    public Image barHumain;
    public Image barNature;
    public Image visuBarHumain;
    public Image visuBarNature;
    List<Collider2D> collision = new List<Collider2D>(5);
    List<Collider2D> toRemove = new List<Collider2D>();
    Vector2 touchPos;
    public Transform player1;
    CardManager manager1;
    public Transform player2;
    CardManager manager2;

    private float maxNature;
    public float nature;
    private float maxHumain;
    public float humain;
    public float visuHumain;
    public float visuNature;

    private void Awake()
    {
        game = this;
        maxNature = nature;
        maxHumain = humain;
        nature = 0;
        humain = 0;
        visuNature = 0;
        visuHumain = 0;
    }


    // Start is called before the first frame update
    void Start()
    {
        manager1 = player1.GetComponent<CardManager>();
        manager2 = player2.GetComponent<CardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        barHumain.fillAmount = (humain+maxHumain) / (maxHumain*2);
        visuBarHumain.fillAmount = (visuHumain+maxHumain) / (maxHumain*2);
        barNature.fillAmount = (nature + maxNature) / (maxNature * 2);
        visuBarNature.fillAmount = (visuNature + maxNature) / (maxNature * 2);


        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches;
            for (int i = 0; i < myTouches.Length; i++)
            {

                touchPos = Camera.main.ScreenToWorldPoint(myTouches[i].position);
                Collider2D test = Physics2D.OverlapPoint(touchPos);
                if (null != Physics2D.OverlapPoint(touchPos) && Physics2D.OverlapPoint(touchPos).name != "Block")
                {
                    switch (myTouches[i].phase)
                    {
                        case TouchPhase.Began:
                            collision.Insert(i, Physics2D.OverlapPoint(touchPos));
                            if (collision[i].tag == "P1")
                            {
                                if (collision[i].transform.position.y < 2)
                                {
                                    collision[i].GetComponent<Card>().PlayVisuNeg();
                                }
                                manager1.noHand.Remove(collision[i].transform);
                                manager1.noHand.Add(collision[i].transform);
                                manager1.hand.Remove(collision[i].transform);
                            }
                            else if (collision[i].tag == "P2")
                            {
                                if (collision[i].transform.position.y > -2)
                                {
                                    collision[i].GetComponent<Card>().PlayVisuNeg();
                                }
                                manager2.noHand.Remove(collision[i].transform);
                                manager2.noHand.Add(collision[i].transform);
                                manager2.hand.Remove(collision[i].transform);
                            }
                            collision[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;

                            break;

                        case TouchPhase.Moved:


                            Vector2 deltaPos = Camera.main.ScreenToWorldPoint(myTouches[i].position - myTouches[i].deltaPosition);
                            collision[i].transform.position = touchPos;
                            if (collision[i].tag == "P1")
                            {
                                collision[i].transform.position = new Vector3(Mathf.Clamp(collision[i].transform.position.x, -2.5f, 2.5f), Mathf.Clamp(collision[i].transform.position.y, 0, 5));
                            }
                            else if (collision[i].tag == "P2")
                            {
                                collision[i].transform.position = new Vector3(Mathf.Clamp(collision[i].transform.position.x, -2.5f, 2.5f), Mathf.Clamp(collision[i].transform.position.y, -5, 0));
                            }

                            break;

                        case TouchPhase.Ended:


                            float min1 = 100;
                            float min2 = 100;
                            int jmin1 = 0;
                            int jmin2 = 0;

                            if (collision[i].tag == "P1")
                            {
                                if (collision[i].transform.position.y < 2)
                                {
                                    collision[i].GetComponent<Card>().PlayVisuPos();
                                }
                                else if (manager2.hand.Count != 0)
                                {
                                    for (int j = 0; j < manager1.hand.Count; j++)
                                    {
                                        float min = (collision[i].transform.position - manager1.hand[j].position).magnitude;
                                        if (min < min1 && j % 2 == 0)
                                        {
                                            min1 = min;
                                            jmin1 = j;
                                        }
                                        else if (min < min2)
                                        {
                                            min2 = min;
                                            jmin2 = j;
                                        }

                                    }
                                    if (min1 < 1 || min2 < 1)
                                    {
                                        if (min2 > 1 && jmin1 == 0)
                                            manager1.hand.Insert(jmin1, collision[i].transform);
                                        else if (min1 > 1)
                                            manager1.hand.Insert(jmin2 + 1, collision[i].transform);
                                        else if (min2 > 1)
                                            manager1.hand.Insert(jmin1 + 1, collision[i].transform);
                                        else if (jmin1 < jmin2)
                                            manager1.hand.Insert(jmin1 + 1, collision[i].transform);
                                        else
                                            manager1.hand.Insert(jmin2 + 1, collision[i].transform);
                                        manager2.noHand.Remove(collision[i].transform);
                                    }
                                }
                                else
                                {
                                    manager2.hand.Add(collision[i].transform);
                                    manager2.noHand.Remove(collision[i].transform);
                                }

                            }
                            else if (collision[i].tag == "P2")
                            {
                                if (collision[i].transform.position.y > -2)
                                {
                                    collision[i].GetComponent<Card>().PlayVisuPos();
                                }else if (manager2.hand.Count != 0)
                                {
                                    for (int j = 0; j < manager2.hand.Count; j++)
                                    {
                                        float min = (collision[i].transform.position - manager2.hand[j].position).magnitude;
                                        if (min < min1 && j % 2 == 0)
                                        {
                                            min1 = min;
                                            jmin1 = j;
                                        }
                                        else if (min < min2)
                                        {
                                            min2 = min;
                                            jmin2 = j;
                                        }

                                    }
                                    if (min1 < 1 || min2 < 1)
                                    {
                                        if (min2 > 1 && jmin1 == 0)
                                            manager2.hand.Insert(jmin1, collision[i].transform);
                                        else if (min1 > 1)
                                            manager2.hand.Insert(jmin2 + 1, collision[i].transform);
                                        else if (min2 > 1)
                                            manager2.hand.Insert(jmin1 + 1, collision[i].transform);
                                        else if (jmin1 < jmin2)
                                            manager2.hand.Insert(jmin1 + 1, collision[i].transform);
                                        else
                                            manager2.hand.Insert(jmin2 + 1, collision[i].transform);
                                        manager2.noHand.Remove(collision[i].transform);
                                    }
                                }
                                else
                                {
                                    manager2.hand.Add(collision[i].transform);
                                    manager2.noHand.Remove(collision[i].transform);
                                }
                            }
                            collision[i].transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder--;
                            toRemove.Add(collision[i]);
                            break;

                    }
                }
            }
            foreach (var item in toRemove)
            {
                collision.Remove(item);
            }
            toRemove.Clear();
        }

    }

    public void PlayCard()
    {
        manager1.PlayCard();
        manager2.PlayCard();
    }

}
