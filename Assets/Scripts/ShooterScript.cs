using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ShooterScript : MonoBehaviour
{
    private GameObject playerToChase;
    private Rigidbody2D rb2d;

    public GameObject explosionPrefab;

    public GameObject sail;

    public GameObject healthBar;
    public Transform healthFill;

    public Transform[] cannonsTransform;

    public GameObject enemyShotPrefab;

    public Sprite sailTied;

    public Sprite sailReleased;

    public Sprite damagedHull;
    public Sprite damagedSail;

    [SerializeField]
    private float fireRate = 2f;

    [SerializeField]
    private float nextFireTime = 0;

    [SerializeField]
    private bool chasing;

    [SerializeField]
    private bool inRange = false;

    [SerializeField]
    private float speed = 6.5f;

    [SerializeField]
    private float health = 100f;

    [SerializeField]
    private float scaleMultiplier = 3f;

    // Start is called before the first frame update
    void Start()
    {
        chasing = true;



        health = 100;
        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/100";

        if (GameObject.FindGameObjectWithTag("Player") != null)
            playerToChase = GameObject.FindGameObjectWithTag("Player");

        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (chasing)
        {
            Vector2 direction = ((Vector2)playerToChase.transform.position - (Vector2)transform.position).normalized;

            transform.up = Vector2.Lerp(transform.up, -direction, 0.015f);

            rb2d.AddForce(-transform.up * speed);

        }
        else if (!chasing)
        {
            Vector2 direction = ((Vector2)playerToChase.transform.position - (Vector2)transform.position).normalized;

            transform.right = Vector2.Lerp(transform.right, -direction, 0.01f);

            

            if (inRange && (Time.time > nextFireTime))
            {
                nextFireTime = Time.time + fireRate;

                for (int i = 0; i < cannonsTransform.Length; i++)
                {
                    if (cannonsTransform[i].childCount == 0)
                    {

                        StartCoroutine(SpawnCannonBall(i));
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouching(gameObject.GetComponent<BoxCollider2D>()))
        {
            inRange = true;
        }
        else if (collision.IsTouching(gameObject.GetComponent<CircleCollider2D>()))
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("works");
                chasing = false;
                sail.GetComponent<SpriteRenderer>().sprite = sailTied;
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.IsTouching(gameObject.GetComponent<CircleCollider2D>()))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("worksExit");
                chasing = true;
                inRange = false;

                sail.GetComponent<SpriteRenderer>().sprite = sailReleased;
            }
        }
        else
        {
            inRange = false;
        }
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

        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/100";
        healthFill.localPosition = new Vector3((health - 100) * 0.01f, 0, 0);

        if (health <= 60)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = damagedHull;
            sailReleased = damagedSail;
            sail.GetComponent<SpriteRenderer>().sprite = damagedSail;
        }

        if (health < 1)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.Euler(0, 0, 0));
            explosion.transform.localScale *= scaleMultiplier;
            GameObject.FindObjectOfType<GameManagerScript>().UpdateScore(2);
            Destroy(gameObject.transform.parent.gameObject);
           
        }
    }


    IEnumerator SpawnCannonBall(int i)
    {

        yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
        if (cannonsTransform != null)
        {
            Instantiate(enemyShotPrefab, cannonsTransform[i].position, Quaternion.Euler(cannonsTransform[i].eulerAngles));
        }
    }


}
