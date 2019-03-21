using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scream : MonoBehaviour
{
    public AudioClip maleScream;
    public AudioClip femaleScream;
    public AudioClip s1;
    public AudioClip s2;
    public AudioClip s3;
    public AudioClip s4;
    public AudioClip playerEaten;
    public AudioClip humanEaten;
    public AudioClip coinCollect;
    private AudioSource scream;
    public Text HText;
    public AudioClip[] screams;
    // Start is called before the first frame update
    void Start()
    {
        scream = GetComponent<AudioSource>();
        screams[0] = maleScream;
        screams[1] = femaleScream;
        screams[2] = s1;
        screams[3] = s2;
        screams[4] = s3;
        screams[5] = s4;

    }

    // Update is called once per frame
    void Update()
    {
        if (HText.enabled)
        {
            scream.PlayOneShot(playerEaten);
            return;
        }
        if (PlayerStats.scream)
        {
            scream.PlayOneShot(humanEaten, 1.5f);
            scream.PlayOneShot(screams[Random.Range(0, screams.Length)]);
            PlayerStats.scream = false;
        }
        if (PlayerStats.collecting)
        {
            scream.PlayOneShot(coinCollect, 1.5f);
            PlayerStats.collecting = false;
        }
    }
}
