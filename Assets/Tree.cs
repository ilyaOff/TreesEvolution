using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tree : MonoBehaviour
{
    public static readonly int countRules = 6;
    public static readonly int lengthDNA = 50;    
    
    public readonly int maxTlmeToLive = 100;
    public readonly int currentTimeToLive = 30;
    [SerializeField]
    private int timeToLive = 30;

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
           
            switch (DNA[node.rules])
            {                
                case 0://UP
                    tmpBranchNodes.Add(AddNode(Vector3.up, node, node.transform.rotation));
                    break;
                case 1://Forvard
                    tmpBranchNodes.Add(AddNode(Vector3.forward, node, node.transform.rotation));
                    break; 
                case 2://Rigth
                    tmpBranchNodes.Add(AddNode(Vector3.right, node, node.transform.rotation));
                    break;
                case 3://Back
                    tmpBranchNodes.Add(AddNode(Vector3.back, node, node.transform.rotation));
                    break;
                case 4://Left
                    tmpBranchNodes.Add(AddNode(Vector3.left, node, node.transform.rotation));
                    break;
                case 5: //Full branch
                    tmpBranchNodes.Add(AddNode(Vector3.up, node, node.transform.rotation));
                    tmpBranchNodes.Add(AddNode(Vector3.forward, node, node.transform.rotation));
                    tmpBranchNodes.Add(AddNode(Vector3.right, node, node.transform.rotation * Quaternion.AngleAxis(90, transform.up)));
                    tmpBranchNodes.Add(AddNode(Vector3.back, node, node.transform.rotation * Quaternion.AngleAxis(180, transform.up)));
                    tmpBranchNodes.Add(AddNode(Vector3.left, node, node.transform.rotation * Quaternion.AngleAxis(-90, transform.up)));
                    break;
                default: break;
            }
            //branchNodes.Remove(node);
        }
        //удалить все null узлы
        tmpBranchNodes.RemoveAll(item => item == null);

        branchNodes.Clear();
        branchNodes = tmpBranchNodes;
    }
    Node AddNode(Vector3 pos, Node parent, Quaternion rot)
    {
        
        if (!CheckInstantiate(parent.transform.position, (parent.transform.localPosition + pos)))//ошибка при повороте
            return null;
        
       Node newNode = Instantiate(prefNode, Vector3.zero, rot, parent.transform)
                        .gameObject.GetComponent<Node>();
        newNode.transform.localPosition = pos;
        print((parent.rules).ToString() + " " +((parent.rules + 1) % (lengthDNA)).ToString());
        newNode.rules = (parent.rules + 1) % (lengthDNA);
        bodyNodes.Add(newNode);
        return newNode;
        //branchNodes.Add(newNode);
    }
    bool CheckInstantiate(Vector3 start, Vector3 dir)
    {      
        return !Physics.Raycast(start, dir, 2f);
    }
}
