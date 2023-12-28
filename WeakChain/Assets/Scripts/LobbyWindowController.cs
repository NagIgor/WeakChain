using System;
using UnityEngine;

public class LobbyWindowController : IDisposable
{
    public static event Action OnStartButtonClicked;
    public static event Action OnBackButtonClicked;
    
    private LobbyWindowHierarchy hierarchy_;
    
    public LobbyWindowController(LobbyWindowHierarchy lobbyHierarchy)
    {
        hierarchy_ = lobbyHierarchy;
        GlobalModel.PlayersAmount = GlobalModel.MIN_PLAYERS;
        hierarchy_.Content.SetActive(false);  
        
        UpdateContent();
        
        hierarchy_.PlusButton.onClick.AddListener(PlusButtonClickHandler);
        hierarchy_.MinusButton.onClick.AddListener(MinusButtonClickHandler);
        hierarchy_.StartButton.onClick.AddListener(StartButtonClickHadler);
        hierarchy_.ResetButton.onClick.AddListener(ClearAllPlayerPrefs);
        hierarchy_.BackButton.onClick.AddListener(BackButtonClickHadler);
        
        RoundWindowController.OnBackButtonClicked += Show;
        MenuWindowController.OnStartButtonClicked += Show;
    }
    
    public void Dispose()
    {
        RoundWindowController.OnBackButtonClicked += Show;
        MenuWindowController.OnStartButtonClicked += Show;
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

    private void MinusButtonClickHandler()
    {
        GlobalModel.PlayersAmount--;
        if (GlobalModel.PlayersAmount < GlobalModel.MIN_PLAYERS)
        {
            GlobalModel.PlayersAmount = GlobalModel.MIN_PLAYERS;
        }
        
        UpdateContent();
    }
    
    private void PlusButtonClickHandler()
    {
        GlobalModel.PlayersAmount++;
        if (GlobalModel.PlayersAmount > GlobalModel.MAX_PLAYERS)
        {
            GlobalModel.PlayersAmount = GlobalModel.MAX_PLAYERS;
        }
        
        UpdateContent();
    }
    
    private void UpdateContent()
    {
        for (int i = 0; i < hierarchy_.InputFields.Length; i++)
        {
            hierarchy_.InputFields[i].gameObject.SetActive(i < GlobalModel.PlayersAmount);
        }
    }
    
    private void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    
    private void BackButtonClickHadler()
    {
        hierarchy_.Content.SetActive(false);
        OnBackButtonClicked?.Invoke();
    }
}
