using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree : MonoBehaviour
{
    public static readonly int countRules = 7;
    public static readonly int lengthDNA = 50;    
    
    public readonly int maxTlmeToLive = 30;
    public readonly int currentTimeToLive = 30;
    [SerializeField]
    private int timeToLive;

    public Transform prefNode;
    public Transform prefSeed;
    
    public int[] DNA;
    [SerializeField]
    List<Node> bodyNodes;
    List<Node> branchNodes;
    List<Seed> seeds;

    //public Color startColor;
    //public Color endColor;

    private float timer = 0;
    void Start()
    {
        DNA = new int[lengthDNA];
        for (int i = 0; i < lengthDNA; i++)
            DNA[i] = Random.Range(0, countRules);
        

        seeds = new List<Seed>();

        bodyNodes = new List<Node>();
        bodyNodes.Add(Instantiate(prefNode, transform.position, Quaternion.identity, this.transform).gameObject.GetComponent<Node>());
       
        branchNodes = new List<Node>();
        branchNodes.Add(bodyNodes[0]);
        timeToLive = maxTlmeToLive;
        DNA[0] = 0;
        DNA[1] = 0;
        DNA[2] = 0;
        DNA[3] = 5;
        DNA[4] = 1;
        DNA[5] = 1;
        DNA[6] = 1;
        DNA[7] = 2;
        DNA[8] = 2;
        DNA[9] = 6;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer = 0;
            Tick();
        }
    }
    public void Tick()
    {
        timeToLive--;
        if (timeToLive <= 0) Dead();
        //int length = branchNodes.Count;
        List<Node> tmpBranchNodes = new List<Node>();
        foreach (Node node in branchNodes)
        {           
            switch (DNA[node.rules])
            {                
                case 0://UP
                    tmpBranchNodes.Add(AddNode(node.transform.up, node, node.transform.rotation));
                    break;
                case 1://Forvard
                    tmpBranchNodes.Add(AddNode(node.transform.forward, node, node.transform.rotation));
                    break; 
                case 2://Rigth
                    tmpBranchNodes.Add(AddNode(node.transform.right, node, node.transform.rotation));
                    break;
                case 3://Back
                    tmpBranchNodes.Add(AddNode(-node.transform.forward, node, node.transform.rotation));
                    break;
                case 4://Left
                    tmpBranchNodes.Add(AddNode(-node.transform.right, node, node.transform.rotation));
                    break;
                case 5: //Full branch
                    tmpBranchNodes.Add(AddNode(node.transform.up, node, node.transform.rotation));
                    tmpBranchNodes.Add(AddNode(node.transform.forward, node, node.transform.rotation));
                    tmpBranchNodes.Add(AddNode(node.transform.right, node, node.transform.rotation * Quaternion.AngleAxis(90, transform.up)));
                    tmpBranchNodes.Add(AddNode(-node.transform.forward, node, node.transform.rotation * Quaternion.AngleAxis(180, transform.up)));
                    tmpBranchNodes.Add(AddNode(-node.transform.right, node, node.transform.rotation * Quaternion.AngleAxis(-90, transform.up)));
                    break;
                case 6:
                    AddSeed(node);
                    node.rules += 1;
                    tmpBranchNodes.Add(node);
                    break;
                default: break;
            }
            //branchNodes.Remove(node);
        }
        //удалить все null узлы
        tmpBranchNodes.RemoveAll(item => item == null);
        
        branchNodes.Clear();
        branchNodes = tmpBranchNodes;
        GetOld();
    }

    private void AddSeed(Node node)
    {
        if (CheckInstantiate(node.transform.position, -node.transform.up))
        {
            seeds.Add(Instantiate(prefSeed, node.transform.position - node.transform.up,
                Quaternion.identity, null).GetComponent<Seed>());        }
       
    }

    Node AddNode(Vector3 pos, Node parent, Quaternion rot)
    {
        //Debug.DrawLine(parent.transform.position, parent.transform.position + pos, Color.red, 10f, false);
        if (!CheckInstantiate(parent.transform.position,  pos))
        {            
            return null;
        }
            
       
        Node newNode = Instantiate(prefNode, parent.transform.position + pos, rot, parent.transform)
                        .GetComponent<Node>();        
        //newNode.transform.localPosition = pos;        
        newNode.rules = (parent.rules + 1) % (lengthDNA);
        bodyNodes.Add(newNode);
        return newNode;
    }
    bool CheckInstantiate(Vector3 start, Vector3 dir)
    {      
        return !Physics.Raycast(start, dir, 2f);
    }
    void GetOld()
    {
        foreach (Node node in bodyNodes)
        {
            Material material = node.gameObject.GetComponent<Renderer>().material;
            float w = timeToLive / (float)maxTlmeToLive;
            //material.color = startColor* w + endColor*(1f- w);
            material.color = new Color(material.color.r + 10f / 255,
                                        material.color.g - 5f/255,
                                        material.color.b);
            //color.r ;
        }
    }
    void Dead()
    {
        foreach (Seed seed in seeds)
        {
            seed.Drop();
        }
        Destroy(this.transform.gameObject);
    }
}
