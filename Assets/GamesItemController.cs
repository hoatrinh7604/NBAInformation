using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamesItemController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI date;
    [SerializeField] TextMeshProUGUI period;
    [SerializeField] TextMeshProUGUI postSeason;
    [SerializeField] TextMeshProUGUI season;
    [SerializeField] TextMeshProUGUI homeTeam;
    [SerializeField] TextMeshProUGUI homeTeamScore;
    [SerializeField] TextMeshProUGUI visitorTeam;
    [SerializeField] TextMeshProUGUI visitorTeamScore;
    [SerializeField] TextMeshProUGUI status;
    [SerializeField] TextMeshProUGUI time;

    [SerializeField] Image imageBG;
    public void UpdateInfo(GameInfoData info, int typeColor)
    {
        id.text = info.id.ToString();
        date.text = ConvertDay(info.date);
        period.text = info.period.ToString();
        postSeason.text = info.postseason ? "Yes" : "No";
        season.text = info.season.ToString();
        homeTeam.text = info.home_team.name;
        homeTeamScore.text = info.home_team_score.ToString();
        visitorTeam.text = info.visitor_team.name;
        visitorTeamScore.text = info.visitor_team_score.ToString();
        status.text = info.status;
        time.text = info.time;

        ChangeBG(typeColor);
    }

    public string ConvertDay(string date)
    {
        DateTime dateTime = DateTime.MinValue;
        DateTime.TryParse(date, out dateTime);

        return dateTime.Day + "/" + dateTime.Month + "/" + dateTime.Year + " " + dateTime.Hour + ":" + dateTime.Minute + ":" + dateTime.Second;
    }

    private void ChangeBG(int type)
    {
        if (type == 0)
        {
            imageBG.color = new Color(0.37f, 0.47f, 0.99f, 0.7f);
        }
        else
        {
            imageBG.color = new Color(0.37f, 0.47f, 0.99f, 0.3f);
        }
    }
}
