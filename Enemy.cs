using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Movement
{
    public int damage;
    public AudioClip[] attackSounds;

    Animator animator;
    Transform player;
    bool skipMove;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        GameHandler.instance.AddToList(this);
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void MoveAttempt<T>(int x, int y)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }
        base.MoveAttempt<T>(x, y);
        skipMove = true;
    }

    public void EnemyMove()
    {
        int x = 0;
        int y = 0;

        if (Mathf.Abs(player.position.x - transform.position.x) < float.Epsilon)
            y = player.position.y > transform.position.y ? 1 : -1;
        else
            x = player.position.x > transform.position.x ? 1 : -1;
        MoveAttempt<Player>(x, y);
    }

    protected override void OnCantMove <T> (T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("zombieAttack");
        SoundHandler.instance.RandomizeSfx(attackSounds);
        hitPlayer.LoseFood(damage);
        animator.SetTrigger("playerHit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
