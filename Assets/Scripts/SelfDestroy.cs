using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestruct", 0.9f);
    }

    void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
