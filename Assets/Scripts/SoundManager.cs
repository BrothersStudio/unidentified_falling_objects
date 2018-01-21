using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    public AudioSource fxSource;
    public AudioSource musicSource;
    public static SoundManager instance = null;
    public AudioClip inLevelMusic;
    public AudioClip menuMusic;
    public AudioMixerSnapshot Mute;

    private int fadeOutTime = 2;
    private int fadeInTime = 2;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += CheckChange;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged += CheckChange;
    }

    void CheckChange(Scene previous, Scene next)
    {
        if (next.name == "MainMenu")
        {
            PlayMusic(menuMusic);
            Debug.Log("Playing menu");
        }
        else
        {
            if (musicSource.clip != inLevelMusic)
            {
                PlayMusic(inLevelMusic);
                Debug.Log("In Level Music");
            }
            
        }
    }

    void Awake ()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
	}

    public void PlaySingleFx(AudioClip clip)
    {
        fxSource.clip = clip;
        fxSource.Play();
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
