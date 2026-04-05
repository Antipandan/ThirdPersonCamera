using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PauseGameManager : MonoBehaviour
{
    // singleton objekt
    // lägg endast gameObjects här som behöver speciell behandling när paus klickas. e.g om paus klickas och en komponent
    // "pausas" inte, så lägger du den här
    [SerializeField] private List<GameObject> objectsToPause;
    [SerializeField] private UnityEvent onGameResume;
    private Action onGamePause;
    private bool isPaused = false;
    private static PauseGameManager instance;
    

    public Action OnGamePause { get; set; }
    
    public bool IsPaused { get; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SubscribeAllPauseObjects();
        }
        else Destroy(this.gameObject);
    }

    private void SubscribeAllPauseObjects()
    {
        foreach (GameObject obj in objectsToPause)
        {
            if (obj.TryGetComponent<IPauseable>(out IPauseable pauseable))
            {
                onGamePause += pauseable.Pause;
            }
        }
    }
    
    public void OnPausePressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log($"PauseGame!");
        //xor
        isPaused ^= true;
        onGamePause?.Invoke();
        OnPause(isPaused ? 0f : 1f);
        if (!isPaused)
        {
            onGameResume.Invoke();
        }
    }

    private static void OnPause(float changeTimeScale)
    {
        Time.timeScale = Mathf.Min(Mathf.Abs(changeTimeScale), 1f);
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
