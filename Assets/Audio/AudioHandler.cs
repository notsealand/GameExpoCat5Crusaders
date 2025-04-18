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
    AudioClip slot1; //"Lobbying"
    public AudioClip GetSlot1()
    {
        return slot1;
    }

    [SerializeField]
    AudioClip slot2; //"Battle"
    public AudioClip GetSlot2()
    {
        return slot2;
    }
    [SerializeField]
    AudioClip slot3; //"BeSmart"
    public AudioClip GetSlot3()
    {
        return slot3;
    }
    [SerializeField]
    AudioClip slot4; //"tbhidek"
    public AudioClip GetSlot4()
    {
        return slot4;
    }
    [SerializeField]
    AudioClip slot5; //"Trance 009"
    public AudioClip GetSlot5()
    {
        return slot5;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
