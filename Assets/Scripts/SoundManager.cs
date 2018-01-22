using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    public AudioSource musicSource;
    public static SoundManager instance = null;
    public AudioClip[] inLevelMusic;
    public AudioClip menuMusic;
    public AudioMixerSnapshot Mute;

    private int trackNumber = 0;
    private float lengthOfClip;
    public float numOfRepeats;
    private float timeOfRepeats;
    private float fadeOutTime = 15;
    private float fadeInTime = 10;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= CheckChange;
    }

    void CheckChange(Scene previous, Scene next)
    {
        if (next.name == "MainMenu")
        {
            PlayMusic(menuMusic);
        }
        else
        {
            if (musicSource.clip != inLevelMusic[trackNumber])
            {
                PlayMusic(inLevelMusic[trackNumber]);
                StartCoroutine(CountSongLoops());
            }
        }
    }

    void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    IEnumerator CountSongLoops()
    {
        while (true)
        {
            //finds out total length of time music will play before switchinging songs
            lengthOfClip = inLevelMusic[trackNumber].length;
            timeOfRepeats = lengthOfClip * numOfRepeats;

            //waits designated amouint of timebefore fading out
            yield return new WaitForSeconds(timeOfRepeats);
            StartCoroutine(FadeOutMusic());

            //switches track #
            if (trackNumber != (inLevelMusic.Length - 1))
                trackNumber++;
            else
                trackNumber = 0;

            //after fade out is done, loads new clip, plays and fades in
            yield return new WaitForSeconds(fadeOutTime);

            musicSource.Stop();
            musicSource.clip = inLevelMusic[trackNumber];
            musicSource.Play();

            StartCoroutine(FadeInMusic());
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    IEnumerator FadeInMusic()
    {
        float t = fadeInTime;

        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            musicSource.volume = (1 - t / fadeInTime);
        }
        yield break;
    }


    IEnumerator FadeOutMusic()
    {
        float t = fadeOutTime;
        while (t > 0)
        {
            yield return null;
            t -= Time.deltaTime;
            musicSource.volume = t / fadeOutTime;
        }
        yield break;
    }
}
