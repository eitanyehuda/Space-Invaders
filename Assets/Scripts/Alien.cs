using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour {

    public float speed = 10;

    private Rigidbody2D rigidBody;

    public Sprite startingImage;

    public Sprite altImage;

    private SpriteRenderer spriteRenderer;

    public float secBeforeSpriteChange = 0.5f;

    public GameObject alienBullet;

    public float minFireRateTime = 1.0f;

    public float maxFireRateTime = 3.0f;

    public float baseFireWaitTime = 3.0f;

    public Sprite explodedShipImage;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.velocity = new Vector2(1, 0) * speed;

        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(changeAlienSprite());

        baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);


    }

    // Turn in opposite direction
    void Turn(int direction)
    {
        Vector2 newVelocity = rigidBody.velocity;
        newVelocity.x = speed * direction;
        rigidBody.velocity = newVelocity;
    }

    // Move down after hitting wall
    void MoveDown()
    {
        Vector2 position = transform.position;
        position.y -= 1;
        transform.position = position;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "LeftWall")
        {
            Turn(1);
            MoveDown();
        }

        if (col.gameObject.name == "RightWall")
        {
            Turn(-1);
            MoveDown();
        }

        if (col.gameObject.tag == "Bullet")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDies);
            Destroy(gameObject);
        }
    }

    public IEnumerator changeAlienSprite()
    {
        while (true)
        {
            if(spriteRenderer.sprite == startingImage)
            {
                spriteRenderer.sprite = altImage;

                //SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz1);
            }
            else
            {
                spriteRenderer.sprite = startingImage;
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienBuzz2);
            }

            yield return new WaitForSeconds(secBeforeSpriteChange);
        }
    }

    void FixedUpdate()
    {
        if(Time.time > baseFireWaitTime)
        {
            baseFireWaitTime = baseFireWaitTime + Random.Range(minFireRateTime, maxFireRateTime);

            Instantiate(alienBullet, transform.position, Quaternion.identity);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);
            col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;
            Destroy(gameObject);
            DestroyObject(col.gameObject, 0.5f);
        }
    }
}
