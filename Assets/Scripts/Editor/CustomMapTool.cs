using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static Codice.Client.Commands.WkTree.WorkspaceTreeNode;
using static Codice.Client.BaseCommands.Import.Commit;

public class CustomMapTool : EditorWindow
{
    private MapToolData mapToolData;
    private GameObject seleteGameObject;

    private Vector2 scrollPos = Vector2.zero;

    private Vector2 palletScrollPos = Vector2.zero;
    private bool isDraw = true;
    private bool isMouseDown = false;
    private bool isSelect = false;

    private static string tabName;

    private int tabIndex;

    [MenuItem("Tools/MapEditor")]
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

        if (seleteGameObject != null)
        {
            DestroyImmediate(seleteGameObject);
        }
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUIUtility.labelWidth = 200;
        // EditorGUI.indentLevel++;

        mapToolData = EditorGUILayout.ObjectField("MapToolData", mapToolData, typeof(MapToolData), true) as MapToolData;
        isDraw = EditorGUILayout.Toggle("Drawable", isDraw);

        if(!isDraw)
        {
            isSelect = false;

            if (seleteGameObject != null)
            {
                DestroyImmediate(seleteGameObject);
                seleteGameObject = null;
            }
        }

        if (mapToolData != null)
        {
            tabIndex = GUILayout.Toolbar(tabIndex, mapToolData.TabNameList.ToArray());
            using (var scope = new GUILayout.VerticalScope(GUI.skin.window, GUILayout.Height(400)))
            {
                palletScrollPos = EditorGUILayout.BeginScrollView(palletScrollPos, false, true);
                DrawTilePaletteCell();
                EditorGUILayout.EndScrollView();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private Vector2 slotSize = new Vector2(100f, 100f);

    void DrawTilePaletteCell()
    {
        int yPos = -1;
        int xPos = 0;

        var area = GUILayoutUtility.GetRect(slotSize.x, slotSize.y, GUI.skin.window, GUILayout.MaxWidth(slotSize.x), GUILayout.MaxHeight(slotSize.y));

        var textureList = mapToolData.GetPreview(mapToolData.TabNameList[tabIndex]);
        int indexCount = textureList.Count;

        /*
        //EditorGUILayout.BeginHorizontal();

        //for (int i = 0; i < indexCount; ++i)
        //{

        //    if (GUILayout.RepeatButton(textureList[i]))
        //    {
        //    }

        //    if (i % 5 == 0)
        //    {
        //        EditorGUILayout.EndHorizontal();
        //        // EditorGUIUtility.fieldWidth = backup1;
        //        // EditorGUIUtility.labelWidth = backup2;

        //        EditorGUILayout.BeginHorizontal();
        //    }

        //}

        //EditorGUILayout.EndHorizontal();
        */

        for (int i = 0; i < textureList.Count; ++i)
        {
            if (i % 7 == 0)
            {
                yPos++;
                xPos = 0;
                EditorGUILayout.Space();
            }

            bool selected = GUI.Button(new Rect(area) { x = slotSize.x * xPos++, y = slotSize.y * yPos }, textureList[i]);

            if (selected)
            {
                isSelect = selected;

                if (seleteGameObject != null)
                {
                    DestroyImmediate(seleteGameObject);
                }

                seleteGameObject = Instantiate(mapToolData.tabGameObjectTable[mapToolData.TabNameList[tabIndex]][i]);
            }
                // SetDrawTile(textureList[i].name);
        }
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
        sceneView.Repaint();
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue))
        {
            if (seleteGameObject != null)
            {
                seleteGameObject.transform.position = hitInfo.point;
            }
        }

        if (e.button == 0)
        {
            // var mouseWorldPosition = GetWorldPosition(sceneView);
           
          

           

            switch (e.type)
            {
                case EventType.MouseDown:
                    isMouseDown = true;
                    GUIUtility.hotControl = id;
                    e.Use();
                    break;
                case EventType.MouseUp:

                    if (seleteGameObject != null)
                    {
                        Instantiate(seleteGameObject, hitInfo.point, Quaternion.identity);
                        //var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        //if (Physics.Raycast(ray, out var hitInfo, float.MaxValue))
                        //{
                        //    Instantiate(seleteGameObject, hitInfo.point, Quaternion.identity);
                        //}

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
