using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerLoad : MonoBehaviour
{
    public GameObject gameHandler;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameHandler.instance == null)
            Instantiate(gameHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
