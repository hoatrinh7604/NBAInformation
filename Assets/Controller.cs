using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using UnityEditor;
using SimpleJSON;

[System.Serializable]
public class TeamInfoData
{
    public int id = 0;
    public string abbreviation = "";
    public string city = "";
    public string conference = "";
    public string division = "";
    public string full_name = "";
    public string name = "";
}
[System.Serializable]
public class InfoMeta
{
    public int total_pages = 0;
    public int current_page = 0;
    public int next_page = 0;
    public int per_page = 0;
    public int total_count = 0;
}
[System.Serializable]
public class TeamInfoRoot
{
    public List<TeamInfoData> data = new List<TeamInfoData>();
    public InfoMeta meta = new InfoMeta();
}


// Player
[System.Serializable]
public class PlayerInfoData
{
    public int id = 0;
    public string first_name = "";
    public float height_feet = 0;
    public float height_inches = 0;
    public string last_name = "";
    public string position = "";
    public TeamInfoData team = new TeamInfoData();
    public float weight_pounds = 0;
}
[System.Serializable]
public class PlayerInfoRoot
{
    public List<PlayerInfoData> data = new List<PlayerInfoData>();
    public InfoMeta meta = new InfoMeta();
}
[System.Serializable]
public class GameInfoData
{
    public int id = 0;
    public string date = "";
    public TeamInfoData home_team = new TeamInfoData();
    public int home_team_score = 0;
    public int period = 0;
    public bool postseason = false;
    public int season = 0;
    public string status = "";
    public string time = "";
    public TeamInfoData visitor_team = new TeamInfoData();
    public int visitor_team_score = 0;
}
[System.Serializable]
public class GamesInfoRoot
{
    public List<GameInfoData> data = new List<GameInfoData> ();
    public InfoMeta meta = new InfoMeta();
}


public class Controller : MonoBehaviour
{
    [SerializeField] Button teamButton;
    [SerializeField] Button playerButton;
    [SerializeField] Button playerNextButton;
    [SerializeField] Button playerPreButton;

    [SerializeField] Button gamesButton;
    [SerializeField] Button gamesNextButton;
    [SerializeField] Button gamesPreButton;

    [SerializeField] GameObject teamResult;
    [SerializeField] GameObject teamInfoItem;
    [SerializeField] GameObject playerResult;
    [SerializeField] GameObject playerInfoItem;
    [SerializeField] GameObject gamesResult;
    [SerializeField] GameObject gamesInfoItem;

    [SerializeField] GameObject listFieldParent;
    [SerializeField] GameObject groupAnswer;

    [SerializeField] GameObject ball;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    [SerializeField] GameObject loading;
    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        //Screen.SetResolution(1920, 1080, false);
        Reset();

        teamButton.onClick.AddListener(delegate { GetTeams(); });
        playerButton.onClick.AddListener(delegate { GetPlayers(currentPlayerPage); });
        playerPreButton.onClick.AddListener(delegate { GetPlayers(prePlayerPage); });
        playerNextButton.onClick.AddListener(delegate { GetPlayers(nextPlayerPage); });
        
        gamesButton.onClick.AddListener(delegate { GetGames(currentGamesPage); });
        gamesPreButton.onClick.AddListener(delegate { GetGames(preGamesPage); });
        gamesNextButton.onClick.AddListener(delegate { GetGames(nextGamesPage); });
    }

    public void LoadData()
    {
        groupAnswer.SetActive(true);
        loading.SetActive(false);
    }

    [SerializeField] Transform teamContent;
    public void InitTeamItem(TeamInfoRoot data)
    {
        ball.SetActive(false);
        ClearContent(teamContent);

        if(teamInfo.data.Count == 0)
        {
            foreach(var item in data.data)
            {
                teamInfo.data.Add(item);
            }
        }

        for(int i = 0; i< data.data.Count; i++)
        {
            GameObject item = Instantiate(teamInfoItem, Vector2.zero, Quaternion.identity, teamContent);
            item.GetComponent<TeamInfoController>().UpdateInfo(data.data[i], i%2);
        }

        teamContent.GetComponent<ContentController>().UpdateSize(data.data.Count);
    }

    public void GetTeams()
    {
        ball.SetActive(true);
        HideAll();
        teamResult.SetActive(true);
        if(teamInfo.data.Count == 0)
        {
            StartCoroutine(GetAllTeam(InitTeamItem));
        }
        else
        {
            InitTeamItem(teamInfo);
        }
    }

    public void ClearContent(Transform tran)
    {
        for(int i = 0; i < tran.childCount; i++)
        {
            Destroy(tran.GetChild(i).gameObject);
        }

        tran.GetComponent<ContentController>().Clear();
    }

    private TeamInfoRoot teamInfo = new TeamInfoRoot();
    IEnumerator GetAllTeam(System.Action<TeamInfoRoot> callback)
    {
        var url = "https://free-nba.p.rapidapi.com/teams?page=0";

        UnityWebRequest wwwLogin = UnityWebRequest.Get(url);
        wwwLogin.SetRequestHeader("X-RapidAPI-Key", "c3fd0117a1msh0d5d37bad58477dp1e6225jsn4a447634f955");
        wwwLogin.SetRequestHeader("X-RapidAPI-Host", "free-nba.p.rapidapi.com");

        yield return wwwLogin.SendWebRequest();

        if (wwwLogin.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwwLogin.error);
        }
        else
        {
            teamInfo = JsonUtility.FromJson<TeamInfoRoot>(wwwLogin.downloadHandler.text);
            callback(teamInfo);
        }

    }

    #region Games Method
    [SerializeField] Transform gamesContent;
    private GamesInfoRoot gamesInfo = new GamesInfoRoot();
    private int currentGamesPage = 0;
    private int preGamesPage = 0;
    private int nextGamesPage = 0;

    public void PreGamesPage()
    {
        GetGames(preGamesPage);
    }

    public void NextGamesPage()
    {
        GetGames(nextGamesPage);
    }

    public void ControllPreGamesBtn(bool isInterable)
    {
        gamesPreButton.interactable = isInterable;
    }

    public void ControllNextGamesBtn(bool isInterable)
    {
        gamesNextButton.interactable = isInterable;
    }

    public void InitGamesItem(GamesInfoRoot data)
    {
        ball.SetActive(false);
        for (int i = 0; i < data.data.Count; i++)
        {
            GameObject item = Instantiate(gamesInfoItem, Vector2.zero, Quaternion.identity, gamesContent);
            item.GetComponent<GamesItemController>().UpdateInfo(data.data[i], i % 2);
        }

        gamesContent.GetComponent<ContentController>().UpdateSize(data.data.Count);
    }

    public void GetGames(int page)
    {
        ball.SetActive(true);
        HideAll();
        gamesResult.SetActive(true);
        ClearContent(gamesContent);
        StartCoroutine(GetAllGames(InitGamesItem, page));
    }

    IEnumerator GetAllGames(System.Action<GamesInfoRoot> callback, int page)
    {
        string url = "";
        if (page == 0)
        {
            url = "https://free-nba.p.rapidapi.com/games?per_page=50";
        }
        else
        {
            url = "https://free-nba.p.rapidapi.com/games?page=" + page + "&per_page=50";
        }


        UnityWebRequest wwwLogin = UnityWebRequest.Get(url);
        wwwLogin.SetRequestHeader("X-RapidAPI-Key", "c3fd0117a1msh0d5d37bad58477dp1e6225jsn4a447634f955");
        wwwLogin.SetRequestHeader("X-RapidAPI-Host", "free-nba.p.rapidapi.com");

        yield return wwwLogin.SendWebRequest();

        if (wwwLogin.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwwLogin.error);
        }
        else
        {
            gamesInfo = JsonUtility.FromJson<GamesInfoRoot>(wwwLogin.downloadHandler.text);
            currentGamesPage = gamesInfo.meta.current_page;

            preGamesPage = currentGamesPage - 1;
            ControllPreGamesBtn(preGamesPage < 1 ? false : true);
            if (preGamesPage < 1)
            {
                preGamesPage = 1;
            }

            ControllNextGamesBtn(currentGamesPage == gamesInfo.meta.total_pages ? false : true);
            nextGamesPage = gamesInfo.meta.next_page;
            if (currentGamesPage == gamesInfo.meta.total_pages)
                nextGamesPage = currentGamesPage;

            callback(gamesInfo);
        }

    }
    #endregion

    #region Player Method

    [SerializeField] Transform playerContent;
    private PlayerInfoRoot playerInfo = new PlayerInfoRoot();
    private int currentPlayerPage = 0;
    private int prePlayerPage = 0;
    private int nextPlayerPage = 0;

    public void PrePlayerPage()
    {
        GetPlayers(prePlayerPage);
    }

    public void NextPlayerPage()
    {
        GetPlayers(nextPlayerPage);
    }

    public void ControllPrePlayerBtn(bool isInterable)
    {
        playerPreButton.interactable = isInterable;
    }

    public void ControllNextPlayerBtn(bool isInterable)
    {
        playerNextButton.interactable = isInterable;
    }

    public void InitPlayerItem(PlayerInfoRoot data)
    {
        ball.SetActive(false);
        for (int i = 0; i < data.data.Count; i++)
        {
            GameObject item = Instantiate(playerInfoItem, Vector2.zero, Quaternion.identity, playerContent);
            item.GetComponent<PlayerItemController>().UpdateInfo(data.data[i], i % 2);
        }

        playerContent.GetComponent<ContentController>().UpdateSize(data.data.Count);
    }

    public void GetPlayers(int page)
    {
        ball.SetActive(true);
        HideAll();
        playerResult.SetActive(true);
        ClearContent(playerContent);
        StartCoroutine(GetAllPlayer(InitPlayerItem, page));
    }

    IEnumerator GetAllPlayer(System.Action<PlayerInfoRoot> callback, int page)
    {
        string url = "";
        if(page == 0)
        {
            url = "https://free-nba.p.rapidapi.com/players?per_page=50";
        }
        else
        {
            url = "https://free-nba.p.rapidapi.com/players?page=" + page + "&per_page=50";
        }
        

        UnityWebRequest wwwLogin = UnityWebRequest.Get(url);
        wwwLogin.SetRequestHeader("X-RapidAPI-Key", "c3fd0117a1msh0d5d37bad58477dp1e6225jsn4a447634f955");
        wwwLogin.SetRequestHeader("X-RapidAPI-Host", "free-nba.p.rapidapi.com");

        yield return wwwLogin.SendWebRequest();

        if (wwwLogin.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(wwwLogin.error);
        }
        else
        {
            playerInfo = JsonUtility.FromJson<PlayerInfoRoot>(wwwLogin.downloadHandler.text);
            currentPlayerPage = playerInfo.meta.current_page;

            prePlayerPage = currentPlayerPage-1;
            ControllPrePlayerBtn(prePlayerPage < 1?false:true);
            if (prePlayerPage < 1)
            {
                prePlayerPage = 1;
            }

            ControllNextPlayerBtn(currentPlayerPage == playerInfo.meta.total_pages?false:true);
            nextPlayerPage = playerInfo.meta.next_page;
            if(currentPlayerPage == playerInfo.meta.total_pages)
                nextPlayerPage = currentPlayerPage;

            callback(playerInfo);
        }

    }
    #endregion

    public string GetResultString()
    {
        return "";
    }
    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = GetResultString();
        //EditorGUIUtility.systemCopyBuffer = GetResultString();
        StartCoroutine(Copied());
    }

    IEnumerator Copied()
    {
        yield return new WaitForSeconds(1f);
    }

    public void Translate()
    {

    }

    public void HideAll()
    {
        teamResult.SetActive(false);
        playerResult.SetActive(false);
        gamesResult.SetActive(false);
    } 
    
    public void Reset()
    {
        //loading.SetActive(true);
        //listFieldParent.SetActive(false);
        HideAll();
        ClearContent(teamContent);
        ClearContent(playerContent);
        ClearContent(gamesContent);
        ball.SetActive(true);
    }

    public void Clear()
    {
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
