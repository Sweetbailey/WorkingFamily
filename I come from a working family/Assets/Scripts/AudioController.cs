using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip bgMusic;
    public AudioClip eatMusic;
    private AudioSource music;
    private bool fkOff = false;
    // Start is called before the first frame update
    void Start()
    {
        music = gameObject.GetComponent<AudioSource>();
        playBGM();
        fkOff = true;

    }
    public void playBGM()
    {
        music.Stop();
        music.loop = true;
        music.PlayOneShot(bgMusic);
    }
    public void playEat()
    {
        music.Stop();
        music.loop = true;
        music.PlayOneShot(eatMusic);
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.bgm && !music.isPlaying)
        {
            Debug.Log("Bgm");
            playBGM();
        }
        if(!PlayerStats.bgm && fkOff)
        {
            fkOff = false;
            Debug.Log("Eatmusic");
            playEat();
            
        }
    }
}
