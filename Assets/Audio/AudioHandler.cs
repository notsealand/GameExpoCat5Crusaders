using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    //On start, play the audio clip "Lobbying" and loop it
    //Once players are ready and game starts, stop "Lobbying"
    //Then play a random audio from folder MUSIC or OPTIONAL during the match
    //Once the game ends, stop the random audio

    [SerializeField]
    AudioClip slot1;
    [SerializeField]
    AudioClip slot2;
    [SerializeField]
    AudioClip slot3;
    [SerializeField]
    AudioClip slot4;
    [SerializeField]
    AudioClip slot5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
