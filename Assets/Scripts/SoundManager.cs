using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;  // Checks there's only one object
    public AudioSource soundEffectsSource;
    public AudioClip destroyNoise;
    public AudioClip winSound;
    public AudioClip loseSound;


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
        EventManager.onMatchMade += OnMuchMade;
    }

    void OnDestroy()
    {
        EventManager.onMatchMade -= OnMuchMade;

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
    }

    public void PlayLoseSound()
    {
        soundEffectsSource.PlayOneShot(loseSound);
    }

    public void OnMuchMade(int muchMade)
    {
        PlayRandomDestroyNoise();
    }

}
