using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModalFunctions.Maze
{
    public class MazeGenerator : MonoBehaviour
    {
        public Maze mazePrefab;

        private Maze mazeInstance;

        // Start is called before the first frame update
        private void Start()
        {
            Generate();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Backspace))  // Rewrite using a GUi Button in Menu
            {
                RestartMaze();
                // NaveMeshBaker.instance.bake = true;
            }
        }

        private void Generate()
        {
            mazeInstance = Instantiate<Maze>(mazePrefab);
            mazeInstance.Generate();
        }

        private void RestartMaze()
        {
            Destroy(mazeInstance.gameObject);
            Generate();
        }
    }
}