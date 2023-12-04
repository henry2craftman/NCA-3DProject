using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class TestExecuteAlways : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right * 1f);
    }
#if UNITY_EDITOR
    [MenuItem("Execute/Hello")]
    public static void UpdateData()
    {
        Debug.Log("Yo");
        //transform.Rotate(Vector3.up * 10f);
        GameObject obj = new GameObject("GO");
        obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    void OnDrawGizmos()
    {
        // Your gizmo drawing thing goes here if required...


        // Ensure continuous Update calls.
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }

    }

    // IMGUI
    void OnGUI()
    {
        GUI.color = Color.yellow;
        GUIStyle headStyle = new GUIStyle();
        headStyle.fontSize = 30;

        GUI.Label(new Rect(10, 10, 1000, 200), "Hello World!", headStyle);

        GUI.Box(new Rect(10, 20, 100, 100), "A BOX");

        if (GUI.Button(new Rect(10 + 100, 20 + 100, 500, 300), "Rotate"))
        {
            transform.Rotate(Vector3.up * 100f);
        }
        
    }
#endif
}