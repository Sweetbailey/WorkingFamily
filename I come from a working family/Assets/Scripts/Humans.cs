using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humans : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player1")
        {
            Debug.Log("Yeet");
            PlayerStats.p1BodyCount++;
            PlayerStats.p1Money += 50f;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player2")
        {
            PlayerStats.p2BodyCount++;
            PlayerStats.p2Money += 50f;
            Destroy(gameObject);
        }
    }
}
