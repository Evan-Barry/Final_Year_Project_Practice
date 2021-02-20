using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertexScript : MonoBehaviour
{
    public bool active;
    public int row;
    public int col;
    public List<edgeScript> edges = new List<edgeScript>();
    public int vertexName;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive(bool a)
    {
        active = a;
    }

    public bool isActive()
    {
        return active;
    }
    public void setVertexNum(int i, int j)
    {
        row = i;
        col = j;
    }

    public string getvertexNum()
    {
        return row.ToString() + "," + col.ToString();
    }

    public int getRow()
    {
        return row;
    }

    public int getCol()
    {
        return col;
    }

    public void addEdge(edgeScript e)
    {
        edges.Add(e);
    }

    public List<edgeScript> GetEdges()
    {
        return edges;
    }

    public void setName(int n)
    {
        vertexName = n;
    }

    public int getName()
    {
        return vertexName;
    }

    public void printStatus()
    {
        if(isActive())
        {
            Debug.Log("Vertex " + getvertexNum() + " is active");
        }
        else
        {
            Debug.Log("Vertex " + getvertexNum() + " is not active");
        }
    }
}
