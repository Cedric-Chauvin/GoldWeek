using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testtouch : MonoBehaviour
{
    enum SHIELDSTATE
    {
        NONE,
        WAIT,
        SHIELD
    }

    enum HEALSTATE
    {
        NONE,
        WAIT,
        T2,
        T3,
        T4
    }

    enum BARSTATE
    {
        NONE,
        HAUTE,
        MEDIUM,
        BASSE
    }

    SHIELDSTATE shielNature = SHIELDSTATE.NONE;
    SHIELDSTATE shielHumain = SHIELDSTATE.NONE;
    HEALSTATE healNature = HEALSTATE.NONE;
    HEALSTATE healHumain = HEALSTATE.NONE;
    BARSTATE natureState = BARSTATE.NONE;
    BARSTATE humainState = BARSTATE.NONE;
    public static Testtouch game;
    public Image barHumain;
    public Image barNature;
    public Image visuBarHumain;
    public Image visuBarNature;
    List<Transform> collision = new List<Transform>(5);
    List<Transform> toRemove = new List<Transform>();
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

    private bool isTimerCard;
    private float timerCard;
    public float tempsPLay;


    public Text text;

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
        SoundControler._soundControler.PlaySound(SoundControler._soundControler._pioche);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCard();
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
                    switch (myTouches[i].phase)
                    {
                        case TouchPhase.Began:
                             if (null != Physics2D.OverlapPoint(touchPos) && Physics2D.OverlapPoint(touchPos).name != "Block")
                             {
                                 collision.Insert(i, Physics2D.OverlapPoint(touchPos).transform);
                                 if (collision[i].tag == "P1")
                                 {
                                    if (collision[i].position.y < 2)
                                    {
                                    collision[i].GetComponent<Card>().PlayVisuNeg();
                                    }
                                    manager1.noHand.Remove(collision[i]);
                                    manager1.noHand.Add(collision[i]);
                                    manager1.hand.Remove(collision[i]);
                                 }
                                 else if (collision[i].tag == "P2")
                                 {
                                    if (collision[i].position.y > -2)
                                    {
                                        collision[i].GetComponent<Card>().PlayVisuNeg();
                                    }
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

                                SoundControler._soundControler.PlaySound(SoundControler._soundControler._poseCard);

                                if (collision[i].tag == "P1")
                                {
                                    if (collision[i].position.y < 2)
                                    {
                                        collision[i].GetComponent<Card>().PlayVisuPos();
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
                                        collision[i].GetComponent<Card>().PlayVisuPos();
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

    public void PlayCard()
    {
        isTimerCard = true;
        timerCard = tempsPLay;
    }

    public void RemoveCard(Transform collider,bool ispreview)
    {
        int index = collision.IndexOf(collider);
        if (index >= 0)
        {
            if (ispreview)
            {
                collider.GetComponent<Card>().PlayVisuPos();
            }
            collision[index] = null;
        }
    }

    private void UpdateCard()
    {
        if (isTimerCard && timerCard <0)
        {
            manager1.PlayCard();
            manager2.PlayCard();
            isTimerCard = false;
            CalculVictory();
            text.text = "Play";
            if(shielHumain != SHIELDSTATE.NONE)
                shielHumain++;
            if (shielHumain == (SHIELDSTATE)3)
                shielHumain = SHIELDSTATE.NONE;
            if (shielNature != SHIELDSTATE.NONE)
                shielNature++;
            if (shielNature == (SHIELDSTATE)3)
                shielNature = SHIELDSTATE.NONE;
            if (healHumain != HEALSTATE.NONE)
            {
                if (healHumain != HEALSTATE.WAIT)
                {
                    humain++;
                    visuHumain++;
                }
                healHumain++;
                if (healHumain == (HEALSTATE)5)
                    healHumain = HEALSTATE.NONE;
            }
            if (healNature != HEALSTATE.NONE)
            {
                if (healNature != HEALSTATE.WAIT)
                {
                    nature++;
                    visuNature++;
                }
                healNature++;
                if (healNature == (HEALSTATE)5)
                    healNature = HEALSTATE.NONE;
            }
        }
        else if(isTimerCard)
        {
            timerCard-=Time.deltaTime;
            text.text = timerCard.ToString();
        }
    }




    private void CalculVictory()
    {
        if (nature > 5)
        {
            if (natureState == BARSTATE.HAUTE)
                ;//victoire nature
            else
                natureState = BARSTATE.HAUTE;
        }
        else if (nature > -4)
        {
            natureState = BARSTATE.NONE;
        }
        else if (nature > -6)
        {
            if (natureState == BARSTATE.MEDIUM)
                ;//victoire humain
            else
                natureState = BARSTATE.MEDIUM;
        }
        else
        {
            if (natureState == BARSTATE.BASSE)
                ;//defaite all
            else
                natureState = BARSTATE.BASSE;
        }

        if (humain > 4)
        {
            if (humainState == BARSTATE.HAUTE)
                ;//victoire humain
            else
                humainState = BARSTATE.HAUTE;
        }
        else if (humain > -4)
        {
            humainState = BARSTATE.NONE;
        }
        else if (humain > -6)
        {
            if (humainState == BARSTATE.MEDIUM)
                ;//victoire nature
            else
                humainState = BARSTATE.MEDIUM;
        }
        else
        {
            if (humainState == BARSTATE.BASSE)
                ;//defaite all
            else
                humainState = BARSTATE.BASSE;
        }
    }

    public void ApplyShield(string tag)
    {
        if (tag == "P1")
            shielNature = SHIELDSTATE.WAIT;
        else
            shielHumain = SHIELDSTATE.WAIT;
    }

    public void ApplyHeal(string tag)
    {
        if (tag == "P1")
            healNature = HEALSTATE.WAIT;
        else
            healHumain = HEALSTATE.WAIT;
    }


    public void ChangeBar(int natureValue,int humainValue)
    {
        if (shielHumain != SHIELDSTATE.SHIELD || humainValue > 0)
        {
            humain += humainValue;
            visuHumain += humainValue;
        }
        if (shielNature != SHIELDSTATE.SHIELD || natureValue >0)
        {
            nature += natureValue;
            visuNature += natureValue;
        }
    }

}
