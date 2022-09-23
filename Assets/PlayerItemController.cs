using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI id;
    [SerializeField] TextMeshProUGUI firstname;
    [SerializeField] TextMeshProUGUI heightFeet;
    [SerializeField] TextMeshProUGUI heightInches;
    [SerializeField] TextMeshProUGUI weightPounds;
    [SerializeField] TextMeshProUGUI lastName;
    [SerializeField] TextMeshProUGUI position;
    [SerializeField] TextMeshProUGUI team;
    [SerializeField] Image imageBG;
    public void UpdateInfo(PlayerInfoData data, int typeColor)
    {
        id.text = data.id.ToString();
        firstname.text = data.first_name;
        heightFeet.text = data.height_feet.ToString();
        heightInches.text = data.height_inches.ToString();
        weightPounds.text = data.weight_pounds.ToString();
        lastName.text = data.last_name;
        position.text = data.position;
        team.text = data.team.name;
        ChangeBG(typeColor);
    }

    private void ChangeBG(int type)
    {
        if (type == 0)
        {
            imageBG.color = new Color(0.98f, 0.84f, 0.45f, 0.7f);
        }
        else
        {
            imageBG.color = new Color(0.98f, 0.84f, 0.45f, 0.3f);
        }
    }
}
