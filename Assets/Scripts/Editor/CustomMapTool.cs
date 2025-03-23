using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomMapTool : EditorWindow
{
    private MapToolData mapToolData;
    private GameObject seleteGameObject;

    private Vector2 scrollPos = Vector2.zero;
    private Vector2 palletScrollPos = Vector2.zero;

    private bool isDraw = true;
    private bool isMouseDown = false;
    private bool isSelect = false;
    private bool isDestory = false;

    private bool isRotation = false;
    private bool isAutoRotation = false;

    private float currentRotation = 0f;
    private float rotationSpeed = 10f;
    private Quaternion rotationRotation = Quaternion.identity;


    private static string tabName;

    private int tabIndex;
    private int seleteIndex;

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
        OnEndSelete();
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUIUtility.labelWidth = 200;
        // EditorGUI.indentLevel++;

        mapToolData = EditorGUILayout.ObjectField("MapToolData", mapToolData, typeof(MapToolData), true) as MapToolData;
        EditorGUILayout.Space(10);

        SetToggle();

        if (!isDraw)
        {
            OnEndSelete();
        }

        if (mapToolData != null)
        {
            int currentIndex = GUILayout.Toolbar(tabIndex, mapToolData.TabNameList.ToArray());

            if(currentIndex != tabIndex)
            {
                tabIndex = currentIndex; 
                OnEndSelete();
            }
            palletScrollPos = EditorGUILayout.BeginScrollView(palletScrollPos, false, true);
            DrawTilePaletteCell();
            EditorGUILayout.EndScrollView();
            //using (var scope = new GUILayout.VerticalScope(GUI.skin.window, GUILayout.Height(400)))
            //{
               
            //}
        }

        EditorGUILayout.EndScrollView();
    }

    private void SetToggle()
    {

        if (EditorGUILayout.Toggle("그리기 시작", isDraw))
        {
            if (!isDraw)
            {
                isDestory = EditorGUILayout.Toggle("배치 오브젝트 삭제", false);
            }
            isDraw = true;
        }
        else
        {
            isDraw = false;
            currentRotation = 0f;
        }

        if (isDraw)
        {
            EditorGUILayout.Space(5);
            ++EditorGUI.indentLevel;

            isAutoRotation = EditorGUILayout.Toggle("자동 회전", isAutoRotation);

            if (!isAutoRotation)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.LabelField("q 버튼을 누르면 좌측 회전/ e 버튼을 누르면 우측 회전");
                rotationSpeed = EditorGUILayout.Slider("회전 속도", rotationSpeed, 0.1f, 360f);
            }
            --EditorGUI.indentLevel;
        }

        EditorGUILayout.Space(5);
        if (EditorGUILayout.Toggle("배치 오브젝트 삭제", isDestory))
        {
            if (!isDestory)
            {
                isDraw = EditorGUILayout.Toggle("그리기 시작", false);
            }
            isDestory = true;
        }
        else
        { 
            isDestory = false;
        }

        EditorGUILayout.Space(10);
    }
    private void OnEndSelete()
    {
        isSelect = false;

        if (seleteGameObject != null)
        {
            DestroyImmediate(seleteGameObject);
            seleteGameObject = null;
        }
    }

    private Vector2 slotSize = new Vector2(100f, 100f);

    void DrawTilePaletteCell()
    {
        var textureList = mapToolData.GetPreview(mapToolData.TabNameList[tabIndex]);
        int indexCount = textureList.Count;

        if (indexCount == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        int lineFeedIndex = 0;

        for (int i = 0; i < indexCount; ++i)
        {
            ++lineFeedIndex;

            if (GUILayout.RepeatButton(textureList[i], GUILayout.MaxWidth(slotSize.x), GUILayout.MaxHeight(slotSize.y)))
            {
                OnEndSelete();

                isSelect = true;
                seleteIndex = 0;
                seleteGameObject = Instantiate(mapToolData.tabGameObjectTable[mapToolData.TabNameList[tabIndex]][i]);
                var colliders = seleteGameObject.GetComponentsInChildren<Collider>();

                if (SceneView.sceneViews.Count > 0)
                {
                    SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                    sceneView.Focus();
                }

                foreach (var collider in colliders)
                {
                    collider.enabled = false;
                }
            }

            if (lineFeedIndex % 7 == 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            }

        }

        EditorGUILayout.EndHorizontal();



        //for (int i = 0; i < textureList.Count; ++i)
        //{
        //    if (i % 7 == 0)
        //    {
        //        yPos++;
        //        xPos = 0;
        //        EditorGUILayout.Space();
        //    }


        //    bool selected = GUI.Button(new Rect(area) { x = slotSize.x * xPos++, y = slotSize.y * yPos }, textureList[i]);

        //    if (selected)
        //    {
        //        OnEndSelete();

        //        isSelect = true;
        //        seleteIndex = i;
        //        seleteGameObject = Instantiate(mapToolData.tabGameObjectTable[mapToolData.TabNameList[tabIndex]][i]);
        //        var colliders = seleteGameObject.GetComponentsInChildren<Collider>();

        //        if (SceneView.sceneViews.Count > 0)
        //        {
        //            SceneView sceneView = (SceneView)SceneView.sceneViews[0];
        //            sceneView.Focus();
        //        }

        //        foreach (var collider in colliders)
        //        {
        //            collider.enabled = false;   
        //        }
        //    }
        //        // SetDrawTile(textureList[i].name);
        //}
    }

    private void UpdateScene(SceneView sceneView)
    {
        // DisplayLines();

        if (isDraw || isDestory)
        {
            Event e = Event.current;
            ProcecssKeyInput(sceneView, e);
            ProcessMouseInput(sceneView, e);
            sceneView.Repaint();
        }
    }

    private void ProcecssKeyInput(SceneView sceneView, Event e)
    {
        if(e.keyCode == KeyCode.None && !isDraw || seleteGameObject == null)
        {
            return;
        }

        switch (e.keyCode)
        {
            case KeyCode.E:
                currentRotation += rotationSpeed;
                seleteGameObject.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
                break;
            case KeyCode.Q:
                currentRotation -= rotationSpeed;
                seleteGameObject.transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
                break;
            default:
                break;
        }
    }

    private void ProcessMouseInput(SceneView sceneView, Event e)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        sceneView.Repaint();
        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, float.MaxValue))
        {
            if (isSelect && seleteGameObject != null)
            {
                seleteGameObject.transform.position = hitInfo.point;
            }

            if(isDestory)
            {
                Handles.color = Color.red;
                Bounds bounds = GetObjectBounds(hitInfo.collider.transform);
                Handles.DrawWireCube(bounds.center, bounds.size);
            }
        }

        if (e.button == 0)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    isMouseDown = true;
                    GUIUtility.hotControl = id;
                    e.Use();
                    break;
                case EventType.MouseUp:

                    if (isSelect && seleteGameObject != null)
                    {
                        var createGameObject = (GameObject)PrefabUtility.InstantiatePrefab(mapToolData.tabGameObjectTable[mapToolData.TabNameList[tabIndex]][seleteIndex]);
                        createGameObject.transform.SetPositionAndRotation(seleteGameObject.transform.position, seleteGameObject.transform.rotation);
                        Undo.RegisterCreatedObjectUndo(createGameObject, "Undo PrefabBrush");
                    }

                    if (isDestory)
                    {
                        if(hitInfo.collider != null)
                        {
                            DestroyImmediate(hitInfo.collider.gameObject);
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
    private Bounds GetObjectBounds(Transform target)
    {
        Renderer renderer = target.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }

        return new Bounds(target.position, Vector3.one);
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

        return worldPosition;
    }

}
