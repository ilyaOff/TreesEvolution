using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface Rules
//{ 
    
//}
// { Stop, MakeSeed, Grow, Branch };
public class Node : MonoBehaviour
{
    public Material m;
    public int rules;

    int powerConsumption = 2;
    int powerMake = 0;

    private void Start()
    {
        this.gameObject.GetComponent<Renderer>().material = new Material(m);
    }
    public void Tick()
    {
        
    }
}
