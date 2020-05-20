using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ModalFunctions.Maze
{
    public class Maze : MonoBehaviour
    {
        [Tooltip("Size of the maze")]
        public IntVector2 size;    

        [Tooltip("Representation of a cell of the Maze")]
        public List<Cell> cellPrefabs;

        public float mazeScaleXZ = 3f;
        public float mazeScaleY = 6f;

        public MazePassage passagePrefab;
        public MazeWall wallPrefab;

        private Cell[,] cells;

        public Cell GetCell(IntVector2 coordinates)
        {
            return cells[coordinates.x, coordinates.z];
        }
        public void Generate()
        {
            cells = new Cell[size.x, size.z];
            List<Cell> activeCells = new List<Cell>();
            DoFirstGenerationStep(activeCells);


            while (activeCells.Count > 0)
            {

                DoNextGenerationStep(activeCells);
            }
        }

        private void DoFirstGenerationStep(List<Cell> activeCells)
        {
            activeCells.Add(CreateCell(RandomCoordinates));
        }

        private void DoNextGenerationStep(List<Cell> activeCells)
        {
            int currentIndex = activeCells.Count - 1;
            Cell currentCell = activeCells[currentIndex];
            if (currentCell.IsFullyInitialized)
            {
                activeCells.RemoveAt(currentIndex);
                return;
            }
            MazeDirection direction = currentCell.RandomUninitializedDirection;
            IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
            if (ContainsCoordinates(coordinates))// && GetCell(coordinates) == null)
            {
                Cell neighbor = GetCell(coordinates);
                if (neighbor == null)
                {
                    neighbor = CreateCell(coordinates);
                    CreatePassage(currentCell, neighbor, direction);
                    activeCells.Add(neighbor);
                }
                else
                {
                    if (neighbor.GetEdge(direction.GetOpposite()) == null)
                        CreateWall(currentCell, neighbor, direction);
                    else
                        CreatePassage(currentCell, neighbor, direction);
                }
            }
            else
            {
                CreateWall(currentCell, null, direction);
            }
        }

        private Cell CreateCell(IntVector2 coordinates)
        {
            Cell cellPrefab = cellPrefabs[Random.Range(0,cellPrefabs.Count)];
            Cell newCell = Instantiate<Cell>(cellPrefab);
            cells[coordinates.x, coordinates.z] = newCell;
            newCell.coordinates = coordinates;
            newCell.name = "Cell " + coordinates.x + ", " + coordinates.z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition =
                new Vector3(coordinates.x * mazeScaleXZ , 0f, coordinates.z * mazeScaleXZ);
            newCell.transform.localScale *= mazeScaleXZ;

            return newCell;
        }

        private void CreatePassage(Cell cell, Cell otherCell, MazeDirection direction)
        {
            MazePassage passage = Instantiate<MazePassage>(passagePrefab);
            passage.transform.localScale *= mazeScaleXZ;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate<MazePassage>(passagePrefab);
            passage.transform.localScale *= mazeScaleXZ;
            passage.Initialize(otherCell, cell, direction.GetOpposite());
        }

        private void CreateWall(Cell cell, Cell otherCell, MazeDirection direction)
        {
            MazeWall wall = Instantiate<MazeWall>(wallPrefab);
            wall.transform.localScale = new Vector3( wall.transform.localScale.x * mazeScaleXZ, wall.transform.localScale.y * mazeScaleY,
                                                        wall.transform.localScale.z * mazeScaleXZ);// mazeScaleXZ;
            wall.Initialize(cell, otherCell, direction);
        }

        public IntVector2 RandomCoordinates
        {
            get
            {
                return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
            }
        }

        public bool ContainsCoordinates(IntVector2 coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
        }
    }
}