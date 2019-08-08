using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    private static Canvas instance;
    private Animator _animator;
    private AudioSource _clickSound;

    public GameObject EventSystem;

    public static Canvas Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(EventSystem);
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _clickSound = GetComponent<AudioSource>();
    }

    public void SlideMainMenuToSettings(bool reverse = false)
    {
        _animator.SetBool("showingSettings", !reverse);
    }

    public void PlayClickSound()
    {
        _clickSound.PlayOneShot(_clickSound.clip);
    }
}
