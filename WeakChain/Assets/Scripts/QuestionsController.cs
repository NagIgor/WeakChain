using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionsController : IDisposable
{
    private Dictionary<int, QuestionModel> questions_;
    
    public QuestionsController()
    {
        LoadQuestions();
    }
    
    public void Dispose()
    {
    }
    
    public void LoadQuestions()
    {
        questions_ = new Dictionary<int, QuestionModel>();
        var payload = Resources.Load<TextAsset>("Questions");
        string[] questions = payload.text.Split(new string[] {",", "\n"}, StringSplitOptions.None);
        int tableSize = questions.Length / 3 - 1;
        for (int i = 0; i < tableSize; i++)
        {
            int id = int.Parse(questions[(i + 1) * 3]);
            
            if (PlayerPrefs.HasKey(id.ToString()))
            {
                continue;
            }
            
            var question = new QuestionModel();
            
            question.Id = id;
            question.Question = questions[(i + 1) * 3 + 1];
            question.Answer = questions[(i + 1) * 3 + 2];
            questions_.Add(question.Id, question);
        }
    }
    
    public QuestionModel GetRandomQuestion()
    {
        var random = new System.Random();
        var index = random.Next(questions_.Count);
        var question = questions_.ElementAt(index);
        SaveId(question.Key);
        questions_.Remove(question.Key);
        return question.Value;
    }
    
    void SaveId(int id)
    {
        PlayerPrefs.SetInt(id.ToString(), id);
        PlayerPrefs.Save();
    }
}
