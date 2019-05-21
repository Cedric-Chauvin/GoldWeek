using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControler : MonoBehaviour {

    public AudioClip _poseCard;
    public AudioClip _pioche;
    public AudioClip _music;
    public static SoundControler _soundControler;

    private AudioSource _source;

    private void Awake()
    {
        if (_soundControler == null)
            _soundControler = this;
        else
            Destroy(_soundControler.gameObject);

        _source = GetComponent<AudioSource>();
        
        _source.clip = _music;
        _source.loop = true;
        _source.Play();
    }

    public void PlaySound(AudioClip sound)
    {
        _source.PlayOneShot(sound);
    }

}
