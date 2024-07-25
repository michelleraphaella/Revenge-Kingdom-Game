using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class Video : MonoBehaviour
{
    private VideoPlayer player;
    public Button button;
    public Sprite playSprite;
    public Sprite pauseSprite;
    //public Button buttonNext;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        //player.loopPointReached += OnVideoEnded;
        //buttonNext.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayPause()
    {
        if (player.isPlaying == false)
        {
            player.Play();
            button.image.sprite = pauseSprite;
        }
        else
        {
            player.Pause();
            button.image.sprite = playSprite;
        }
    }

    //private void OnVideoEnded(VideoPlayer vp)
    //{
    //    // Show the button when the video ends
    //    buttonNext.gameObject.SetActive(true);
    //}
}
