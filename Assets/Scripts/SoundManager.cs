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


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    public void PlayRandomDestroyNoise()
    {
        // Choose a random number
        // int clipToPlay = Random.Range(0, destroyNoise.Length);
        // Play that clip
        soundEffectsSource.PlayOneShot(destroyNoise);
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
        PlayRandomDestroyNoise();
    }

}
