using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class baseGraphScript2 : MonoBehaviour
{
    public int size;
    public bool horAndVer, diag;
    //public vertexScript[,] vertices;
    public List<vertexScript> vertices;
    //public edgeScript[] edges;
    public List<edgeScript> edges;
    int edgesInArray;
    public int maxEdges;
    int maxPosEdges;
    public int maxEdgesPerVertex; //4 = N,S,E,W | 8 = N,S,E,W,NW,NE,SE,SW
    public GameObject vertexGO;
    public GameObject edgeGO;
    public float edgeSpawnRate;
    public List<edgeScript> MST;
    public int vertexNumbering = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(horAndVer && diag)
        {
            maxPosEdges = ((size-1)*(size+size)) + (((size-1)*(size-1))*2);
        }

        else if(horAndVer)
        {
            maxPosEdges = (size-1)*(size+size);
        }

        else if(diag)
        {
            maxPosEdges = ((size-1)*(size-1))*2;
        }

        else
        {
            maxPosEdges = ((size-1)*(size+size)) + (((size-1)*(size-1))*2);
        }


        if(maxEdges == 0 || maxEdges > maxPosEdges)
        {
            maxEdges = maxPosEdges;
        }

        vertices = new List<vertexScript>();
        initVertices();
        displayVertices();
        setVertexNames();
        positionCamera();
        //edges = new edgeScript[maxEdges];
        edges = new List<edgeScript>();
        edgesInArray = 0;
        initEdges();
        //printVerticesStatus();
        MST = kruskalMST(edges);
        updateEdges(MST);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void initVertices()
    {
        //vertexScript v = vertexGO.GetComponent<vertexScript>();

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                vertexScript v = vertexGO.GetComponent<vertexScript>();
                //vertices[i,j] = v;
                v.setActive(true);
                vertices.Add(v);
                //vertices[i,j].setActive(true);
                //vertices[vertices.Count-1].setActive(true);
            } 
        }
    }

    // void printVerticesStatus()
    // {
    //     for(int i = 0; i < size; i++)
    //     {
    //         for(int j = 0; j < size; j++)
    //         {
    //             vertices[i,j].printStatus();
    //         }
    //     }
    // }

    void displayVertices()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                GameObject vertex = Instantiate(vertexGO, new Vector3(j, -i, 0), Quaternion.identity);
                vertex.name = i + "," + j;
                vertexScript vs = vertex.GetComponent<vertexScript>();
                vs.setName(vertexNumbering);
                vertexNumbering++;
            }
        }
    }

    void positionCamera()
    {
        Camera.main.transform.position = new Vector3((size-1f)/2f, -(size-1f)/2f, Camera.main.transform.position.z);
        Camera.main.orthographicSize *= (size/Camera.main.orthographicSize);
    }

    void setVertexNames()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                string name = i + "," + j;
                //Debug.Log(name);
                GameObject v = GameObject.Find(name);
                vertexScript vs = v.GetComponent<vertexScript>();
                vs.setVertexNum(i,j);
            }
        }
    }

    void initEdges()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                string name = i + "," + j;
                GameObject v = GameObject.Find(name);
                vertexScript vs = v.GetComponent<vertexScript>();

                if(i > 0 && horAndVer &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add North Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3(j, (-i+0.5f), 0), Quaternion.Euler(0,0,90));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),j);
                    if(!edgeExists(e))
                    {
                        string nameB = (i-1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i-1),j,edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j > 0 && diag &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add North-West Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,-45));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = (i-1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i-1),(j-1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j < size-1 && diag &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add North-East Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,45));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = (i-1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i-1),(j+1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1  && horAndVer &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add South Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3(j, (-i-0.5f), 0), Quaternion.Euler(0,0,90));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),j);
                    if(!edgeExists(e))
                    {
                        string nameB = (i+1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i+1),j,edge,e,vs,vsB);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j > 0 && diag &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add South-West Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,45));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = (i+1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i+1),(j-1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j < size-1 && diag &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add South-East Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,-45));
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = (i+1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,(i+1),(j+1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j > 0 && horAndVer &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add West Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j-0.5f), -i, 0), Quaternion.identity);
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = i + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,i,(j-1),edge,e,vs,vsB);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j < size-1 && horAndVer &&  Random.Range(0.0f, 1.0f) <= edgeSpawnRate)//Add East Edge
                {
                    GameObject edge = Instantiate(edgeGO, new Vector3((j+0.5f), -i, 0), Quaternion.identity);
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = i + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(i,j,i,(j+1),edge,e,vs,vsB);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                //vertices[i,j] = vs;
            }
        }
    }

    void createEdge(int i1 , int j1, int i2, int j2, GameObject e, edgeScript es, vertexScript vsA, vertexScript vsB)
    {
        e.name = "[" + i1 + "," + j1 + "]-[" + i2 + "," + j2 + "]";
        es.setWeight(Random.Range(1,101));
        es.setA(vsA.getName());
        es.setB(vsB.getName());
        //edges[edgesInArray] = es;
        edges.Add(es);
        edgesInArray++;
        vsA.addEdge(es);
    }

    bool edgeExists(edgeScript e)
    {
        bool exists = false;

        for(int i = 0; i < edges.Count; i++)
        {
            if(edges[i] != null)
            {
                if(e.isTheSame(edges[i]))
                {
                    exists = true;
                }

                else if(e.isMirroredEdge(edges[i]))
                {
                    exists = true;
                }
            }
        }

        return exists;
    }

    public class subset 
    {
       public int parent, rank;
    };

    List<edgeScript> kruskalMST(List<edgeScript> edges)
    {
        List<edgeScript> result = new List<edgeScript>();

        int e = 0;
        int i = 0;

        List<edgeScript> sortedEdges = edges.OrderBy(se=>se.getWeight()).ToList();

        subset[] subsets = new subset[size*size];
        
        for(i = 0; i < (size*size); i++)
        {
            subsets[i] = new subset();
        }

        for(int v = 0; v < (size*size); v++)
        {
            subsets[v].parent = v;
            subsets[v].rank = 0;
        }

        i=0;
        
        while(e < (size*size)-1)
        {
            edgeScript next_edge = sortedEdges[i++];   

            int x = find(subsets, next_edge.getA());
            int y = find(subsets, next_edge.getB());

            if(x != y)
            {
                result.Add(next_edge);
                e++;
                union(subsets, x, y);
            }
        }

        // int minimumCost = 0;
        // for(i = 0; i < e; i++)
        // {
        //     Debug.Log(result[i].getA() + " -- " + result[i].getB() + " == " + result[i].weight);
        //     minimumCost += result[i].weight;
        // }

        return result;

    }

    int find(subset[] subsets, int i)
    {
        if(subsets[i].parent != i)
        {
            subsets[i].parent = find(subsets, subsets[i].parent);
        }

        return subsets[i].parent;
    }

    void union(subset[] subsets, int x, int y)
    {
        int xroot = find(subsets, x);
        int yroot = find(subsets, y);

        if(subsets[xroot].rank < subsets[yroot].rank)
        {
            subsets[xroot].parent = yroot;
        }

        else if(subsets[xroot].rank > subsets[yroot].rank)
        {
            subsets[yroot].parent = xroot;
        }

        else
        {
            subsets[yroot].parent = xroot;
            subsets[xroot].rank++;
        }
    }

    void updateEdges(List<edgeScript> edgesToShow)
    {
        foreach (edgeScript edge in edges)
        {
            if(!edgesToShow.Contains(edge))
            {
                string GOname = edge.printStatus();
                GameObject edgeToDestroy = GameObject.Find(GOname);
                Destroy(edgeToDestroy);
            }
        }
    }

}
