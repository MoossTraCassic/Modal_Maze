using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Maze
{
    public class Cell : MonoBehaviour
    {
        public IntVector2 coordinates;

        private CellEdge[] edges = new CellEdge[MazeDirections.Count];

        private int initializedEdgeCount;

        public CellEdge GetEdge(MazeDirection direction)
        {
            return edges[(int)direction];
        }

        public bool IsFullyInitialized
        {
            get
            {
                return initializedEdgeCount == MazeDirections.Count;
            }
        }

        public void SetEdge(MazeDirection direction, CellEdge edge)
        {
            edges[(int)direction] = edge;
            initializedEdgeCount += 1;
        }

        public MazeDirection RandomUninitializedDirection
        {
            get
            {
                int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
                for (int i = 0; i < MazeDirections.Count; i++)
                {
                    if (edges[i] == null)
                    {
                        if (skips == 0)
                        {
                            return (MazeDirection)i;
                        }
                        skips -= 1;
                    }
                }
                //return (MazeDirection)0;
                throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
            }
            
        }

    }
    
}


