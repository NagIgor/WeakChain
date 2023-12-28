using System;
using System.Collections.Generic;
using UnityEngine;

public class Executor : MonoBehaviour
{
    private List<IDisposable> disposables_ = new List<IDisposable>();
    
    void Start()
    {
        var mainUIHierarchy = FindObjectOfType<MainUIHierarchy>();
        var timerController = FindObjectOfType<TimerController>();
        disposables_.Add(new LobbyWindowController(mainUIHierarchy.Lobby));
        disposables_.Add(new RoundWindowController(mainUIHierarchy.Round, timerController));
        disposables_.Add(new MenuWindowController(mainUIHierarchy.Menu));
    }

    private void OnDestroy()
    {
        for (int i = 0; i < disposables_.Count; i++)
        {
            disposables_[i].Dispose();
        }
    }
}
