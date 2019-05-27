using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        HAUTET2,
        MEDIUM,
        MEDIUMT2,
        BASSE,
        BASSET2
    }

    SHIELDSTATE shielNature = SHIELDSTATE.NONE;
    SHIELDSTATE shielHumain = SHIELDSTATE.NONE;
    HEALSTATE healNature = HEALSTATE.NONE;
    HEALSTATE healHumain = HEALSTATE.NONE;
    BARSTATE natureState = BARSTATE.NONE;
    BARSTATE humainState = BARSTATE.NONE;
    public GameObject shieldN;
    public GameObject shieldH;
    public GameObject healN;
    public GameObject healH;
    int comptHealN;
    int comptHealH;
    public static Testtouch game;
    public Image barHumain;
    public Image barNature;
    public Image visuBarHumain;
    public Image visuBarNature;
    public Image victoireHumain;
    public Image victoireNature;
    public Image defaite;
    public Image victoireCoop;
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
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }


        UpdateCard();
        UpdateBar();

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

                        if (collision.Count > i)
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

    private void UpdateBar()
    {
        if(humain<0)
        {
            barHumain.rectTransform.rotation = Quaternion.Euler(0, 0, 180);
            barHumain.rectTransform.localPosition = new Vector3(barHumain.rectTransform.localPosition.x, -200);
            
        }
        else
        {
            barHumain.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            barHumain.rectTransform.localPosition = new Vector3(barHumain.rectTransform.localPosition.x, 200);
            
        }

        if (visuHumain<0)
        {
            visuBarHumain.rectTransform.rotation = Quaternion.Euler(0, 0, 180);
            visuBarHumain.rectTransform.localPosition = new Vector3(barHumain.rectTransform.localPosition.x, -200);
        }
        else
        {
            visuBarHumain.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            visuBarHumain.rectTransform.localPosition = new Vector3(barHumain.rectTransform.localPosition.x, 200);
        }

        if(nature<0)
        {
            barNature.rectTransform.rotation = Quaternion.Euler(0, 0, 180);
            barNature.rectTransform.localPosition = new Vector3(barNature.rectTransform.localPosition.x, 200);
            
        }
        else
        {
            barNature.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            barNature.rectTransform.localPosition = new Vector3(barNature.rectTransform.localPosition.x, -200);
            
        }

        if (visuNature < 0)
        {
            visuBarNature.rectTransform.rotation = Quaternion.Euler(0, 0, 180);
            visuBarNature.rectTransform.localPosition = new Vector3(barNature.rectTransform.localPosition.x, 200);
        }
        else
        {
            visuBarNature.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
            visuBarNature.rectTransform.localPosition = new Vector3(barNature.rectTransform.localPosition.x, -200);
        }

        barHumain.fillAmount = (Mathf.Abs( humain)) / (maxHumain );
        visuBarHumain.fillAmount = (Mathf.Abs( visuHumain)) / (maxHumain );
        barNature.fillAmount = (Mathf.Abs( nature) ) / (maxNature );
        visuBarNature.fillAmount = (Mathf.Abs( visuNature )) / (maxNature );
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
            SoundControler._soundControler.PlaySound(SoundControler._soundControler._pioche);
            manager1.PlayCard();
            manager2.PlayCard();
            isTimerCard = false;
            CalculVictory();
            text.text = "Play";
            if (shielHumain != SHIELDSTATE.NONE)
            {
                shielHumain++;
                shieldH.SetActive(true);
            }
            if (shielHumain == (SHIELDSTATE)3)
            {
                shielHumain = SHIELDSTATE.NONE;
                shieldH.SetActive(false);
            }
            if (shielNature != SHIELDSTATE.NONE)
            {
                shielNature++;
                shieldN.SetActive(true);
            }
            if (shielNature == (SHIELDSTATE)3)
            {
                shielNature = SHIELDSTATE.NONE;
                shieldN.SetActive(false);
            }
            if (healHumain != HEALSTATE.NONE)
            {
                if (healHumain != HEALSTATE.WAIT)
                {
                    humain++;
                    visuHumain++;
                    comptHealH--;
                    healH.transform.GetChild(0).GetComponent<Text>().text = comptHealH.ToString();
                }
                else visuHumain++;
                healHumain++;
                if (healHumain == (HEALSTATE)5)
                {
                    visuHumain--;
                    healHumain = HEALSTATE.NONE;
                    healH.SetActive(false);
                }
            }
            if (healNature != HEALSTATE.NONE)
            {
                if (healNature != HEALSTATE.WAIT)
                {
                    nature++;
                    visuNature++;
                    comptHealN--;
                    healN.transform.GetChild(0).GetComponent<Text>().text = comptHealN.ToString();
                }else
                    visuNature++;
                healNature++;
                if (healNature == (HEALSTATE)5)
                {
                    visuNature--;
                    healNature = HEALSTATE.NONE;
                    healN.SetActive(false);
                }
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
        if (nature > 6)
        {
            if (natureState == BARSTATE.HAUTE)
                natureState = BARSTATE.HAUTET2;
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
                natureState = BARSTATE.MEDIUMT2;
            else
                natureState = BARSTATE.MEDIUM;
        }
        else
        {
            if (natureState == BARSTATE.BASSE)
                natureState = BARSTATE.BASSET2;
            else
                natureState = BARSTATE.BASSE;
        }

        if (humain > 6)
        {
            if (humainState == BARSTATE.HAUTE)
                humainState = BARSTATE.HAUTET2;
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
                humainState = BARSTATE.MEDIUMT2;
            else
                humainState = BARSTATE.MEDIUM;
        }
        else
        {
            if (humainState == BARSTATE.BASSE)
                humainState = BARSTATE.BASSET2;
            else
                humainState = BARSTATE.BASSE;
        }


        switch (natureState)
        {
            case BARSTATE.NONE:
                switch (humainState)
                {

                    case BARSTATE.HAUTET2:
                        VictoireH() ;
                        break;
                    case BARSTATE.MEDIUMT2:
                        VictoireN() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        //rien
                        break;

                }
                break;
            case BARSTATE.HAUTE:

                switch (humainState)
                {
                    case BARSTATE.HAUTET2:
                        VictoireH() ;
                        break;
                    case BARSTATE.MEDIUMT2:
                        VictoireN() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        //rien
                        break;
                }
                break;

            case BARSTATE.HAUTET2:

                switch (humainState)
                {
                    case BARSTATE.HAUTET2:
                        VictoireC() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        VictoireN() ;
                        break;
                }

                break;
            case BARSTATE.MEDIUM:

                switch (humainState)
                {
                    case BARSTATE.HAUTET2:
                        VictoireH() ;
                        break;
                    case BARSTATE.MEDIUMT2:
                        VictoireN() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        //rien
                        break;
                }

                break;
            case BARSTATE.MEDIUMT2:

                switch (humainState)
                {
                    case BARSTATE.HAUTET2:
                        VictoireH() ;
                        break;
                    case BARSTATE.MEDIUMT2:
                        Defaite() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        VictoireH() ;
                        break;
                }

                break;
            case BARSTATE.BASSE:

                switch (humainState)
                {
                    case BARSTATE.HAUTET2:
                        VictoireH() ;
                        break;
                    case BARSTATE.MEDIUMT2:
                        VictoireN() ;
                        break;
                    case BARSTATE.BASSET2:
                        Defaite() ;
                        break;
                    default:
                        //rien
                        break;
                }

                break;
            case BARSTATE.BASSET2:

                Defaite() ;

                break;
            default:
                break;
        }

    }

    public void VictoireH()
    {
        victoireHumain.gameObject.SetActive(true);
    }

    public void VictoireN()
    {
        victoireNature.gameObject.SetActive(true);
    }

    public void VictoireC()
    {
        victoireCoop.gameObject.SetActive(true);
    }

    public void Defaite()
    {
        defaite.gameObject.SetActive(true);
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
        {
            healNature = HEALSTATE.WAIT;
            healN.SetActive(true);
            comptHealN = 3;
        }
        else
        {
            healHumain = HEALSTATE.WAIT;
            healH.SetActive(true);
            comptHealH = 3;

        }
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

    public void PlayVisu(int natureValue, int humainValue, bool positif)
    {
        if (positif)
        {
            if (shielHumain != SHIELDSTATE.SHIELD || humainValue > 0)
                visuHumain += humainValue;
            if (shielNature != SHIELDSTATE.SHIELD || natureValue > 0)
                visuNature += natureValue;
        }
        else
        {
            if (shielHumain != SHIELDSTATE.SHIELD || humainValue > 0)
                visuHumain -= humainValue;
            if (shielNature != SHIELDSTATE.SHIELD || natureValue > 0)
                visuNature -= natureValue;
        }
    }

}
