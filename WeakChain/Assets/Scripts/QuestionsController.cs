using System;
using System.Collections.Generic;
using System.IO;
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
        var questions = ParseCSV("Questions");
        for (int i = 0; i < questions.Count; i++)
        {
            string stringId = questions[i][0];
            int intId = int.Parse(stringId);
            
            if (PlayerPrefs.HasKey(stringId))
            {
                continue;
            }
            
            var question = new QuestionModel();
            
            question.Id = intId;
            question.Question = questions[i][1];
            question.Answer = questions[i][2];
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

    public List<List<string>> ParseCSV(string fileName)
    {
        List<List<string>> data = new List<List<string>>();

        TextAsset csvData = Resources.Load<TextAsset>(fileName);
        if (csvData == null)
        {
            Debug.LogError("CSV file not found: " + fileName);
            return data;
        }

        string[] lines = csvData.text.Split('\n');

        if (lines.Length > 0)
        {
            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = SplitCSVLine(lines[i]);

                List<string> entry = new List<string>();
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    entry.Add(values[j]);
                }

                data.Add(entry);
            }
        }

        return data;
    }

    private string[] SplitCSVLine(string line)
    {
        List<string> values = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        foreach (char c in line)
        {
            if (c == ',' && !insideQuotes)
            {
                values.Add(currentField);
                currentField = "";
            }
            else if (c == '"')
            {
                insideQuotes = !insideQuotes;
            }
            else
            {
                currentField += c;
            }
        }

        values.Add(currentField);

        return values.ToArray();
    }
}
