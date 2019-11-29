using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip calmMusic;
    public AudioClip intenseMusic;

    private AudioSource audioSource;
    private static MusicManager _instance;

    public static MusicManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MusicManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if (this != _instance)
            {
                Destroy(this.gameObject);
            }
                
        }

        audioSource = this.gameObject.GetComponent<AudioSource>();
        PlayCalmMusic();
    }

    public void PlayCalmMusic()
    {
        audioSource.Stop();
        audioSource.clip = calmMusic;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void PlayIntenseMusic()
    {
        audioSource.Stop();
        audioSource.clip = intenseMusic;
        audioSource.Play();
        audioSource.loop = true;
    }
}