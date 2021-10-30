using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels; 
using UnityEngine.UI;


public class playfabma : MonoBehaviour
{
    public static string playername ;
    public InputField email;
    public InputField password;
    public InputField name;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void loadname()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),OndataRecieved ,OnError);
    }
    void OndataRecieved (GetUserDataResult result)
    {
        Debug.Log("recieved user Data");
        if (result.Data != null && result.Data.ContainsKey("Playername"))
        {
            playername = result.Data["Playername"].Value;
        }
    }
    public void savename()
    {
        playername = name.text;
        var request = new UpdateUserDataRequest {
            Data = new Dictionary<string, string>
            {
                {"Playername", name.text}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend ,OnError);
    }
    public void adminlogin()
    {
        var request = new LoginWithEmailAddressRequest {
            Email = email.text,
            Password = password.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request,OnSucces,OnError);
        
    }
    public void login()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucces, OnError);
    }
    public void logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    
    }
    void OnSucces (LoginResult result)
    {
        Debug.Log("Successful login/account create!");
    }
    void OnDataSend (UpdateUserDataResult result)
    {
        Debug.Log("Successful Send!! user Data");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("error while logging in/creating account");
        Debug.Log(error.GenerateErrorReport());
    }
}
