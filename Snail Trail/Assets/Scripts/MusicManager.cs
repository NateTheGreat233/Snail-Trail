using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager m;

    public AudioClip breakSound;
    public AudioClip loseSound;
    public AudioClip snailMoveSound;
    public AudioClip boxMoveSound;
    public AudioClip reachFlagSound;
    public AudioClip winSound;
    public AudioClip pressButtonSound;
    public AudioClip releaseButtonSound;
    public AudioClip cannotMoveSound;
    public AudioClip lockedLevelSound;
    public AudioClip menuMusic;
    public AudioClip levelMusic;

    private bool music;
    private bool sfx;
    private AudioSource sfxSource;
    private AudioSource musicSource;

    private bool boxMoveSoundReady;
    private bool cannotMoveSoundReady;
    private bool snailMoveSoundReady;

    private void Awake()
    {
        music = true;
        sfx = true;
        boxMoveSoundReady = true;
        cannotMoveSoundReady = true;
        snailMoveSoundReady = true;

        if (m == null)
        {
            m = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        musicSource = GetComponentsInChildren<AudioSource>()[1];

        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        if (!sfx) return;
        
        AudioClip c = null;
        if (name.Equals("destroy"))
        {
            c = breakSound;
        }
        else if (name.Equals("move") && snailMoveSoundReady)
        {
            c = snailMoveSound;
            snailMoveSoundReady = false;
            StartCoroutine(SnailMoveCooldown());
        }
        else if (name.Equals("box move") && boxMoveSoundReady)
        {
            c = boxMoveSound;
            boxMoveSoundReady = false;
            StartCoroutine(BoxMoveCooldown());
        }
        else if (name.Equals("flag"))
        {
            c = reachFlagSound;
        }
        else if (name.Equals("press button"))
        {
            c = pressButtonSound;
        }
        else if (name.Equals("release button"))
        {
            c = releaseButtonSound;
        }
        else if (name.Equals("cannot move") && cannotMoveSoundReady)
        {
            c = cannotMoveSound;
            cannotMoveSoundReady = false;
            StartCoroutine(CannotMoveCooldown());
        }
        else if (name.Equals("win"))
        {
            c = winSound;
        }
        else if (name.Equals("lose"))
        {
            c = loseSound;
        }
        else if (name.Equals("locked level"))
        {
            c = lockedLevelSound;
        }

        if (c != null)
        {
            sfxSource.PlayOneShot(c);
        }
    }

    public void ChangeToMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void ChangeToLevelMusic()
    {
        musicSource.clip = levelMusic;
        musicSource.Play();
    }

    public void PlayMusic(bool b)
    {
        music = b;

        if (!music)
        {
            musicSource.volume = 0;
        }
        else
        {
            musicSource.volume = 1f;
        }
    }

    public void PlaySFX(bool b)
    {
        sfx = b;
    }

    public bool GetMusic()
    {
        return music;
    }

    public bool GetSFX()
    {
        return sfx;
    }

    private IEnumerator CannotMoveCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        cannotMoveSoundReady = true;
    }

    private IEnumerator BoxMoveCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        boxMoveSoundReady = true;
    }

    private IEnumerator SnailMoveCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        snailMoveSoundReady = true;
    }

}
