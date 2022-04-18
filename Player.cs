using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Movement
{
    public int obstacleDmg = 1;
    public int berryPoints = 10;
    public int sodaPoints = 20;
    public float restartDelay = 1f;
    public Text remainingFood;
    public AudioClip[] moveSounds;
    public AudioClip[] eatSounds;
    public AudioClip[] drinkSounds;
    public AudioClip gameOverSound;

    Animator animator;
    int food = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameHandler.instance.foodAmount;
        remainingFood.text = "Food: " + food;
        base.Start();
    }

    void OnDisable()
    {
        GameHandler.instance.foodAmount = food;
    }

    void GameOverCheck()
    {
        if (food <= 0)
        {
            SoundHandler.instance.RandomizeSfx(gameOverSound);
            SoundHandler.instance.musicSource.Stop();
            GameHandler.instance.GameOver(0);
        }
    }

    protected override void MoveAttempt<T>(int x, int y)
    {
        food--;
        remainingFood.text = "Food: " + food;
        base.MoveAttempt<T>(x, y);
        RaycastHit2D hit;
        if (Move(x, y, out hit))
        {
            SoundHandler.instance.RandomizeSfx(moveSounds);
        }
        GameOverCheck();
        GameHandler.instance.isPlayerTurn = false;
    }

    protected override void OnCantMove <T> (T component)
    {
        Obstacle hitObstacle = component as Obstacle;
        hitObstacle.damageObstacle(obstacleDmg);
        animator.SetTrigger("playerHit");
    }

    void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerGettingHit");
        food -= loss;
        remainingFood.text = "Food: " + food + " (-" + loss + ")";
        GameOverCheck();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Exit")
        {
            Invoke("Restart", restartDelay);
            enabled = false;
        }
        else if (collider.tag == "Berries")
        {
            food += berryPoints;
            remainingFood.text = "Food: " + food + " (+" + berryPoints + ")";
            SoundHandler.instance.RandomizeSfx(eatSounds);
            collider.gameObject.SetActive(false);
        }
        else if (collider.tag == "Soda")
        {
            food += sodaPoints;
            remainingFood.text = "Food: " + food + " (+" + sodaPoints + ")";
            SoundHandler.instance.RandomizeSfx(drinkSounds);
            collider.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameHandler.instance.isPlayerTurn) return;

        int x = 0;
        int y = 0;
        x = (int)Input.GetAxisRaw("Horizontal");
        y = (int)Input.GetAxisRaw("Vertical");

        if (x != 0) y = 0;
        if (x != 0 || y != 0) MoveAttempt<Obstacle>(x, y);
    }
}
