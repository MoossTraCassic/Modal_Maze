using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NaveMeshBaker : MonoBehaviour
{
    public static NaveMeshBaker instance;
    public bool bake = false;

    [SerializeField]
    private List<NavMeshSurface> naveMeshSurfaces;

    void Awake()
    {
        instance = this;    
    }

    void Update()
    {
        if(bake)
        {
            for (int i = 0; i < naveMeshSurfaces.Count; i++)
            {
                naveMeshSurfaces[i].BuildNavMesh();
            }
            bake = false;
        }
    }

    public List<NavMeshSurface> GetNavMeshSurfaces()
    {
        return naveMeshSurfaces;
    }
}
