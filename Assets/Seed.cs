using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public Tree prefTree;
    private int[] DNA;
    private int treeTTL;
    [SerializeField]
    private int mutation = 15;//percent

    private Rigidbody rb = null;
    // Start is called before the first frame update

    public Seed(int[] a, int currentTTL)
    {
        DNA = (int[])a.Clone();
        for (int i = 0; i < DNA.Length; i++)
        {
            if (Random.Range(0, 100) >= 100 - mutation)
            {
                int k = Random.Range(0, DNA.Length);
                DNA[k] += -1 + 2 * Random.Range(0, 2);// -1 or 1
                DNA[k] = Mathf.Clamp(DNA[k], 0, Tree.countRules - 1);
            }
        }
        treeTTL = currentTTL;
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            //проверить, находится ли на земле
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
            {
                if (hit.transform.name.Equals("Terrain"))
                {
                    Tree tree = Instantiate(prefTree,
                       transform.position + Vector3.down, Quaternion.identity, null);
                    tree.DNA = this.DNA;
                    Destroy(this.transform.gameObject);
                }
            }
            if(transform.position.y < -100)
                Destroy(this.transform.gameObject);
        }
    }

    public void Drop()
    {
        transform.parent = null;
        rb = gameObject.AddComponent<Rigidbody>();
    }
}
