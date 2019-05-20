using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TesttouchMenu : MonoBehaviour
{
    public static TesttouchMenu game;

    List<Transform> collision = new List<Transform>(5);
    List<Transform> toRemove = new List<Transform>();
    Vector2 touchPos;
    public Transform player1;
    CardManager manager1;
    public Transform player2;
    CardManager manager2;


    private void Awake()
    {
        game = this;
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

        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches;
            for (int i = 0; i < myTouches.Length; i++)
            {

                touchPos = Camera.main.ScreenToWorldPoint(myTouches[i].position);
                    switch (myTouches[i].phase)
                    {
                        case TouchPhase.Began:
                             if (null != Physics2D.OverlapPoint(touchPos) && Physics2D.OverlapPoint(touchPos).name != "Block")
                             {
                                 collision.Insert(i, Physics2D.OverlapPoint(touchPos).transform);
                                 if (collision[i].tag == "P1")
                                 {
                                    manager1.noHand.Remove(collision[i]);
                                    manager1.noHand.Add(collision[i]);
                                    manager1.hand.Remove(collision[i]);
                                 }
                                 else if (collision[i].tag == "P2")
                                 {
                                    manager2.noHand.Remove(collision[i]);
                                    manager2.noHand.Add(collision[i]);
                                    manager2.hand.Remove(collision[i]);
                                 }
                                collision[i].GetChild(0).GetComponent<SpriteRenderer>().sortingOrder++;
                        }
                        else
                        {
                            collision.Insert(i, null);
                        }
                             break;

                        case TouchPhase.Moved:

                        if (collision.Count> i)
                        {
                            if (collision[i] != null)
                            {
                                Vector2 deltaPos = Camera.main.ScreenToWorldPoint(myTouches[i].position - myTouches[i].deltaPosition);
                                collision[i].position = touchPos;
                                if (collision[i].tag == "P1")
                                {
                                    collision[i].position = new Vector3(Mathf.Clamp(collision[i].position.x, -2.5f, 2.5f), Mathf.Clamp(collision[i].position.y, 0, 5));
                                }
                                else if (collision[i].tag == "P2")
                                {
                                    collision[i].position = new Vector3(Mathf.Clamp(collision[i].position.x, -2.5f, 2.5f), Mathf.Clamp(collision[i].position.y, -5, 0));
                                }
                            }
                        }
                            break;

                        case TouchPhase.Ended:

                        if (collision.Count > i)
                        {
                            if (collision[i] != null)
                            {
                                float min1 = 100;
                                float min2 = 100;
                                int jmin1 = 0;
                                int jmin2 = 0;

                                if (collision[i].tag == "P1")
                                {
                                    if (collision[i].position.y < 2)
                                    {
                                    }
                                    else if (manager1.hand.Count != 0)
                                    {
                                        for (int j = 0; j < manager1.hand.Count; j++)
                                        {
                                            float min = (collision[i].position - manager1.hand[j].position).magnitude;
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
                                                manager1.hand.Insert(jmin1, collision[i]);
                                            else if (min1 > 1)
                                                manager1.hand.Insert(jmin2 + 1, collision[i]);
                                            else if (min2 > 1)
                                                manager1.hand.Insert(jmin1 + 1, collision[i]);
                                            else if (jmin1 < jmin2)
                                                manager1.hand.Insert(jmin1 + 1, collision[i]);
                                            else
                                                manager1.hand.Insert(jmin2 + 1, collision[i]);
                                            manager2.noHand.Remove(collision[i]);
                                        }
                                    }
                                    else
                                    {
                                        manager1.hand.Add(collision[i]);
                                        manager1.noHand.Remove(collision[i]);
                                    }

                                }
                                else if (collision[i].tag == "P2")
                                {
                                    if (collision[i].position.y > -2)
                                    {
                                    }
                                    else if (manager2.hand.Count != 0)
                                    {
                                        for (int j = 0; j < manager2.hand.Count; j++)
                                        {
                                            float min = (collision[i].position - manager2.hand[j].position).magnitude;
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
                                                manager2.hand.Insert(jmin1, collision[i]);
                                            else if (min1 > 1)
                                                manager2.hand.Insert(jmin2 + 1, collision[i]);
                                            else if (min2 > 1)
                                                manager2.hand.Insert(jmin1 + 1, collision[i]);
                                            else if (jmin1 < jmin2)
                                                manager2.hand.Insert(jmin1 + 1, collision[i]);
                                            else
                                                manager2.hand.Insert(jmin2 + 1, collision[i]);
                                            manager2.noHand.Remove(collision[i]);
                                        }
                                    }
                                    else
                                    {
                                        manager2.hand.Add(collision[i]);
                                        manager2.noHand.Remove(collision[i]);
                                    }
                                }
                                collision[i].GetChild(0).GetComponent<SpriteRenderer>().sortingOrder--;
                                toRemove.Add(collision[i]);
                            }
                            else
                                toRemove.Add(collision[i]);
                        }
                            break;
                        
                    }
                
            }
            foreach (var item in toRemove)
            {
                collision.Remove(item);
            }
            toRemove.Clear();
        }

    }

    public void Play()
    {
        if (manager1.noHand.Count == 1 && manager2.noHand.Count == 1)
        {
            if (manager1.noHand[0].GetComponent<CardMenu>().type == CardMenu.MENUCARDTYPE.Play && manager2.noHand[0].GetComponent<CardMenu>().type == CardMenu.MENUCARDTYPE.Play)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            if (manager1.noHand[0].GetComponent<CardMenu>().type == CardMenu.MENUCARDTYPE.Quit && manager2.noHand[0].GetComponent<CardMenu>().type == CardMenu.MENUCARDTYPE.Quit)
                Application.Quit();
        }
    }

}
