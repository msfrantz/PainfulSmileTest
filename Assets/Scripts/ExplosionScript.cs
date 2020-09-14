using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestruct", 0.45f);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
