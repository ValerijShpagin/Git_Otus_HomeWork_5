using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiRestartButton : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] Canvas _canvas;

    private void Start()
    {
        _canvas.enabled = false;
    }

    private void Update()
    {
        CheckStatusPlayer();
    }

    private void CheckStatusPlayer()
    {
        if (_playerController.state == PlayerController.State.Dead)
            _canvas.enabled = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
