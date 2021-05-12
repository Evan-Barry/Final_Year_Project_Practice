using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

public class baseGraphScript2 : MonoBehaviour
{
    public int size;
    public bool horAndVer, diag;
    //public vertexScript[,] vertices;
    public List<vertexScript> vertices;
    public List<vertexScript> borderVertices;
    public bool startingPointSet = false;
    public vertexScript startingPoingScript;
    //public edgeScript[] edges;
    public List<edgeScript> edges;
    int edgesInArray;
    public int maxEdges;
    int maxPosEdges;
    public int maxEdgesPerVertex; //4 = N,S,E,W | 8 = N,S,E,W,NW,NE,SE,SW
    public GameObject vertexGO;
    public GameObject edgeGO;
    public GameObject startingPointGO;
    public List<edgeScript> MST;
    public List<edgeScript> edgesFromGAN;
    public List<edgeScript> disgardedEdges;
    public int vertexNumbering = 0;
    public float edgeResinsertionRate = 1f;
    public int graphNo;

    public GameObject[] vertexGOs;
    public bool isConnected = true;
    public List<edgeScript> activeEdges;


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

        clearFolder();
        vertices = new List<vertexScript>();
        initVertices();
        displayVertices();
        setVertexNames();
        //edges = new edgeScript[maxEdges];
        edges = new List<edgeScript>();
        edgesInArray = 0;

        //USe the block of code below to generate real base graphs with MST

        ////////////////////////////////
        //initEdges();
        //MST = kruskalMST(edges);
        //updateDisgardedEdges(MST);
        //updateEdges(MST);
        //edgeResinsertion();
        ////////////////////////////////

        //Use the block of code below to generate base graphs from GANs

        /////////////////////////////////////////
        edgesFromGAN = initEdgesFromFile();
        updateEdges(edgesFromGAN);
        /////////////////////////////////////////


        //StartCoroutine(exportBaseGraphData());
        //repositionVertices();
        //repositionEdges();
        //InstantiateSegments();
        //showStartingPoint();
        //checkIfVertexIsConnected();
        //getShortestPath();
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     edgeResinsertion();
        // }
    }

    void initVertices()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                vertexScript v = vertexGO.GetComponent<vertexScript>();
                v.setActive(true);
                vertices.Add(v);
            } 
        }
    }

    void displayVertices()
    {
        baseGraphData baseGraph = new baseGraphData();
        baseGraph.size = this.size;

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                GameObject vertex = Instantiate(vertexGO, new Vector3 (transform.position.x+j, transform.position.y-i, 0), Quaternion.identity);
                vertex.name = graphNo + " - " + i + "," + j;
                vertex.transform.parent = gameObject.transform;
                vertexScript vs = vertex.GetComponent<vertexScript>();
                if((graphNo == 1) && (i == 0 || i == size-1 || j == 0 || j == size-1))
                {
                    if(!startingPointSet && UnityEngine.Random.Range(0f,1f) <= 0.5f)
                    {
                        vs.setStartingPoint(true);
                        startingPointSet = true;
                        startingPointGO = vs.gameObject;
                        startingPoingScript = vs;
                    }
                    borderVertices.Add(vs);
                }
                vs.setName(vertexNumbering);
                vertexNumbering++;
            }
        }
    }

    void setVertexNames()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                string name = graphNo + " - " + i + "," + j;
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
                string name = graphNo + " - " + i + "," + j;
                GameObject v = GameObject.Find(name);
                vertexScript vs = v.GetComponent<vertexScript>();

                if(i > 0 && horAndVer)//Add North Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3(j, (-i+0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size), (-i+0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),j);
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),j,edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j > 0 && diag)//Add North-West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),(j-1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j < size-1 && diag)//Add North-East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),(j+1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1  && horAndVer)//Add South Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3(j, (-i-0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3(j+size, (-i-0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),j);
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),j,edge,e,vs,vsB);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j > 0 && diag)//Add South-West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),(j-1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j < size-1 && diag)//Add South-East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),(j+1),edge,e,vs,vsB);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j > 0 && horAndVer)//Add West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), -i, 0), Quaternion.identity);
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), -i, 0), Quaternion.identity);
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j-1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + i + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,i,(j-1),edge,e,vs,vsB);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j < size-1 && horAndVer)//Add East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), -i, 0), Quaternion.identity);
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), -i, 0), Quaternion.identity);
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j+1));
                    if(!edgeExists(e))
                    {
                        string nameB = graphNo + " - " + i + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,i,(j+1),edge,e,vs,vsB);
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

    List<edgeScript> initEdgesFromFile()
    {
        List<edgeScript> edges = new List<edgeScript>();

        string path;

        if(graphNo == 1)
        {
            path = "Assets/vertices1.json";
        }
        else
        {
            path = "Assets/vertices2.json";
        }
        string[] vertexData;
        int[][] vertexDataInt;
        StreamReader reader = new StreamReader(path);
        string verticesFromFile = reader.ReadToEnd();
        reader.Close();
        verticesFromFile = verticesFromFile.Substring(1, verticesFromFile.Length-2);
        vertexData = verticesFromFile.Split(']');

        vertexDataInt = new int[vertexData.Length-1][];

        for(int i = 0; i < vertexData.Length; i++)
        {
            vertexData[i] = vertexData[i].Trim(' ',',','[');

            if(i < vertexData.Length-1)
            {
                vertexDataInt[i] = stringToInt(vertexData[i]);
            }
        }

        int counter = 0;

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                string name = graphNo + " - " + i + "," + j;
                GameObject v = GameObject.Find(name);
                vertexScript vs = v.GetComponent<vertexScript>();

                if(i > 0 && horAndVer)//Add North Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3(j, (-i+0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size), (-i+0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),j);
                    if(vertexDataInt[counter][1] == 1)
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),j,edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j > 0 && diag)//Add North-West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j-1));
                    if(vertexDataInt[counter][0] == 1)
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),(j-1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i > 0 && j < size-1 && diag)//Add North-East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), (-i+0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i-1),(j+1));
                    if(vertexDataInt[counter][2] == 1)
                    {
                        string nameB = graphNo + " - " + (i-1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i-1),(j+1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1  && horAndVer)//Add South Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3(j, (-i-0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3(j+size, (-i-0.5f), 0), Quaternion.Euler(0,0,90));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),j);
                    if(vertexDataInt[counter][6] == 1)
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + j;
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),j,edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j > 0 && diag)//Add South-West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j-1));
                    if(vertexDataInt[counter][5] == 1)
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),(j-1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(i < size-1 && j < size-1 && diag)//Add South-East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), (-i-0.5f), 0), Quaternion.Euler(0,0,-45));
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,(i+1),(j+1));
                    if(vertexDataInt[counter][7] == 1)
                    {
                        string nameB = graphNo + " - " + (i+1) + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,(i+1),(j+1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j > 0 && horAndVer)//Add West Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j-0.5f), -i, 0), Quaternion.identity);
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size-0.5f), -i, 0), Quaternion.identity);
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j-1));
                    if(vertexDataInt[counter][3] == 1)
                    {
                        string nameB = graphNo + " - " + i + "," + (j-1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,i,(j-1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                if(j < size-1 && horAndVer)//Add East Edge
                {
                    GameObject edge;

                    if(graphNo == 1)
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+0.5f), -i, 0), Quaternion.identity);
                    }
                    else
                    {
                        edge = Instantiate(edgeGO, new Vector3((j+size+0.5f), -i, 0), Quaternion.identity);
                    }
                    
                    edge.transform.parent = v.transform;
                    edgeScript e = edge.GetComponent<edgeScript>();
                    e.setVertices(i,j,i,(j+1));
                    if(vertexDataInt[counter][4] == 1)
                    {
                        string nameB = graphNo + " - " + i + "," + (j+1);
                        GameObject vB = GameObject.Find(nameB);
                        vertexScript vsB = vB.GetComponent<vertexScript>();
                        createEdge(graphNo,i,j,i,(j+1),edge,e,vs,vsB);
                        edges.Add(e);
                    }
                    
                    else
                    {
                        Destroy(edge);
                    }
                }

                counter++;
            }
        }

        return edges;

    }

    int[] stringToInt(string s)
    {
        string[] split = s.Split(',');
        int[] ints = new int[split.Length];

        for(int i = 0; i < split.Length; i++)
        {
            ints[i] = Int32.Parse(split[i].Trim()[0].ToString());
        }

        return ints;
    }

    void createEdge(int graphNo, int i1 , int j1, int i2, int j2, GameObject e, edgeScript es, vertexScript vsA, vertexScript vsB)
    {
        e.name = graphNo + " - [" + i1 + "," + j1 + "]-[" + i2 + "," + j2 + "]";
        //e.SetActive(false);
        e.GetComponent<Renderer>().enabled = false;
        es.setWeight(UnityEngine.Random.Range(1,101));
        es.setA(vsA.getName());
        es.setB(vsB.getName());
        es.setGraphNo(graphNo);
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

    int minDistance(int[] dist, bool[] spt)
    {
        int min = int.MaxValue;
        int min_index = -1;

        for(int i = 0; i < (size*size); i++)
        {
            if(spt[i] == false && dist[i] <= min)
            {
                min = dist[i];
                min_index = i;
            }
        }
        return min_index;
    }

    void printDijkstraSolution(int[] dist)
    {
        Debug.Log("Vertex \t\t Distance from source\n");
        for(int i = 0; i < (size*size); i++)
        {
            Debug.Log(i + " \t\t " + dist[i] + "\n");
        }
    }
    void disjkstra(int[,] graph, int src)
    {
        int[] dist = new int[(size*size)];
        bool[] spt = new bool[(size*size)];

        for(int i = 0; i < (size*size); i++)
        {
            dist[i] = int.MaxValue;
            spt[i] = false;
        }

        dist[src] = 0;

        for(int count = 0; count < (size*size); count++)
        {
            int i = minDistance(dist, spt);
            spt[i] = true;

            for(int j = 0; j < (size*size); j++)
            {
                if(!spt[j] && graph[i,j] != 0 && dist[i] != int.MaxValue && dist[i] + graph[i,j] < dist[j])
                {
                    dist[j] = dist[i] + graph[i,j];
                }
            }
        }
        printDijkstraSolution(dist);
    }

    void getShortestPath()
    {
        int[,] graphWeights = new int[(size*size),8];
        activeEdges = new List<edgeScript>();

        foreach (edgeScript es in edges)
        {
            if(es.gameObject.GetComponent<MeshRenderer>().enabled == true)
            {
                activeEdges.Add(es);
            }
        }

        for(int i = 0; i < (size*size); i++)
        {
            int[] vertexWeights = vertices[i].gameObject.GetComponent<vertexScript>().getWeights();
            for(int j = 0; j < 8; j++)
            {
                graphWeights[i,j] = vertexWeights[j];
                //Debug.Log(graphWeights[i,j]);
            }
        }
    }

    void updateEdges(List<edgeScript> edgesToShow)
    {
        foreach (edgeScript edge in edges)
        {
            string GOname = edge.printStatus();
            GameObject edgeGO = GameObject.Find(GOname);

            if(edgeGO != null)
            {
                if(edgesToShow.Contains(edge))
                {
                    //Debug.Log("Turn on edge");
                    //edgeGO.SetActive(true);
                    edgeGO.GetComponent<Renderer>().enabled = true;
                    edgeGO.GetComponent<edgeScript>().setActive(true);
                }
            }

        }
    }

    void updateDisgardedEdges(List<edgeScript> enabledEdges)
    {
        foreach(edgeScript edge in edges)
        {
            string GOname = edge.printStatus();
            GameObject edgeGO = GameObject.Find(GOname);

            if(edgeGO != null)
            {
                if(!enabledEdges.Contains(edge))
                {
                    disgardedEdges.Add(edgeGO.GetComponent<edgeScript>());
                }
            }
        }
    }

    void edgeResinsertion()
    {
        List<edgeScript> reinsertedEdges = new List<edgeScript>();

        for(int i = 0; i < disgardedEdges.Count; i++)
        {
            if(UnityEngine.Random.Range(0.0f,1.0f) < edgeResinsertionRate)
            {
                MST.Add(disgardedEdges[i]);
                reinsertedEdges.Add(disgardedEdges[i]);
                disgardedEdges.RemoveAt(i);
            }
        }
        updateEdges(MST);
    }

    public void setGraphNo(int num)
    {
        graphNo = num;
    }

    public void clearFolder()
    {
        if(Directory.Exists("Assets/baseGraphJSONs/"))
        {
            string[] files = Directory.GetFiles("Assets/baseGraphJSONs/");

            for(int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }
        }
    }

    public IEnumerator exportBaseGraphData()
    {
        yield return new WaitForSeconds(1);
        baseGraphData baseGraph = new baseGraphData();
        baseGraph.size = this.size;

        GameObject[] vertexGOs = new GameObject[transform.childCount];

        //vertexGOs = GameObject.FindGameObjectsWithTag("vertex");
        for(int i = 0; i < transform.childCount; i++)
        {
            vertexGOs[i] = transform.GetChild(i).gameObject;
        }

        foreach (GameObject vertex in vertexGOs)
        {
            vertexData vertexData = new vertexData();
            vertexScript vs = vertex.GetComponent<vertexScript>();
            GameObject[] edgeGOs = new GameObject[vertex.transform.childCount];
            //Debug.Log("This vertex has " + vertex.transform.childCount + " children");

            for(int i = 0; i < vertex.transform.childCount; i++)
            {
                edgeGOs[i] = vertex.transform.GetChild(i).gameObject;
            }

            bool[] activeEdges = new bool[8];

            for(int j = 0; j < edgeGOs.Length-1; j++)
            {
                //Debug.Log("J - " + j + ", edgesGOs Length - " + edgeGOs.Length);
                if(edgeGOs[j].GetComponent<MeshRenderer>().enabled == true)
                {
                    if(vs.isEdgeConnected(edgeGOs[j].GetComponent<edgeScript>()) != -1)
                    {
                        activeEdges[vs.isEdgeConnected(edgeGOs[j].GetComponent<edgeScript>())] = true;
                    }
                }
            }

            vertexData.setVertexData(vs.getPos(), activeEdges, vs.getStartingPoint(), vs.getObjectivePoint());
            baseGraph.addVertex(vertexData);
        }

        string baseGraphString = JsonUtility.ToJson(baseGraph);

        DirectoryInfo dir = new DirectoryInfo("Assets/baseGraphJSONs/");

        FileInfo[] fileInfo = dir.GetFiles();

        System.IO.File.WriteAllText("Assets/baseGraphJSONs/BaseGraphData" + fileInfo.Count() + ".json", baseGraphString);
        baseGraphString = "";
        baseGraph.clear();
    }

    // public void repositionVertices()
    // {
    //     vertexGOs = new List<GameObject>();

    //     foreach(Transform child in transform)
    //     {
    //         if(child.GetComponent<vertexScript>())
    //         {
    //             vertexGOs.Add(child.gameObject);
    //         }
    //     }

        

    //     //Debug.Log("This base graph has " + vertexGOs.Count + " vertices");

    //     foreach(GameObject vertex in vertexGOs)
    //     {
    //         vertex.transform.position = new Vector2(vertex.transform.position.x + Random.Range(-0.5f,0.5f), vertex.transform.position.y + Random.Range(-0.5f,0.5f));
    //     }
    // }

    // public void repositionEdges()
    // {
    //     foreach(GameObject vertex in vertexGOs)
    //     {
    //         foreach (Transform edge in vertex.transform)
    //         {
    //             if(edge.gameObject.GetComponent<MeshRenderer>().enabled)
    //             {
    //                 edgeScript es = edge.gameObject.GetComponent<edgeScript>();
    //                 //Debug.Log("Edge - "+ graphNo + " " + es.getAR() + es.getAC() + " " + es.getBR() + es.getBC());
    //                 string vertexBname = graphNo.ToString() + " - [" + es.getAR() + "," + es.getAC() + "]-[" + es.getBR() + "," + es.getBC() + "]";
    //                 foreach (GameObject vB in vertexGOs)
    //                 {
    //                     if(vB.name.Equals(vertexBname))
    //                     {
    //                         edge.position = new Vector2(vertex.transform.position.x + (vB.transform.position.x - vertex.transform.position.x) / 2f, vertex.transform.position.y + (vB.transform.position.y - vertex.transform.position.y) / 2f);
    //                         Debug.Log("Edge - " + edge.name + " moved");
    //                     }
    //                 }
    //                 //GameObject vertexB = GameObject.Find(vertexBname);
    //             }
    //         }
    //     }
    // }

    public void showStartingPoint()
    {
        foreach (vertexScript vs in borderVertices)
        {
            if(vs.getStartingPoint() == true)
            {
                Debug.Log("Starting point - " + vs.getPos());
            }
        }
    }

    public void checkIfVertexIsConnected()
    {
        // foreach (vertexScript vs in vertices)
        // {
        //     Debug.Log("In Vertex ForEach");
        //     Debug.Log(vs.getName());
        //     //GameObject v = vs.gameObject;
        //     for(int i = 0; i < vs.edges.Count; i++)
        //     {
        //         Debug.Log("In edge for loop");
        //         if(edges[i].gameObject.GetComponent<MeshRenderer>() != null)
        //         {
        //             Debug.Log(vs.getPos() + " is connected");
        //             break;
        //         }
        //         else
        //         {
        //             Debug.Log("Graph is unconnected");
        //             isConnected = false;
        //         }
        //     }
        // }

        foreach(Transform child in transform)//For each vertex in graph
        {
            bool vertexConnected = false;
            foreach (Transform c in child)//For each edge in vertex
            {
                if(c.gameObject.GetComponent<MeshRenderer>().enabled == true)//If edge is showing
                {
                    //Debug.Log(c.GetComponent<edgeScript>().getA() + " " + c.GetComponent<edgeScript>().getB());
                    vertexConnected = true;
                }
            }
            if(!vertexConnected)
            {
                Debug.Log("Vertex - " + child.GetComponent<vertexScript>().getName() + " is not connected");
                isConnected = false;
            }
        }

    }

}

[System.Serializable]
public class baseGraphData
{
    public int size;//graph will be size*size (i.e - 5*5)
    public List<vertexData> vertices = new List<vertexData>();

    public void addVertex(vertexData v)
    {
        vertices.Add(v);
    }

    public void clear()
    {
        vertices.Clear();
    }
}

[System.Serializable]
public class vertexData
{
    public Vector2 position;
    public bool[] connectedEdges;
    public bool entryPoint;
    public bool objective;

    public void setVertexData(Vector2 pos, bool[] cEdges, bool ePoint, bool obj)
    {
        position = pos;
        connectedEdges = cEdges;
        entryPoint = ePoint;
        objective = obj;
        //Debug.Log(pos + " , " + cEdges + " , " + ePoint + " , " + obj);
    }

    public string getVertexData()
    {
        return JsonUtility.ToJson(this);
    }
}
