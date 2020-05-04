using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Game : MonoBehaviour
{
    //static variables
    public static Game game;
    public static PlayerManager player;
    public static Vector3 Player_Starting_Location;
    public static int EnemyCount = 0;
    public static int EnemyCap = 30;

    private static float levelOneAI = 0.1f; // time it takes to update system
    private static float levelTwoAI = 0.5f; // time it takes to update system
    private static float levelThreeAI = 0.25f; // time it takes to update system
    private static float levelFourAI = 0.1f; // time it takes to update system

    public List<Spawner> spawners = new List<Spawner>();

    public GameObject[] EnemyPrefabs;

    public GameObject playerToSpawn;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!game)
        {
            game = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (playerToSpawn)
        {
            Instantiate(game.playerToSpawn);
        }
    }

    void Start()
    {
        player.transform.position = Player_Starting_Location;
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


        //Test if enemy cap reached
        if (EnemyCount < EnemyCap)
        {
            enemy = Instantiate(game.EnemyPrefabs[whatEnemy], location, Quaternion.identity);
        }

        //returns true if an enemy was spawned
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

    public static void LoadNextScene()
    {
        game.playerToSpawn = player.gameObject;

        Debug.Log("Player to Spawn: " + game.playerToSpawn.name);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public static void playerStats()
    {

    }

}
