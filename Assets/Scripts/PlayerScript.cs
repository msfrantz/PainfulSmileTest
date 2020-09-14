using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [SerializeField]
    private bool isMoving;
    [SerializeField]
    private bool sailChangedRecently;

    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float health;

    public GameObject healthBar;
    public Transform healthFill;

    public GameObject gameOverMenu;

    public GameObject infoTextPrefab;

    public GameObject sail;

    public Sprite sailReleased;

    public Sprite sailTied;

    public Sprite damagedHull;
    public Sprite damagedHull2;
    public Sprite damagedSail;
    public Sprite damagedSail2;

    void Start()
    {
        health = 100;
        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/100";

        rb2d = GetComponent<Rigidbody2D>();
        sailChangedRecently = false;

        sail.transform.localScale = new Vector3(1f, 1f, 1f);
        sail.GetComponent<SpriteRenderer>().sprite = sailTied;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health--;

        }



        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!sailChangedRecently && !isMoving)
            {
                sailChangedRecently = true;
                StartCoroutine("StartMoving");
            }
            else if (sailChangedRecently && !isMoving)
            {
                SpawnText("Crew is Busy!", Color.red);
            }

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (!sailChangedRecently && isMoving)
            {
                sailChangedRecently = true;
                StartCoroutine("StopMoving");
            }
            else if (sailChangedRecently && isMoving)
            {
                SpawnText("Crew is Busy!", Color.red);
            }

        }


        if (Input.GetKey(KeyCode.D))
        {
            rb2d.AddTorque(-0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb2d.AddTorque(0.1f);
        }


    }
    void FixedUpdate()
    {

        if (isMoving)
        {
            rb2d.AddForce(-transform.up * speed);
        }

    }

    IEnumerator StartMoving()
    {
        SpawnText("Release Sails!", Color.white);


        sail.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        sail.GetComponent<SpriteRenderer>().sprite = sailReleased;

        isMoving = true;
        rb2d.drag = 3f;
        yield return new WaitForSeconds(1f);
        rb2d.drag = 2.5f;
        yield return new WaitForSeconds(1f);
        rb2d.drag = 1.5f;


        sailChangedRecently = false;

    }

    IEnumerator StopMoving()
    {
        SpawnText("Hold Sails!", Color.white);

        sail.transform.localScale = new Vector3(1f, 1f, 1f);
        sail.GetComponent<SpriteRenderer>().sprite = sailTied;

        isMoving = false;
        yield return new WaitForSeconds(2f);
        sailChangedRecently = false;
    }



    void SpawnText(string textToSpawn, Color colorToSpawn)
    {
        GameObject infoText = Instantiate(infoTextPrefab, new Vector3(transform.position.x, transform.position.y + 0.25f, -0.01f), new Quaternion(0, 0, 0, 0));
        infoText.GetComponentInChildren<TextMeshPro>().SetText(textToSpawn);
        infoText.GetComponentInChildren<TextMeshPro>().color = colorToSpawn;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Ship":
                health -= 25;
                break;

            case "Island":
                health -= 5;
                break;

            case "EnemyShot":
                health -= 10;
                break;
        }

        if (health < 65)
        {
            sailReleased = damagedSail;
            if (isMoving)
                sail.GetComponent<SpriteRenderer>().sprite = damagedSail;
            gameObject.GetComponent<SpriteRenderer>().sprite = damagedHull;

        }
        if (health < 30)
        {
            sailReleased = damagedSail2;
            if (isMoving)
                sail.GetComponent<SpriteRenderer>().sprite = damagedSail2;
            gameObject.GetComponent<SpriteRenderer>().sprite = damagedHull2;
        }
        if (health < 1)
        {
            gameObject.SetActive(false);
            GameObject.FindObjectOfType<GameManagerScript>().GameOver();
        }
        healthBar.GetComponent<TextMeshPro>().text = health.ToString() + "/100";
        healthFill.localPosition = new Vector3((health - 100) * 0.01f, 0, 0);
    }
}
