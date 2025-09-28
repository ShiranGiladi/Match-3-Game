using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;  // Checks there's only one object
    public AudioSource soundEffectsSource;
    public AudioSource backgroundSource;
    public AudioClip destroyNoise;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioClip menuSound;  // Menu and level screens sound

    public float DestroyVolume = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Start()
    {
        EventManager.onMatchMade += OnMatchMade;
        EventManager.onGameWon += PlayWinSound;
        EventManager.onGameLost += PlayLoseSound;

    }

    void OnDestroy()
    {
        EventManager.onMatchMade -= OnMatchMade;
        EventManager.onGameWon -= PlayWinSound;
        EventManager.onGameLost -= PlayLoseSound;

    }

    public void PlayDestroyNoise()
    {
        soundEffectsSource.PlayOneShot(destroyNoise, DestroyVolume);
    }

    public void PlayWinSound()
    {
        soundEffectsSource.PlayOneShot(winSound);
        backgroundSource.Stop();
    }

    public void PlayLoseSound()
    {
        soundEffectsSource.PlayOneShot(loseSound);
        backgroundSource.Stop();
    }

    public void OnMatchMade(MatchEventData eventData)
    {
        PlayDestroyNoise();
    }

}
