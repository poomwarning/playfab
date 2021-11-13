using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.PfEditor;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.AuthenticationModels;
using PlayFab.ProfilesModels;
using System.Threading.Tasks;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;


// this code is playfabmanager can use in any secene
public class playfabma : MonoBehaviour
{
      List<friendlist> getname = new List<friendlist>();
      List<Progress> teacherstudentlist = new List<Progress>();
    //INT VALUE to ref in method
    [Header("UI")]
    public setprogress[] Setprogresses; //this is objectclass contained value (it can convert to json) stack into array becuz we have many progress each student
    friendlistmono friendobject;
    public static string playername ;
    public static string playertitieID;
    public static string localplayfabID;
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
                Id = playertitieID,
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
      
        //Debug.Log(JsonConvert.SerializeObject(studentsprogress));
        var request3 = new SetEntityProfilePolicyRequest
        {
           Entity = new PlayFab.ProfilesModels.EntityKey()
           {
               Id = playertitieID,
               Type = "title_player_account"
           },
           Statements = new List<EntityPermissionStatement>()
           {
               new EntityPermissionStatement()
               {
                   Action ="Read",
                   Effect = EffectType.Allow,
                   Resource = string.Format("pfrn:data--title_player_account!{0}/Profile/Objects/Playerjson",playertitieID),
                   Principal = "*",
                   Comment = "PlayerJson",
                   Condition = null
               }
           }
         
        };
        PlayFabProfilesAPI.SetProfilePolicy(request3,OnSetprofilepolicy,OnError);
        PlayFabDataAPI.SetObjects(request2,onSetobjectsend,OnError);
        PlayFabClientAPI.UpdateUserData(request,OnDataSend,OnError);
       // PlayFabClientAPI.UpdatePlayerStatistics(request,OnDataSend,OnError);
    }
    public void loadjson()
    {
        foreach(var x in getname)
        {
            
        
        var FriendEntityRequest = new PlayFab.ProfilesModels.GetEntityProfileRequest()
        {
            Entity = new PlayFab.ProfilesModels.EntityKey()
            {
                //Id =  getname[0].playertitleID ,
                Id = x.playertitleID,
                Type = "title_player_account"
            }
            //FriendEntityRequest.Entity = new PlayFab.ProfilesModels.EntityKey()
           

        };
        Debug.Log(x.playertitleID);
        PlayFabProfilesAPI.GetProfile(FriendEntityRequest,reciveobject,OnError); 
        }
        //PlayFabProfilesAPI.GetProfile(new PlayFab.ProfilesModels.GetEntityProfileRequest(),reciveobject,OnError); // get object from cilent to server (this can try friend to cilent server)
      // PlayFabClientAPI.GetUserData(new GetUserDataRequest(),recvicejson ,OnError); // get normal player data client to server
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
                Debug.Log(getname[x].masterusername);
                Debug.Log(x);
                x++;
                //Debug.Log(item.FriendPlayFabId);
              //  Debug.Log(getname[x].name);
             }
             Debug.Log("get friend successful");
            // Debug.Log("u have friend");
         };
         //Debug.Log(getname.Count);
         // Debug.Log(getname[0].masterusername);
     }
    public void getTitleData()
     {
         int loopx = 0;
         
          var bigoof = new GetTitlePlayersFromMasterPlayerAccountIdsRequest();
           bigoof.MasterPlayerAccountIds = new List<string>();
          foreach(var x in getname)
          {
            bigoof.MasterPlayerAccountIds.Add(getname[loopx].playertitleID);
            // {getname[loopx].playertitleID};
            loopx++;
          }
          
          
         PlayFabProfilesAPI.GetTitlePlayersFromMasterPlayerAccountIds(bigoof,recivetitleID,OnError);
     }
         void recivetitleID(GetTitlePlayersFromMasterPlayerAccountIdsResponse result)
     {
         //Debug.Log(result.TitleId);
         foreach(var x in getname)
         {
             x.playertitleID = result.TitlePlayerAccounts[x.masterusername].Id;
             Debug.Log(x.playertitleID);
         }
         //Debug.Log(result.TitlePlayerAccounts[getname[0].masterusername].Id);
         
        // getname[0].playertitleID = result.TitlePlayerAccounts[getname[0].masterusername].Id;
         //Debug.Log("get friend title ID"+getname[0].titledisplayname);
       //  getname[0].playertitleID = result.TitleId;
     }
     void reciveobject(GetEntityProfileResponse result)
     {
         
          if(result.Profile.Objects != null&& result.Profile.Objects.ContainsKey("Playerjson"))
          {
                  // Debug.Log(JsonConvert.DeserializeObject<Progress>(result.Profile.Objects["Playerjson"].EscapedDataObject));
                  var oof = JsonConvert.DeserializeObject<Progress>(result.Profile.Objects["Playerjson"].EscapedDataObject);
                  teacherstudentlist.Add(oof);
                  Debug.Log(teacherstudentlist.Count);
                //teacherstudentlist = JsonConvert.DeserializeObject<List<Progress>>(result.Profile.Objects["Playerjson"].EscapedDataObject);
              
              
               // teacherstudentlist = JsonConvert.DeserializeObject<List<Progress>>(result.Profile.Objects["Playerjson"].EscapedDataObject);
           /* for (int i = 0; i<Setprogresses.Length;i++)
            {
                Setprogresses[i].SetUi(teacherstudentlist[i]);
            }*/
            Debug.Log("recieved student object json data!");
          }
     }

     public void toframe()
     {
         for (int i = 0; i<Setprogresses.Length;i++)
            {
                Setprogresses[i].SetUi(teacherstudentlist[i]);
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
    public  void  login()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
      
        PlayFabClientAPI.LoginWithCustomID(request, OnSucces, OnError); 
        
       // PlayFabAuthenticationAPI.GetEntityToken(new PlayFab.AuthenticationModels.GetEntityTokenRequest(),onGettokenrepon,OnError);
    }
    public void logout()
    {
        PlayFabClientAPI.ForgetAllCredentials();
    
    }
    void OnGetcombineinforesult(GetPlayerCombinedInfoResult result)
    {
       playertitieID =  result.InfoResultPayload.AccountInfo.TitleInfo.TitlePlayerAccount.Id ;
       Debug.Log("CurrentTitleID : "+playertitieID);
    }
    void OnSucces (LoginResult result)
    {
        localplayfabID = result.PlayFabId;
        Debug.Log("playfabID :"+localplayfabID);
        Debug.Log("Successful login/account create!");
        PlayFabClientAPI.GetPlayerCombinedInfo(new GetPlayerCombinedInfoRequest
        {
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                 GetUserAccountInfo = true
            }
        },OnGetcombineinforesult,OnError);
    }
    void onSetobjectsend(SetObjectsResponse result)
    {
        
        Debug.Log(result.SetResults[0].ObjectName);
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
    void OnSetprofilepolicy(SetEntityProfilePolicyResponse result)
    {
        Debug.Log("Setpolicy complete");
        int x1 = 0;
        if( result.Permissions != null )
        {
            foreach(var x in result.Permissions)
            {
                if(result.Permissions[x1].Comment == "Playerjson")
                {
                    Debug.Log(string.Format("found{0}policy and its contained{1}",x1,result.Permissions[x1].Comment));
                };
                x1++;
            }
        }
        else
        {
            Debug.Log("cant find shit in policy");
        }
       
    }
}
