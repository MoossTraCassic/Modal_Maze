﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Maze
{
    public class Maze : MonoBehaviour
    {
        [Tooltip("Size of the maze")]
        public IntVector2 size;      // Rewrite later to manage the difficulty of a level

        [Tooltip("Representation of a cell of the Maze")]
        public Cell cellPrefab;

        public float mazeScale = 3f;

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
            //IntVector2 coordinates = RandomCoordinates;
            while (activeCells.Count > 0)
            {
                /*
                CreateCell(coordinates);
                coordinates += MazeDirections.RandomValue.ToIntVector2();
                */
                DoNextGenerationStep(activeCells);
            }
            /*
            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    CreateCell(new IntVector2(x, z));
                }
            }
            */
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
            MazeDirection direction = currentCell.RandomUninitializedDirection; ;
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
                    CreateWall(currentCell, neighbor, direction);
                    //activeCells.RemoveAt(currentIndex);
                    // no longer remove the Cell
                }
            }
            else
            {
                CreateWall(currentCell, null, direction);
                //activeCells.RemoveAt(currentIndex);
                // no longer remove the Cell 
            }
        }

        private Cell CreateCell(IntVector2 coordinates)
        {
            Cell newCell = Instantiate<Cell>(cellPrefab);
            cells[coordinates.x, coordinates.z] = newCell;
            newCell.coordinates = coordinates;
            newCell.name = "Cell " + coordinates.x + ", " + coordinates.z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition =
                new Vector3(coordinates.x * mazeScale - size.x * mazeScale * 0.5f, 0f, coordinates.z * mazeScale - size.z * mazeScale * 0.5f);
            newCell.transform.localScale *= mazeScale;

            return newCell;
        }

        private void CreatePassage(Cell cell, Cell otherCell, MazeDirection direction)
        {
            MazePassage passage = Instantiate<MazePassage>(passagePrefab);
            passage.transform.localScale *= mazeScale;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate<MazePassage>(passagePrefab);
            passage.transform.localScale *= mazeScale;
            passage.Initialize(otherCell, cell, direction.GetOpposite());
        }

        private void CreateWall(Cell cell, Cell otherCell, MazeDirection direction)
        {
            MazeWall wall = Instantiate<MazeWall>(wallPrefab);
            wall.transform.localScale *= mazeScale;
            wall.Initialize(cell, otherCell, direction);
            if (otherCell != null)
            {
                wall = Instantiate<MazeWall>(wallPrefab);
                wall.transform.localScale *= mazeScale;
                wall.Initialize(otherCell, cell, direction.GetOpposite());
            }
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