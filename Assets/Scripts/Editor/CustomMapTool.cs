using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomMapTool : EditorWindow
{
    private GameObject testObject;
    private Vector2 scrollPos = Vector2.zero;
    private bool isDraw = true;
    private bool isMouseDown = false;

    [MenuItem("Tool/MapEditor")]
    private static void Open()
    {
        CustomMapTool window = (CustomMapTool)GetWindow(typeof(CustomMapTool), false, "TestMapTool", true);
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui -= UpdateScene;
        SceneView.duringSceneGui += UpdateScene;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= UpdateScene;

        //if (brushTransfom != null)
        //{
        //    DestroyImmediate(brushTransfom.gameObject);
        //}
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUIUtility.labelWidth = 200;
        // EditorGUI.indentLevel++;

        testObject = EditorGUILayout.ObjectField("GameObject", testObject, typeof(GameObject), true) as GameObject;
        isDraw = EditorGUILayout.Toggle("Drawable", isDraw);


        EditorGUILayout.EndScrollView();
    }

    private void UpdateScene(SceneView sceneView)
    {
        // DisplayLines();

        if (isDraw)
        {
            Event e = Event.current;
            ProcecssKeyInput(sceneView, e);
            ProcessMouseInput(sceneView, e);
            sceneView.Repaint();
        }
    }

    private void ProcecssKeyInput(SceneView sceneView, Event e)
    {
    }

    private void ProcessMouseInput(SceneView sceneView, Event e)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);

        if(e.button == 0)
        {
            // var mouseWorldPosition = GetWorldPosition(sceneView);
            sceneView.Repaint();

            switch (e.type)
            {
                case EventType.MouseDown:
                    isMouseDown = true;
                    GUIUtility.hotControl = id;
                    e.Use();
                    break;
                case EventType.MouseUp:

                    if(testObject != null)
                    {
                        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        if (Physics.Raycast(ray, out var hitInfo,float.MaxValue))
                        {
                            Instantiate(testObject, hitInfo.point, Quaternion.identity);                            
                        }

                    }


                    isMouseDown = false;
                    GUIUtility.hotControl = 0;
                    e.Use();
                    break;
                case EventType.MouseMove:
                    break;
                default:
                    break;
            }
        }

    }

        private Vector3 GetWorldPosition(SceneView sceneView)
    {
        Vector3 mousePosition = Event.current.mousePosition;

        float mult = 1;
#if UNITY_5_4_OR_NEWER
        mult = EditorGUIUtility.pixelsPerPoint;
#endif

        mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * mult;
        mousePosition.x *= mult;

        Vector3 worldPosition = sceneView.camera.ScreenToWorldPoint(mousePosition);
        // worldPosition.z = 0f;

        return worldPosition;
    }

}
