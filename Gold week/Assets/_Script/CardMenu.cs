using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMenu : MonoBehaviour
{
    public float speed;
    public CARDTYPE type;

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

   public enum CARDTYPE
    {
        Play,
        Quit,
        Option,
    }
}
