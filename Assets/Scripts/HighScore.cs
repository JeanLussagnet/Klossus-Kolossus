using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    public TMP_InputField input_name;
    public Button btnClick;
    private List<Transform> highscoreEntryTransformList;
    public Board board;
    public bool isHighscoreSceneLoaded;

    public HighScore(Board board)
    {

        this.board = board;

    }
    public void GetInputOnClickHandler()
    {
        string name = input_name.text;
        if (name.Length > 10)
        {
            name = name.Substring(0, 10);
        }
        else
            name = input_name.text;
        
        AddHighscoreEntry(board.FinalScore, name);
    }
    private void Awake()
    {
        if(isHighscoreSceneLoaded) {
        
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);
        

      
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores.highscoreEntryList != null)
        {
        highscores.highscoreEntryList = highscores.highscoreEntryList.OrderByDescending(x => x.score).Take(5).ToList();
        }




        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        }



    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTemplate.gameObject.SetActive(true);

        int rank = transformList.Count()+1;
        string rankString;
        switch (rank)
        {
            case 1: rankString = rank + "ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
            default:
                rankString = rank + "TH"; break;
        }
        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;


        int score = highScoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();


        string name = highScoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;

       entryTransform.Find("background").gameObject.SetActive(rank%2==1);
        transformList.Add(entryTransform);
    }

    public bool CheckIfHighscore(int score)
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        var list = highscores.highscoreEntryList.OrderBy(x => x.score).First();
        if (list.score < score)
        {
            return true;
        }

        return false; 
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highScoreEntry = new HighscoreEntry {score = score, name=name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            highscores = new Highscores();
        }
        if (highscores.highscoreEntryList == null)
        {
            highscores.highscoreEntryList = new List<HighscoreEntry>();
        }
        highscores.highscoreEntryList.Add(highScoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();


    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
    public int score;
    public string name;
    }
}