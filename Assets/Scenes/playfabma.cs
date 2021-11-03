using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.PfEditor;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.AuthenticationModels;
using PlayFab.ProfilesModels;

using UnityEngine.UI;
using Newtonsoft.Json;


// this code is playfabmanager can use in any secene
public class playfabma : MonoBehaviour
{
      List<friendlist> getname = new List<friendlist>();
    //INT VALUE to ref in method
    [Header("UI")]
    public setprogress[] Setprogresses; //this is objectclass contained value (it can convert to json) stack into array becuz we have many progress each student
    friendlistmono friendobject;
    public static string playername ;
    public InputField email;
    public InputField password;
    public InputField studentname;

    // void load name Get title data by get user Data from ClientAPI method (request,DATA,error)
       public void loadname()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),OndataRecieved ,OnError);
    }
    // this mothod will call value from server to contain in client 
    void OndataRecieved (GetUserDataResult result)
    {
        Debug.Log("recieved user Data");
        if (result.Data != null && result.Data.ContainsKey("Playername"))
        {
            playername = result.Data["Playername"].Value;
        }
    }
    // generate player title data and send to server
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
        var FriendEntityRequest = new PlayFab.ProfilesModels.GetEntityProfileRequest()
        {
            Entity = new PlayFab.ProfilesModels.EntityKey()
            {
                Id =  getname[0].playertitleID ,
                Type = "title_player_account"
            }
            //FriendEntityRequest.Entity = new PlayFab.ProfilesModels.EntityKey()
           

        };
        Debug.Log(getname[0].playertitleID);
        PlayFabProfilesAPI.GetProfile(FriendEntityRequest,reciveobject,OnError); 
       //PlayFabProfilesAPI.GetProfile(new PlayFab.ProfilesModels.GetEntityProfileRequest(),reciveobject,OnError); // get object from cilent to server (this can try friend to cilent server)
       //PlayFabClientAPI.GetUserData(new GetUserDataRequest(),recvicejson ,OnError); // get normal player data client to server
    }
    public void loadfriendDATA()
    {
       PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(),reciveFriendlist,OnError);
       
    }
     void reciveFriendlist(GetFriendsListResult result)
     {
         if(result.Friends != null)
         {
             int x = 0;
             
             foreach(var item in result.Friends)
             {
                  getname.Add(new friendlist(item.FriendPlayFabId,item.FriendPlayFabId,item.FriendPlayFabId));
                // getname.Add();
                 // getname[x].name =  item.FriendPlayFabId;    //result.Friends[x].FriendPlayFabId;
                 //getname[x].name 
                x++;
                Debug.Log(x);
                //Debug.Log(item.FriendPlayFabId);
                 
                
              //  Debug.Log(getname[x].name);
             }
             Debug.Log("get friend successful");
            // Debug.Log("u have friend");
         };
         //Debug.Log(getname.Count);
          Debug.Log(getname[0].name);
     }
    public void getTitleData()
     {
          var bigoof = new GetTitlePlayersFromMasterPlayerAccountIdsRequest();
          bigoof.MasterPlayerAccountIds = new List<string>(){getname[0].playertitleID};
          
         PlayFabProfilesAPI.GetTitlePlayersFromMasterPlayerAccountIds(bigoof,recivetitleID,OnError);
     }
         void recivetitleID(GetTitlePlayersFromMasterPlayerAccountIdsResponse result)
     {
         //Debug.Log(result.TitleId);
         Debug.Log(result.TitlePlayerAccounts[getname[0].name].Id);
         Debug.Log("get friend title ID"+getname[0].displayname);
         getname[0].playertitleID = result.TitlePlayerAccounts[getname[0].name].Id;
       //  getname[0].playertitleID = result.TitleId;
     }
     void reciveobject(GetEntityProfileResponse result)
     {
         
          if(result.Profile.Objects != null&& result.Profile.Objects.ContainsKey("Playerjson"))
          {
                List<Progress> studentsprogress = JsonConvert.DeserializeObject<List<Progress>>(result.Profile.Objects["Playerjson"].EscapedDataObject);
            for (int i = 0; i<Setprogresses.Length;i++)
            {
                Setprogresses[i].SetUi(studentsprogress[i]);
            }
            Debug.Log("recieved student object json data!");
          }
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
