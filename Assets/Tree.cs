using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree : MonoBehaviour
{
    private static int countRules = 6;
    private static int lengthDNA = 50;
    public int timeToLive = 30;
    public int maxTlmeToLive = 100;

    public Transform prefNode;
    [SerializeField]
    private int[] DNA;
    [SerializeField]
    List<Node> bodyNodes;
    List<Node> branchNodes;
     
    private float timer = 0;
    void Start()
    {
        DNA = new int[lengthDNA];
        for (int i = 0; i < lengthDNA; i++)
            DNA[i] = Random.Range(0, countRules);

        bodyNodes = new List<Node>();
        bodyNodes.Add(Instantiate(prefNode, transform.position, Quaternion.identity, this.transform).gameObject.GetComponent<Node>());
        branchNodes = new List<Node>();
        branchNodes.Add(bodyNodes[0]);
        bodyNodes[0].rules = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)
        {
            timer = 0;
            Tick();
        }
    }
    void Tick()
    {
        timeToLive--;
        if (timeToLive <= 0) Destroy(this.transform.gameObject);
        //int length = branchNodes.Count;
        List<Node> tmpBranchNodes = new List<Node>();
        foreach (Node node in branchNodes)
        { 
            switch(DNA[node.rules])
            {                
                case 0://UP
                    tmpBranchNodes.Add( AddNode(Vector3.up, node));
                    break;
                case 1://Forvard
                    tmpBranchNodes.Add(AddNode(Vector3.forward, node));
                    break; 
                case 2://Rigth
                    tmpBranchNodes.Add(AddNode(Vector3.right, node));
                    break;
                case 3://Back
                    tmpBranchNodes.Add(AddNode(Vector3.back, node));
                    break;
                case 4://Left
                    tmpBranchNodes.Add(AddNode(Vector3.left, node));
                    break;
                case 5: //Full branch
                    tmpBranchNodes.Add(AddNode(Vector3.up, node));
                    tmpBranchNodes.Add(AddNode(Vector3.forward, node));
                    tmpBranchNodes.Add(AddNode(Vector3.right, node));
                    tmpBranchNodes.Add(AddNode(Vector3.back, node));
                    tmpBranchNodes.Add(AddNode(Vector3.left, node));
                    break;
                default: break;
            }
            //branchNodes.Remove(node);
        }
        branchNodes.Clear();
        branchNodes = tmpBranchNodes;
    }
    Node AddNode(Vector3 pos, Node parent)
    {
        Node newNode = Instantiate(prefNode, parent.transform.position + pos, Quaternion.identity, parent.transform)
                        .gameObject.GetComponent<Node>();
       
        print((parent.rules).ToString() + " " +((parent.rules + 1) % (lengthDNA)).ToString());
        newNode.rules = (parent.rules + 1) % (lengthDNA);
        bodyNodes.Add(newNode);
        return newNode;
        //branchNodes.Add(newNode);
    }
}
