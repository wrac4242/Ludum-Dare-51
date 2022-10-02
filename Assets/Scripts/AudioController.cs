using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public enum SoundEffect
    {
        Phase1Correct,
        Phase1Incorrect,
        Phase2Success,
        GameOver,
        GameStart,
        ButtonClick,
        NextPhase
    }
    AudioSource audioOut;

    [SerializeField]
    AudioClip phase1Correct;
    [SerializeField]
    AudioClip phase1Incorrect;
    [SerializeField]
    AudioClip phase2Success;
    [SerializeField]
    AudioClip gameOver;
    [SerializeField]
    AudioClip gameStart;
    [SerializeField]
    AudioClip buttonClick;
    [SerializeField]
    AudioClip nextPhase;
    
    [Range(0f, 1f)]
    [SerializeField]
    float volume = 0.5f;

    public void PlayEffect(SoundEffect effect) {
        if (audioOut == null) audioOut = gameObject.GetComponent<AudioSource>();
        AudioClip toPlay = null;

        switch (effect)
        {
            case SoundEffect.Phase1Correct:
                toPlay = phase1Correct;
                break;
            case SoundEffect.Phase1Incorrect:
                toPlay = phase1Incorrect;
                break;
            case SoundEffect.Phase2Success:
                toPlay = phase2Success;
                break;
            case SoundEffect.GameOver:
                toPlay = gameOver;
                break;
            case SoundEffect.GameStart:
                toPlay = gameStart;
                break;
            case SoundEffect.ButtonClick:
                toPlay = buttonClick;
                break;
            case SoundEffect.NextPhase:
                toPlay = nextPhase;
                break;
        }

        if (toPlay == null) {
            Debug.LogError("No effect to play");
            return;
        }

        audioOut.PlayOneShot(toPlay, volume);
    }
}
