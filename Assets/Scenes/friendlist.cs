using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class friendlist 
{
    public string masterusername ;
    public string titledisplayname;
    public string playertitleID ;
    public friendlist(string name,string displayname,string playertitleID)
    {
        this.masterusername = name;
        this.titledisplayname = displayname;
        this.playertitleID = playertitleID;
    }
}
public class friendlistmono : MonoBehaviour {

    string studentname;
    string displayname;
    string playertitleID;
    public friendlist returnclass(){
        return new friendlist(studentname,displayname,playertitleID);
    }

}
