using System;
using UnityEngine;

public class MenuWindowController : IDisposable
{
    public static event Action OnStartButtonClicked;
    private MenuWindowHierarchy hierarchy_;
    
    public MenuWindowController(MenuWindowHierarchy lobbyHierarchy)
    {
        hierarchy_ = lobbyHierarchy;
        hierarchy_.Content.SetActive(true);  
        
        hierarchy_.StartButton.onClick.AddListener(StartButtonClickHadler);
        
        LobbyWindowController.OnBackButtonClicked += Show;
    }
    
    public void Dispose()
    {
        LobbyWindowController.OnBackButtonClicked += Show;
    }
    
    private void Show()
    {
        hierarchy_.Content.SetActive(true);  
    }
    
    private void StartButtonClickHadler()
    {
        hierarchy_.Content.SetActive(false);
        OnStartButtonClicked?.Invoke();
    }
}
