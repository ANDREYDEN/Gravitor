using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    private static Canvas instance;
    private Animator _animator;

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
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SlideMainMenuToSettings(bool reverse=false)
    {
        _animator.SetBool("showSettings", !reverse);
    }
}
