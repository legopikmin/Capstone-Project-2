using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Created by: Matthew Myers and Alejandro Muros
/// Date: 5/5/20
/// Script was made to play audio sounds picked radomly from a list
/// 
/// NOTE: couldn't test this on my computer (Matt) due to sound issues. However it did work on Alejandro's computer
/// </summary>
public class Announcer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>(); //stores all the audio clips this script can play
    [SerializeField] private float minTimer; //the minimum time that can be set by random.range
    [SerializeField] private float maxTimer; //the maximum time that can be set by random.range

    public float timer;
    private AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        timer = maxTimer; //sets timer to equal maxTimer at the start
    }

    private void Update()
    {
        if (timer > 0) //used if timer is greater than 0
        {
            timer -= Time.deltaTime;

            if (timer <= 0) //used if timer is less or equal to 0
            {
                timer = Random.Range(minTimer, maxTimer); //sets timer to equal a number between minTimer and maxTimer
                PlayAudio(); //calls a method to play a random audio clip
            }
        }

    }

    //This method is used to play a random audio clip
    private void PlayAudio()
    {
        int chosenClip = Random.Range(0, clips.Count); //chooses a number between 0 and the value of the max number of audio clips in clips 

        aud.PlayOneShot(clips[chosenClip]); //this will play a audio clip based on the chosenClips value
    }
}
