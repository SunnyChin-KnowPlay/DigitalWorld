using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using System.IO;

namespace DigitalWorld.AI.Editor
{
    public class NavMeshExport : EditorWindow
    {
        [MenuItem("AI/NavMesh/Export")]
        private static void Export()
        {
            NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
        }
    }

}

