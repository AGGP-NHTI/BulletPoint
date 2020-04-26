using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static Game game;
    public GameObject Player_Object;
    public static GameObject player;

    private static float levelOneAI = 0.1f; // time it takes to update system
    private static float levelTwoAI = 0.5f; // time it takes to update system
    private static float levelThreeAI = 0.25f; // time it takes to update system
    private static float levelFourAI = 0.1f; // time it takes to update system

    void Awake()
    {
        player = Player_Object;
        if (!game)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float getlevelOneAI()
    {
        return levelOneAI;
    }

    public static float getlevelTwoAI()
    {
        return levelTwoAI;
    }
    public static float getlevelThreeAI()
    {
        return levelThreeAI;
    }
    public static float getlevelFourAI()
    {
        return levelFourAI;
    }
}
