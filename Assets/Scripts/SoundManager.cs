using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip CardShuffle;
    [SerializeField] private AudioClip LevelComplete;
    [SerializeField] private AudioClip PlayerJump;
    [SerializeField] private AudioClip PlayerTurning;
    [SerializeField] private AudioClip SelectCard;
    [SerializeField] private AudioClip StoreCard;

    [SerializeField] private GameObject audioLocation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleCards()
    {
        AudioSource.PlayClipAtPoint(CardShuffle, audioLocation.transform.position);
    }

    public void CompleteLevel()
    {
        AudioSource.PlayClipAtPoint(LevelComplete, audioLocation.transform.position);
    }

    public void Jump()
    { 
        AudioSource.PlayClipAtPoint(PlayerJump, audioLocation.transform.position);
    }

    public void Turning()
    {
        AudioSource.PlayClipAtPoint(PlayerTurning, audioLocation.transform.position);
    }

    public void SelectedCard()
    {
        AudioSource.PlayClipAtPoint(SelectCard, audioLocation.transform.position);
    }

    public void StoringCard()
    {
        AudioSource.PlayClipAtPoint(StoreCard, audioLocation.transform.position);
    }
}
