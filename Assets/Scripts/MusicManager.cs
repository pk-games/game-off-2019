using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip music;

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
        Play();
    }

    public void Play()
    {
        this.gameObject.GetComponent<AudioSource>().Play();
    }
}