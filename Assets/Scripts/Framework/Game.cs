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

    [Header("Debugging")]
    public GameObject Player;

    [Header("Scene Transitions")]
    public int NextWeapon = 0;

    [Header("Scene Prefabs")]
    public GameObject PlayerPrefab;
    public GameObject[] WeaponPrefab;


    void Awake()
    {
        if (!game)
        {
            game = this;
        }
        else
        {
           Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }


    void OnEnable()
    {
        SceneManager.sceneLoaded += loadObjs;
    }

    void loadObjs(Scene scene, LoadSceneMode mode)
    {



        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (PlayerPrefab)
            {
                Debug.Log("INSTANTIATING PLAYER");
                player = Instantiate(PlayerPrefab).GetComponent<PlayerManager>();
                Debug.Log("Player: " + player.gameObject.name);
                Weapons weapon = Instantiate(WeaponPrefab[NextWeapon]).GetComponent<Weapons>();
                if (weapon)
                {
                    Debug.Log("SHOULD SET WEAPON OWNED--------------------------------------------------------------------------");
                    player.setOwnedWeapon(weapon);
                }
            }
        }



    }




    private void FixedUpdate()
    {
        //Debug.Log("Player: " + player?.name);
        Player = player?.gameObject;
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
        game.NextWeapon = player.weaponOwned.Game_ID;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }


    void OnDisable()
    {

        SceneManager.sceneLoaded -= loadObjs;
    }

}
