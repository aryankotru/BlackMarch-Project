using UnityEditor;
using UnityEngine;


public class ObstacleEditor : EditorWindow
{
    private ObstacleData obstacleData;

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditor>("Obstacle Editor");
    }

    private void OnGUI()
    {
        if (obstacleData == null)
        {
            GUILayout.Label("Select Obstacle Data", EditorStyles.boldLabel);
            obstacleData = (ObstacleData)EditorGUILayout.ObjectField("Obstacle Data", obstacleData, typeof(ObstacleData), false);
            return;
        }

        GUILayout.Label("Obstacle Grid Editor", EditorStyles.boldLabel);

        // Display 10x10 grid of buttons
        for (int x = 0; x < 10; x++)
        {
            GUILayout.BeginHorizontal();
            for (int y = 0; y < 10; y++)
            {
                // Toggle button for each grid cell
                bool isBlocked = obstacleData._gridData[x, y];
                bool newIsBlocked = GUILayout.Toggle(isBlocked, $"{x},{y}", GUILayout.Width(50));

                // Update data if changed
                if (newIsBlocked != isBlocked)
                {
                    Undo.RecordObject(obstacleData, "Toggle Obstacle");
                    obstacleData._gridData[x, y] = newIsBlocked;

                    // Make the SO editable and saveable basically -> if you want to save at runtime
                    EditorUtility.SetDirty(obstacleData);
                }
            }
            GUILayout.EndHorizontal();
        }

        // Add a save button (optional)
        if (GUILayout.Button("Save Obstacle Data"))
        {
            AssetDatabase.SaveAssets();
            //Debug.Log("Obstacle Data saved!");
        }
    }
}
