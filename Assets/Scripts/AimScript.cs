using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript : MonoBehaviour
{
    Vector3 mouse;
    public LayerMask hitLayers;

    public Transform playerTransform;

    public Material defaultMat, outlineMat;

    [SerializeField]
    private bool cannonSelected;

    [SerializeField]
    private float fireRate = 0.5f;

    private float nextFireTime = 0;

    Transform[] cannonsTransform;

    public GameObject cannonBallPrefab;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (cannonSelected && Time.time > nextFireTime)
            {
                if (cannonsTransform != null)
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



        mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers))
        {
            //Debug.Log(Vector3.Distance(transform.position, playerTransform.position));

            Vector2 direction = ((Vector2)hit.point - (Vector2)transform.position).normalized;
            transform.up = direction;

            float radius = 0.8f;

            Vector3 newLocation = hit.point;
            Vector3 playerPosition = playerTransform.position;


            float distance = Vector3.Distance(newLocation, playerPosition);

            if (distance > radius)
            {
                Vector3 fromOriginToObject = newLocation - playerPosition;
                fromOriginToObject *= radius / distance;
                newLocation = playerPosition + fromOriginToObject;
                transform.position = Vector3.Lerp(transform.position, newLocation, 0.05f);
            }

        }
    }

    IEnumerator SpawnCannonBall(int i)
    {

        yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
        if (cannonsTransform != null)
        {
            Instantiate(cannonBallPrefab, cannonsTransform[i].position, Quaternion.Euler(cannonsTransform[i].eulerAngles));
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cannon"))
        {
            cannonSelected = true;

            Renderer[] children;
            children = collision.gameObject.GetComponentsInChildren<Renderer>();

            cannonsTransform = collision.gameObject.GetComponentsInChildren<Transform>();

            foreach (Renderer rend in children)
            {
                rend.sharedMaterial = outlineMat;
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cannon"))
        {
            cannonSelected = false;

            Renderer[] children;
            children = collision.gameObject.GetComponentsInChildren<Renderer>();


            foreach (Renderer rend in children)
            {
                rend.sharedMaterial = defaultMat;
            }


        }

    }

}
