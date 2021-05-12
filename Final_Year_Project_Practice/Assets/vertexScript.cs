using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vertexScript : MonoBehaviour
{
    public bool active;
    public bool startingPoint = false;
    public int row;
    public int col;
    public List<edgeScript> edges = new List<edgeScript>();
    public int vertexName;

    public bool objectivePoint = false;//Must implement setting objective point!!!
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

    public void setStartingPoint(bool s)
    {
        startingPoint = s;
    }

    public bool getStartingPoint()
    {
        return startingPoint;
    }

    public void setObjectivePoint(bool o)
    {
        objectivePoint = o;
    }

    public bool getObjectivePoint()
    {
        return objectivePoint;
    }

    public Vector2 getPos()
    {
        return new Vector2(row, col);
    }

    public bool[] getConnectedEdges()
    {
        bool[] edges = new bool[8];
        foreach (edgeScript e in this.edges)
        {
            if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol-1)
            {
                edges[0] = true;
            }

            if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol)
            {
                edges[1] = true;
            }

            if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol+1)
            {
                edges[2] = true;
            }

            if(e.vertexBRow == e.vertexARow && e.vertexBCol == e.vertexACol-1)
            {
                edges[3] = true;
            }

            if(e.vertexBRow == e.vertexARow && e.vertexBCol == e.vertexACol+1)
            {
                edges[4] = true;
            }

            if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol-1)
            {
                edges[5] = true;
            }

            if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol)
            {
                edges[6] = true;
            }

            if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol+1)
            {
                edges[7] = true;
            }
        }

        return edges;
    }

    public int isEdgeConnected(edgeScript e)
    {
        if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol-1)
        {
            return 0;
        }

        else if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol)
        {
            return 1;
        }

        else if(e.vertexBRow == e.vertexARow-1 && e.vertexBCol == e.vertexACol+1)
        {
            return 2;
        }

        else if(e.vertexBRow == e.vertexARow && e.vertexBCol == e.vertexACol-1)
        {
            return 3;
        }

        else if(e.vertexBRow == e.vertexARow && e.vertexBCol == e.vertexACol+1)
        {
            return 4;
        }

        else if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol-1)
        {
            return 5;
        }

        else if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol)
        {
            return 6;
        }

        else if(e.vertexBRow == e.vertexARow+1 && e.vertexBCol == e.vertexACol+1)
        {
            return 7;
        }

        else
        {
            return -1;
        }
    }

    public int[] getWeights()
    {
        int[] weights = {0,0,0,0,0,0,0,0};

        for(int i = 0 ; i < edges.Count; i++)
        {
            if(isEdgeConnected(edges[i]) != -1)
            {
                weights[isEdgeConnected(edges[i])] = edges[i].getWeight();
            }
        }

        //for(int i = 0; i < weights.Length; i++)
        //{
            //weights[i] = edges[i].getWeight();
            //if(isEdgeConnected(edges[i]))
        //}

        return weights;
    }
}
