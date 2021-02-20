using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionFindAlgorithm : MonoBehaviour
{
    public int V,E;

    public Edge[] edges;

    public class Edge
    {
        public int s,d;
    }

    public UnionFindAlgorithm(int v, int e)
    {
        V = v;
        E = e;
        edges = new Edge[E];

        for(int i = 0; i < e; ++i)
        {
            edges[i] = new Edge();
        }
    }

    int find(int[] parent, int i)
    {
        if (parent[i] == -1)
            return i;
        return find(parent, parent[i]);
    }   

    void union(int []parent, int x, int y)
    {
        parent[x]=y;
    }

    int isCycle(UnionFindAlgorithm graph)
    {
        int[] parent = new int[graph.V];

        for(int i = 0; i < graph.V; ++i)
        {
            parent[i] =- 1;
        }

        for(int i = 0; i < graph.E; ++i)
        {
            int x = graph.find(parent, graph.edges[i].s);
            int y = graph.find(parent, graph.edges[i].d);

            if(x==y)
            {
                return 1;
            }

            graph.union(parent,x,y);
        }
        
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        UnionFindAlgorithm graph = new UnionFindAlgorithm(V,E);

        // add edge 0-1
        graph.edges[0].s = 0;
        graph.edges[0].d = 1;

        // add edge 1-2
        graph.edges[1].s = 1;
        graph.edges[1].d = 2;

        // add edge 0-2
        //graph.edges[2].s = 0;
        //graph.edges[2].d = 2;

        if(graph.isCycle(graph) == 1)
        {
            Debug.Log("Graph contains cycle");
        }
        else
        {
            Debug.Log("Graph doesn't contain cycle");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
