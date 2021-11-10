using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree : MonoBehaviour
{
    private static int countRules = 5;
    private static int lengthDNA = 50;
    public int timeToLive = 30;
    public int maxTlmeToLive = 100;

    public Transform prefNode;
    private int[] DNA;
    [SerializeField]
    List<Node> bodyNodes;
    List<Node> branchNodes;
     
    private float timer = 0;
    void Start()
    {
        DNA = new int[lengthDNA];
        for (int i = 0; i < lengthDNA; i++)
            DNA[i] = Random.Range(0, countRules*2);

        bodyNodes = new List<Node>();
        bodyNodes.Add(Instantiate(prefNode, transform.position, Quaternion.identity, this.transform).gameObject.GetComponent<Node>());
        branchNodes = new List<Node>();
        branchNodes.Add(bodyNodes[0]);
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
        if (timeToLive <= 0) return;
        //int length = branchNodes.Count;
        foreach (Node node in branchNodes)
        { 
            switch(node.rules)
            {                
                case 0://UP
                    AddNode(Vector3.up, node.transform, 0);
                    break;
                case 1://Forvard
                    AddNode(Vector3.forward, node.transform, 1);
                    break; 
                case 2://Rigth
                    AddNode(Vector3.right, node.transform, 2);
                    break;
                case 3://Back
                    AddNode(Vector3.back, node.transform, 3);
                    break;
                case 4://Left
                    AddNode(Vector3.left, node.transform, 4);
                    break;
                default: break;
            }
            branchNodes.Remove(node);
        }
    }
    void AddNode(Vector3 pos, Transform parent, int parentRules)
    {
        Node newNode = Instantiate(prefNode, parent.position + pos, Quaternion.identity, parent).gameObject.GetComponent<Node>();
        newNode.rules = (parentRules + 1) % (2 * countRules);
        bodyNodes.Add(newNode);
        branchNodes.Add(newNode);
    }
}
