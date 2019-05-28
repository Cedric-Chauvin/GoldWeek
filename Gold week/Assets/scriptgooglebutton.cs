using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scriptgooglebutton : MonoBehaviour
{
    public static scriptgooglebutton Instance { get; private set; }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}

