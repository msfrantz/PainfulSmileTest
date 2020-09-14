using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallScript : MonoBehaviour
{
    [SerializeField]
    private float cannonBallImpulse = 1.5f;

    public GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestruct", 1.3f);
        //InvokeRepeating("AddDrag", 0.25f, 0.2f);
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * cannonBallImpulse, ForceMode2D.Impulse);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    void AddDrag()
    {
        gameObject.GetComponent<Rigidbody2D>().drag += 0.5f;
    }
    private void FixedUpdate()
    {
       

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
             
        if (collision.gameObject.CompareTag("Ship") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerShot"))
        {
            Instantiate(explosionPrefab, collision.transform.position, Quaternion.Euler(0, 0, 0));
        }

        Destroy(gameObject);

    }
}

