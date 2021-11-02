using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.PfEditor;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.AuthenticationModels;

using UnityEngine.UI;
using Newtonsoft.Json;

public class playfabma : MonoBehaviour
{
    [Header("UI")]
    public setprogress[] Setprogresses;
    public static string playername ;
    public InputField email;
    public InputField password;
    public InputField studentname;
    
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
        playername = studentname.text;
        var request = new UpdateUserDataRequest {
            Data = new Dictionary<string, string>
            {
                {"Playername", studentname.text}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend ,OnError);
    }
    public  void sendjson()
    {
        List<SetObject> oof = new List<SetObject>();
        List<Progress> studentsprogress = new List<Progress>();
       foreach (var item in Setprogresses)
       {
           studentsprogress.Add(item.ReturnClass());
       }
        var request = new UpdateUserDataRequest{
            Data = new Dictionary<string, string>
            {
                {"Progress", JsonConvert.SerializeObject(studentsprogress)}
            }
        };
        var request2 = new SetObjectsRequest {
            
            /* CustomTags = new Dictionary<string, string>
            {
                {"Progress", JsonConvert.SerializeObject(studentsprogress)}
            }*/
            
            //Entity = new PlayFab.DataModels.EntityKey{Id = EntityKey}
            //Type="title_player_account"
            Entity = new PlayFab.DataModels.EntityKey
            {
                Id = "D9356F70E28857BC",
                Type = "title_player_account"
            }
            ,
            Objects = new List<SetObject>()
            {
                new SetObject()
                {
                    ObjectName = "Playerjson",
                    EscapedDataObject = JsonConvert.SerializeObject(studentsprogress)
                
                }
            }
    
        };
        Debug.Log(JsonConvert.SerializeObject(studentsprogress));
        
        PlayFabDataAPI.SetObjects(request2,onSetobjectsend,OnError);
        PlayFabClientAPI.UpdateUserData(request,OnDataSend,OnError);
       // PlayFabClientAPI.UpdatePlayerStatistics(request,OnDataSend,OnError);
    }
    public void loadjson()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),recvicejson ,OnError);
    }
    public void loadfriendDATA()
    {
       // PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest,)
       //PlayFabClientAPI.GetTitleData
    }
 
     void recvicejson(GetUserDataResult result)
    {
        Debug.Log("recieved student progress data!");
        if(result.Data != null && result.Data.ContainsKey("Progress")){
            List<Progress> studentsprogress = JsonConvert.DeserializeObject<List<Progress>>(result.Data["Progress"].Value);
            for (int i = 0; i<Setprogresses.Length;i++)
            {
                Setprogresses[i].SetUi(studentsprogress[i]);
            }
        }
    }
    public void adminlogin()
    {
        var request = new LoginWithEmailAddressRequest {
            Email = email.text,
            Password = password.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request,OnSucces,OnError);
    }
    public void teacherloginNopassword()
    {
        var request = new LoginWithEmailAddressRequest {
            Email = "teacher@mail.com",
            Password = "teacher@mail.com",
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
        PlayFabAuthenticationAPI.GetEntityToken(new PlayFab.AuthenticationModels.GetEntityTokenRequest(),onGettokenrepon,OnError);
    }
    public void logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    
    }
    void OnSucces (LoginResult result)
    {
        Debug.Log("Successful login/account create!");
    }
    void onSetobjectsend(SetObjectsResponse result)
    {
        Debug.Log(result.SetResults);
        Debug.Log("suck send Setobject");
    
    }
    void onGettokenrepon(GetEntityTokenResponse response)
    {
        Debug.Log(response.Entity.Id);
        Debug.Log(response.Entity.Type);
        Debug.Log("suck send Setobject");
    }
    void OnDataSend (UpdateUserDataResult result)
    {
        Debug.Log("Successful Send!! user Data");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("error to processed");
        Debug.Log(error.GenerateErrorReport());
    }
}
