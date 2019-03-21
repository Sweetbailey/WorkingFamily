using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private float health;
    private Transform tf;
    private float p1Rate;
    private float p2Rate;
    [SerializeField]
    float baseFactor;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        health = tf.localScale.x * 100f;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player1")
        {
            p1Rate = baseFactor * PlayerStats.p1SizeRatio;
            health -= p1Rate;
            PlayerStats.p1Money += p1Rate;
        }
        else if(collision.gameObject.tag == "Player2")
        {
            p2Rate = baseFactor * PlayerStats.p2SizeRatio;
            health -= p2Rate;
            PlayerStats.p2Money += p2Rate;
        }
    }
}
