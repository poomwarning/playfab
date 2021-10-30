using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class Progress 
{
    public string name;
    public string rank;
    public float year1;
    public float accurate;
    public Progress(string name,string rank,float year1,float accurate)
    {
        this.name = name;
        this.rank = rank;
        this.year1 = year1;
        this.accurate = accurate;
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
    public void SetUi(Progress progress)
    {
        nameInput.text = progress.name;
        rankInput.text =  progress.rank;
        year1progress.text = progress.year1.ToString();
        accurate.text = progress.accurate.ToString();
    
    }
}

