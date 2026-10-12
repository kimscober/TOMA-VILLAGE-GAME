using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip tomaShout;
    public AudioClip hitSound;
    public AudioClip cheerSound;
    public AudioClip catchSound;
    
    AudioSource src;

    void Awake(){
        if(Instance==null){ Instance=this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
        src = GetComponent<AudioSource>();
        if(src==null) src = gameObject.AddComponent<AudioSource>();
    }

    public void Play(string type){
        if(src==null) return;
        switch(type){
            case "toma": if(tomaShout) src.PlayOneShot(tomaShout); break;
            case "hit": if(hitSound) src.PlayOneShot(hitSound); break;
            case "cheer": if(cheerSound) src.PlayOneShot(cheerSound); break;
            case "catch": if(catchSound) src.PlayOneShot(catchSound); break;
        }
    }
}
