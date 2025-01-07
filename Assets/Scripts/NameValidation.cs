using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameValidation : MonoBehaviour
{
    public TMP_InputField nameInputField; //A text field that the player can type into
    public PlayFabManager playFabManager; //The script which manages the online leaderboards

    public void OnValueChanged() //When the player types into this input field
    {
        //Exception handling to detect non-letter characters is not need in this script because the 'Content Type' property of this input field has been set to 'Name' in the inspector
        //This only allows letters and spaces

        nameInputField.text = nameInputField.text.ToUpper(); //Convert the text to upper case
        nameInputField.text = nameInputField.text.Replace(" ", ""); //Remove any spaces in the text
        //Only upper case strings with no spaces may be entered into the text box
    }

    public void OnEndEdit() //When the user presses enter to confirm their username
    {
        if (nameInputField.text.Length == 3) //Only allow three letter, upper case names with no spaces
        {
            playFabManager.SubmitName(nameInputField.text); //Update the username associated with this device for the online leaderboard
        }
    }
    
}
