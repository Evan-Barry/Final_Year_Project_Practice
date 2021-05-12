using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseGraphScript : MonoBehaviour
{
    public int size;
    public GameObject vertexGO;
    public GameObject edgeGO;
    public vertexScript[,] vertices;
    public edgeScript[] edges;
    //public int numOfVertices;
    public int numOfEdges;
    // Start is called before the first frame update
    void Start()
    {
        //numOfVertices = size*size;
        vertices = new vertexScript[size,size];
        initVertices();
        //printVerticesStatus();

        numOfEdges = (size-1)*(size+size);
        edges = new edgeScript[numOfEdges];
        initEdges();
        //connectEdgesAndVertices();
        printEdgeStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void initVertices()
    {
        vertexScript v = vertexGO.GetComponent<vertexScript>();

        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                vertices[i,j] = v;
                vertices[i,j].setActive(true);
                //setVertexRule(i,j);
                //vertices[i,j].setVertexNum(i,j);
                //Debug.Log(vertices[i,j].printStatus());
            }
            
        }
    }

    void printVerticesStatus()
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                //Debug.Log(vertices[i,j].printStatus());
            }
        }
    }

    void setVerticesStatus(bool status)
    {
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                vertices[i,j].setActive(status);
                //Debug.Log(vertices[i,j].printStatus());
            }
        }
    }

    // void setVertexRule(int i, int j)
    // {
    //     if(i == 0)
    //     {
    //         vertices[i,j].setEdgeRule('N',false);
    //     }

    //     if(i == size-1)
    //     {
    //         vertices[i,j].setEdgeRule('S',false);
    //     }

    //     if(j == 0)
    //     {
    //         vertices[i,j].setEdgeRule('W',false);
    //     }

    //     if(j == size-1)
    //     {
    //         vertices[i,j].setEdgeRule('E',false);
    //     }
    // }

    void initEdges()
    {
        edgeScript e = edgeGO.GetComponent<edgeScript>();

        for(int i = 0; i < numOfEdges; i++)
        {
            edges[i] = e;
            //edges[i].setActive(false);
        }
    }

    // void connectEdgesAndVertices()
    // {
    //     for(int i = 0; i < size; i++)
    //     {
    //         for(int j = 0; j < size; j++)
    //         {
    //             if(vertices[i,j].north == true)
    //             {
    //                 if(!edgeExists(vertices[i,j],vertices[i+1,j]))
    //                 {
    //                     foreach (edgeScript edge in edges)
    //                     {
    //                         if(!edge.isActive())
    //                         {
    //                             //edge.setVertices((i.ToString()+j.ToString()),((i+1).ToString()+j.ToString()));
    //                             edge.setActive(true);
    //                         }
    //                     }
    //                 }
    //             }

    //             if(vertices[i,j].south == true)
    //             {
    //                 if(!edgeExists(vertices[i,j],vertices[i-1,j]))
    //                 {
    //                     foreach (edgeScript edge in edges)
    //                     {
    //                         if(!edge.isActive())
    //                         {
    //                             //edge.setVertices((i.ToString()+j.ToString()),((i-1).ToString()+j.ToString()));
    //                             edge.setActive(true);
    //                         }
    //                     }
    //                 }
    //             }

    //             if(vertices[i,j].west == true)
    //             {
    //                 if(!edgeExists(vertices[i,j],vertices[i,j-1]))
    //                 {
    //                     foreach (edgeScript edge in edges)
    //                     {
    //                         if(!edge.isActive())
    //                         {
    //                             //edge.setVertices((i.ToString()+j.ToString()),(i.ToString()+(j-1).ToString()));
    //                             edge.setActive(true);
    //                         }
    //                     }
    //                 }
    //             }

    //             if(vertices[i,j].east == true)
    //             {
    //                 if(!edgeExists(vertices[i,j],vertices[i,j+1]))
    //                 {
    //                     foreach (edgeScript edge in edges)
    //                     {
    //                         if(!edge.isActive())
    //                         {
    //                             //edge.setVertices((i.ToString()+j.ToString()),(i.ToString()+(j+1).ToString()));
    //                             edge.setActive(true);
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }

    bool edgeExists(vertexScript a, vertexScript b)
    {
        foreach (edgeScript edge in edges)
        {
            //if(edge.vertexA.ToString()+edge.vertexB.ToString() == a.getvertexNum() || edge.vertexB.ToString()+edge.vertexA.ToString() == a.getvertexNum())
            {
                return true;
            }
        }

        return false;
    }

    void printEdgeStatus()
    {
        for(int i = 0; i < numOfEdges; i++)
        {
            edges[i].printStatus();
        }
    }
}
