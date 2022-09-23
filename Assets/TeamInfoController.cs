using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamInfoController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI abbreviation;
    [SerializeField] TextMeshProUGUI city;
    [SerializeField] TextMeshProUGUI conference;
    [SerializeField] TextMeshProUGUI division;
    [SerializeField] TextMeshProUGUI fullname;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] Image imageBG;
    public void UpdateInfo(TeamInfoData data, int typeColor)
    {
        id.text = data.id.ToString();
        abbreviation.text = data.abbreviation;
        city.text = data.city;
        conference.text = data.conference;
        division.text = data.division;
        fullname.text = data.full_name;
        name.text = data.name;
        ChangeBG(typeColor);
    }

    private void ChangeBG(int type)
    {
        if(type == 0)
        {
            imageBG.color = new Color(0.93f, 0.36f, 0.36f, 0.7f);
        }
        else
        {
            imageBG.color = new Color(0.93f, 0.36f, 0.36f, 0.3f);
        }
    }
}
