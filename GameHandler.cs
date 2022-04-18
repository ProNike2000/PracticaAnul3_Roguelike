using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance = null;
    public BoardHandler boardScript;

    public float startDelay = 2f;
    public float turnDelay = .1f;
    public int foodAmount = 100;
    [HideInInspector] public bool isPlayerTurn = true;

    Text levelText;
    GameObject levelImage;
    int lvl = 1;
    List<Enemy> zombies;
    bool enemyMoves;
    bool isInSetup;

    public void GameOver(int code)
    {
        if (code == 1)
        {
            isInSetup = true;
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelImage = GameObject.Find("Level");
            SoundHandler.instance.musicSource.Stop();
            levelText.text = "Congratulations! You've\nmanaged to survive and to escape";
        } else
        if (code == 0)
        {
            levelText.text = "After " + lvl + " days, you died";
        }    
        levelImage.SetActive(true);
        enabled = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        zombies = new List<Enemy>();
        boardScript = GetComponent<BoardHandler>();
        init();
    }

    void OnLevelWasLoaded(int index)
    {
        lvl++;
        if (lvl == 11)
            GameOver(1);
        else
            init();
    }

    void init()
    {
        isInSetup = true;
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + lvl;
        levelImage = GameObject.Find("Level");
        levelImage.SetActive(true);
        Invoke("HideImage", startDelay);

        zombies.Clear();
        boardScript.levelSetup(lvl);
    }

    void HideImage()
    {
        levelImage.SetActive(false);
        isInSetup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerTurn || enemyMoves || isInSetup)
            return;
        
        StartCoroutine(MoveEnemies());
    }

    public void AddToList(Enemy script)
    {
        zombies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemyMoves = true;
        yield return new WaitForSeconds(turnDelay);
        if (zombies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < zombies.Count; i++)
        {
            zombies[i].EnemyMove();
            yield return new WaitForSeconds(zombies[i].moveTime);
        }
        isPlayerTurn = true;
        enemyMoves = false;
    }
}
