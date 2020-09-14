using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ChaserScript : MonoBehaviour
{
    private GameObject playerToChase;
    private Rigidbody2D rb2d;

    public GameObject explosionPrefab;

    public GameObject sail;

    public GameObject healthBar;
    public Transform healthFill;

    public Sprite damagedHull;
    public Sprite damagedSail;


    [SerializeField]
    private float speed = 6.5f;

    [SerializeField]
    private float health = 50f;

    [SerializeField]
    private float scaleMultiplier = 3f;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/50";

        if (GameObject.FindGameObjectWithTag("Player") != null)
            playerToChase = GameObject.FindGameObjectWithTag("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 direction = ((Vector2)playerToChase.transform.position - (Vector2)transform.position).normalized;

        transform.up = Vector2.Lerp(transform.up, -direction, 0.03f);

        rb2d.AddForce(-transform.up * speed);

    }




    private void OnCollisionEnter2D(Collision2D collision)
    {


        switch (collision.gameObject.tag)
        {
            case "Player":
                health -= 50;
                break;

            case "Island":
                health -= 5;
                break;

            case "PlayerShot":
                health -= 20;
                break;
        }

        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/50";
        healthFill.localPosition = new Vector3((health - 50) * 0.02f, 0, 0);

        if (health <= 30)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = damagedHull;
            sail.GetComponent<SpriteRenderer>().sprite = damagedSail;
        }
            if (health < 1)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            explosion.transform.localScale *= scaleMultiplier;
            GameObject.FindObjectOfType<GameManagerScript>().UpdateScore(1);
            Destroy(gameObject.transform.parent.gameObject);
            
        }
    }
}
