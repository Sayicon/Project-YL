using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip backgroundMusic;

    [Header("SFX Settings")]
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip shurikenClip;
    [SerializeField] private int sfxPoolSize = 10;

    private List<AudioSource> sfxPool;

    private static AudioManager instance;

    void Awake()
    {
        // Singleton kurulum (birden fazla AudioManager olmasın)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // SFX havuzu oluştur
        sfxPool = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxPool.Add(src);
        }
    }

    void Start()
    {
        // Sahne başlar başlamaz müzik çalsın
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource src in sfxPool)
        {
            if (!src.isPlaying)
                return src;
        }

        // Hiç boş yoksa, ilkini kullan (üst üste ses gerekirse)
        return sfxPool[0];
    }

    public static void PlayExplosion()
    {
        if (instance == null) return;
        instance.PlaySFX(instance.explosionClip);
    }

    public static void PlayShuriken()
    {
        if (instance == null) return;
        instance.PlaySFX(instance.shurikenClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource src = GetAvailableSFXSource();
        src.clip = clip;
        src.volume = 0.3f;
        src.pitch = Random.Range(0.95f, 1.05f); // küçük varyasyon doğal his verir
        src.Play();
    }
}
