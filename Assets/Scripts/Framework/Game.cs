using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static Game game;
    public PlayerManager Player;
    public static PlayerManager player;

    public static int EnemyCount = 0;

    public GameObject[] EnemyPrefabs;
    public static int EnemyCap = 10;

    private static float levelOneAI = 0.1f; // time it takes to update system
    private static float levelTwoAI = 0.5f; // time it takes to update system
    private static float levelThreeAI = 0.25f; // time it takes to update system
    private static float levelFourAI = 0.1f; // time it takes to update system

    void Awake()
    {
        player = Player;

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
       // Debug.Log("Enemy Count: " + EnemyCount);
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

    public static bool SpawmEnemy(int whatEnemy, Vector3 location)
    {
        GameObject enemy = null;

        if (EnemyCount < EnemyCap)
        {
            enemy = Instantiate(game.EnemyPrefabs[whatEnemy], location, Quaternion.identity);
        }
        if (enemy)
        {
            EnemyCount++;
            return true;
        }
        else
        {
            return false;
        }
    }



}
