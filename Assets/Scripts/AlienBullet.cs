using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBullet : MonoBehaviour {

    private Rigidbody2D rigidBody;

    public float speed = 30;

    public Sprite explodedShipImage;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody2D>();

        rigidBody.velocity = Vector2.down * speed;

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if(col.gameObject.tag == "Player")
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.shipExplosion);

            col.GetComponent<SpriteRenderer>().sprite = explodedShipImage;

            Destroy(gameObject);

            DestroyObject(col.gameObject, 0.5f);
        }

        if (col.tag == "Shield")
        {
            Destroy(gameObject);
            DestroyObject(col.gameObject);
        }

    }

    void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
