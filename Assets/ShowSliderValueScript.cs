using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSliderValueScript : MonoBehaviour
{

    public void textUpdate(float value)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if(gameObject.name == "CurrentValueSessionTime")
        {
            GameObject.FindObjectOfType<GameManagerScript>().gameTime = value;
        }
        else if (gameObject.name == "CurrentValueSpawnTime")
        {
            GameObject.FindObjectOfType<GameManagerScript>().spawnTime = value;
        }
    }
}
