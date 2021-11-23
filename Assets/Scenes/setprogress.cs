using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
[System.Serializable]
public class Progress 
{
    public string name;
    public string rank;
    public float year1;
    public List<stagestatfuck> statge ;
    public Dictionary<string ,string> dicktest ;
    public int [] arrayfucked;
    public float accurate;

    public Progress(string name,string rank,float year1,float accurate)
    {
        this.name = name;
        this.rank = rank;
        this.year1 = year1;
        this.accurate = accurate;
    }
    public Progress()
    {

    }
}
public class setprogress : MonoBehaviour {
    public InputField nameInput;
    public InputField rankInput;
    public InputField year1progress;
    public InputField accurate;
    public Progress ReturnClass(){
        return new Progress(nameInput.text,rankInput.text,float.Parse(year1progress.text),float.Parse(accurate.text));
    }
    private void Start() 
    {
        /*  var oof = new Progress();
        oof.stagestudentstat.namefuck = "1-2";
         oof.stagestudentstat.accfuck = 100;
         oof.stagestudentstat.time = 60;*/
    }
    public void SetUi(Progress progress)
    {
        nameInput.text = progress.name;
        rankInput.text =  progress.rank;
        year1progress.text = progress.year1.ToString();
        accurate.text = progress.accurate.ToString();
      
        
    
    }
}

