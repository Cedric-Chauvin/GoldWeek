using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CARDTYPE
    {
        NORMAL,
        SHIELD,
        HEAL
    }

    public CARDTYPE type;
    public Sprite petit;
    public Sprite grand;
    public int nature;
    public int humain;
    public float speed;
    private bool visuPos;
    private bool visuNeg;
    private bool change;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.tag == "P1")
        {
            if (transform.position.y < 2)
            {
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
            }
            else
            {
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed);

            }
        }
        else
        {
            if (transform.position.y > -2)
            {
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(0, 0, 180), Time.deltaTime * speed);

            }
            else
            {
                transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
            }
        }

    }

    public void PlayVisuPos()
    {
        Testtouch.game.visuHumain += humain;
        Testtouch.game.visuNature += nature;
    }

    public void PlayVisuNeg()
    {
        Testtouch.game.visuHumain -= humain;
        Testtouch.game.visuNature -= nature;
    }

    public void PlayCard()
    {
        switch (type)
        {
            case CARDTYPE.NORMAL:
                Testtouch.game.ChangeBar(nature, humain);

                break;

            case CARDTYPE.SHIELD:
                Testtouch.game.ApplyShield(transform.tag);

                break;

            case CARDTYPE.HEAL:
                Testtouch.game.ApplyHeal(transform.tag);
                break;
            default:
                break;
        }

    }
}
