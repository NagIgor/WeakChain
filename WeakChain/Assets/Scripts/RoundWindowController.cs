using System;
using System.Linq;
using UnityEngine;

public class RoundWindowController : IDisposable
{
    public static event Action OnBackButtonClicked;
    
    private RoundWindowHierarchy hierarchy_;
    
    private QuestionsController questionsController_;
    private TimerController timerController_;
    
    private int selectedMilestoneIndex_;
    private bool isPaused_;
    private int currentBank_;
    
    public RoundWindowController(RoundWindowHierarchy lobbyHierarchy, TimerController timerController)
    {
        hierarchy_ = lobbyHierarchy;
        hierarchy_.Content.SetActive(false);
        questionsController_ = new QuestionsController();
        timerController_ = timerController;
        
        hierarchy_.BankButton.onClick.AddListener(BankButtonClickHandler);
        hierarchy_.YesButton.onClick.AddListener(YesButtonClickHandler);
        hierarchy_.NoButton.onClick.AddListener(NoButtonClickHadler);
        hierarchy_.BackButton.onClick.AddListener(BackButtonClickHadler);
        hierarchy_.PlayPauseButton.onClick.AddListener(PlayPauseButtonClickHadler);
        
        LobbyWindowController.OnStartButtonClicked += Show;
        timerController_.OnValueChanged += TimerValueChangedHandler;
    }

    public void Dispose()
    {
        LobbyWindowController.OnStartButtonClicked -= Show;
        timerController_.OnValueChanged -= TimerValueChangedHandler;
    }
    
    private void PlayPauseButtonClickHadler()
    {
       isPaused_ = !isPaused_;
       hierarchy_.InputBlocker.SetActive(isPaused_);
       hierarchy_.PlayIcon.SetActive(isPaused_);
       hierarchy_.PauseIcon.SetActive(!isPaused_);
       timerController_.Pause(isPaused_);
       if (!isPaused_)
       {
           float timer = GlobalModel.BASE_TIMER + (GlobalModel.PlayersAmount * 10);
           timerController_.SetTimer(timer);
       }
    }
    
    private void BackButtonClickHadler()
    {
        hierarchy_.Content.SetActive(false);
        OnBackButtonClicked?.Invoke();
    }

    private void Show()
    {
        isPaused_ = true;
        hierarchy_.Content.SetActive(true);
        ResetGame();
        
        UpdateContent(true);
    }
    
    private void NoButtonClickHadler()
    {
        selectedMilestoneIndex_ = 0;
        
        UpdateContent(true);
    }

    private void YesButtonClickHandler()
    {
        selectedMilestoneIndex_++;
        if(selectedMilestoneIndex_ >= hierarchy_.RoundMilestones.Length)
        {
            selectedMilestoneIndex_ = 0;
            StopGame();
        }
        
        UpdateContent(true);
    }
    
    private void BankButtonClickHandler()
    {
        currentBank_ += hierarchy_.RoundMilestones[selectedMilestoneIndex_].Price;
        if (currentBank_ > hierarchy_.RoundMilestones.Last().Price)
        {
            currentBank_ = hierarchy_.RoundMilestones.Last().Price;
        }
        
        selectedMilestoneIndex_ = 0;
        
        UpdateContent(false);
    }
    
    private void UpdateContent(bool showQuestion)
    {
        ShowMilestones();
        if (showQuestion)
            ShowQuestion();
        
        hierarchy_.Bank.text = currentBank_.ToString();
    }
    
    private void ShowMilestones()
    {
        for (int i = 0; i < hierarchy_.RoundMilestones.Length; i++)
        {
            var currentMilestone = hierarchy_.RoundMilestones[i];
            currentMilestone.Value.text = currentMilestone.Price.ToString(); 
            currentMilestone.Image.color = i > selectedMilestoneIndex_ ? Color.blue : Color.white;
        }
        
        hierarchy_.RoundMilestones[selectedMilestoneIndex_].Image.color = Color.green;
    }
    
    private void ShowQuestion()
    {
        var question = questionsController_.GetRandomQuestion();
        hierarchy_.Question.text = question.Question;
        hierarchy_.Answer.text = question.Answer;
    }
    
    private void StopGame()
    {
    }
    
    private void TimerValueChangedHandler()
    {
        TimeSpan time = TimeSpan.FromSeconds( timerController_.CurrentValue);
        string displayTime = time.ToString(@"mm\:ss");
        hierarchy_.Timer.text = displayTime;
    }
    
    private void ResetGame()
    {
        timerController_.CurrentValue = 0;
        selectedMilestoneIndex_ = 0;
        currentBank_ = 0;
        isPaused_ = true;
        questionsController_.LoadQuestions();
        hierarchy_.Timer.text = "";
    }
}
