using UnityEngine;

public class AudioManagement : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio(int ID)
    {
        audioSource.PlayOneShot(audioClips[ID]);
    }
}
