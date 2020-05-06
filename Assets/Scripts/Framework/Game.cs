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

    bool isNeverUnloadLoaded = false;

    System.Func<bool> isCurrentSceneLoaded = () => false;

    System.Func<bool> isSceneUnloaded = () => false;

    public static System.Func<bool> playerExists = () => false;


    int currentSceneLoaded = 1;

    private void Start()
    {
        playerExists = () => player;
        if (!isNeverUnloadLoaded)
        {
            if (!game)
            {
                game = this;
            }
            else
            {
                Destroy(gameObject);
            }


            setupSceneLoading();

            isNeverUnloadLoaded = true;
        }
    }


    //void loadObjs(Scene scene, LoadSceneMode mode)
    //{



    //    if (SceneManager.GetActiveScene().buildIndex != 0)
    //    {
    //        if (PlayerPrefab)
    //        {
    //            Debug.Log("INSTANTIATING PLAYER");
    //            player = Instantiate(PlayerPrefab).GetComponent<PlayerManager>();
    //            Debug.Log("Player: " + player.gameObject.name);
    //            Weapons weapon = Instantiate(WeaponPrefab[NextWeapon]).GetComponent<Weapons>();
    //            if (weapon)
    //            {
    //                Debug.Log("SHOULD SET WEAPON OWNED--------------------------------------------------------------------------");
    //                player.setOwnedWeapon(weapon);
    //            }
    //        }
    //    }



    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LoadLastScene();
        }
    }
    private void FixedUpdate()
    {
        Debug.Log("Player In The Scene: " +player?.name);
        Debug.Log("Active Scene: " + SceneManager.GetActiveScene().name);
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
    
















    //Scene Loading

    void OnEnable()
    {
        SceneManager.sceneLoaded += setisSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= setisSceneLoaded;
    }

    void setisSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        
        isCurrentSceneLoaded = () => false;
    }

    void setupSceneLoading()
    {
        player = GameObject.Find("Player").GetComponent<PlayerManager>();
        Debug.Log("SETUP LOADING SCENE");
        game.StartCoroutine(LoadScene(1));
    }

    public static void LoadNextScene()
    {
        //game.NextWeapon = player.weaponOwned.Game_ID;
        if (game.currentSceneLoaded < SceneManager.sceneCountInBuildSettings - 1)
        {
            game.StartCoroutine(game.Unload(game.currentSceneLoaded));


            game.StartCoroutine(game.LoadScene(game.currentSceneLoaded + 1));



            game.currentSceneLoaded++;
        }
        else
        {
            Debug.Log("Reached BOSS LEVEL");
        }

    }

    public static void LoadLastScene()
    {
        //game.NextWeapon = player.weaponOwned.Game_ID;
        if (game.currentSceneLoaded > 0)
        {
            game.StartCoroutine(game.Unload(game.currentSceneLoaded));


            game.StartCoroutine(game.LoadScene(game.currentSceneLoaded - 1));



            game.currentSceneLoaded--;

        }
    }

    IEnumerator Unload(int scene)
    {
        yield return new WaitForEndOfFrame();
        isSceneUnloaded = () => false;

        SceneManager.UnloadSceneAsync(scene);

        //isSceneUnloaded = () => SceneManager.GetSceneByBuildIndex(scene).isLoaded;
    }


    IEnumerator LoadScene(int scene)
    {
        yield return null;

       // yield return new WaitUntil(game.isSceneUnloaded);

        game.isCurrentSceneLoaded = () => false;

        Debug.Log("Start Loading");

        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        //game.isCurrentSceneLoaded = () => SceneManager.GetSceneByBuildIndex(scene).isLoaded;
    }
}
