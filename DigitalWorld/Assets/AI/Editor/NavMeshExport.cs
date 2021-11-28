using UnityEditor;
using UnityEngine.AI;

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

