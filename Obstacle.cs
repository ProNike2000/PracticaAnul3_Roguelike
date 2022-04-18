using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Sprite damage;
    public int hp = 3;
    public AudioClip[] hitSounds;

    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void damageObstacle(int dmg)
    {
        renderer.sprite = damage;
        hp -= dmg;
        SoundHandler.instance.RandomizeSfx(hitSounds);
        if (hp <= 0)
            gameObject.SetActive(false);
    }
}
