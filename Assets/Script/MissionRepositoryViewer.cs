using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class MissionRepositoryViewer : EditorWindow
{
    private string saveSlot = "start";
    private MissionRepository repository;
    private Vector2 scrollPosition;
    private Vector2 jsonScrollPosition;
    private bool showJson = false;
    private string jsonText = "";
    private string searchFilter = "";
    private MissionType? typeFilter = null;
    
    [MenuItem("Tools/Mission Repository Viewer")]
    public static void ShowWindow()
    {
        GetWindow<MissionRepositoryViewer>("Mission Repository");
    }
    
    [MenuItem("Window/Mission Tools/Repository Viewer")]
    public static void ShowWindowAlternate()
    {
        ShowWindow();
    }
    
    void OnEnable()
    {
        LoadRepository();
    }
    
    void LoadRepository()
    {
        repository = MissionSaveSystem.LoadRepository(saveSlot);

        string path = Application.persistentDataPath + "/Saves/" + saveSlot + "_missions.json";

        if(saveSlot == "start")
        {
            path = Application.dataPath + "/JSON/start_missions.json";
        }
        if (File.Exists(path))
        {
            jsonText = File.ReadAllText(path);
        }
        else
        {
            jsonText = "No repository file found";
        }
    }



    
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();


        // Button to open save location
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Open Save Location"))
        {
            string path = Application.persistentDataPath + "/Saves/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            EditorUtility.RevealInFinder(path);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        // Save slot selection
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save Slot:", GUILayout.Width(70));
        string[] saveFiles = Directory.Exists(Application.persistentDataPath + "/Saves/") 
            ? Directory.GetFiles(Application.persistentDataPath + "/Saves/", "*_missions.json")
            .Select(path => Path.GetFileNameWithoutExtension(path).Replace("_missions", ""))
            .ToArray()
            : new string[0];
            
        List<string> availableSaveSlots = new List<string> { "start" };
        availableSaveSlots.AddRange(saveFiles);
        
        int selectedIndex = availableSaveSlots.IndexOf(saveSlot);
        if (selectedIndex < 0) selectedIndex = 0;
        
        int newSelectedIndex = EditorGUILayout.Popup("", selectedIndex, availableSaveSlots.ToArray(), GUILayout.Width(150));


        string newSaveSlot = availableSaveSlots[newSelectedIndex];
        if (newSaveSlot != saveSlot)
        {
            saveSlot = newSaveSlot;
            LoadRepository();
        }
        if (GUILayout.Button("Reload", GUILayout.Width(60)))
        {
            LoadRepository();
        }
        EditorGUILayout.EndHorizontal();
        
        // Filtering options
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Filter:", GUILayout.Width(50));
        searchFilter = EditorGUILayout.TextField(searchFilter);
        
        MissionType? newTypeFilter = EditorGUILayout.EnumPopup("Type Filter:", typeFilter ?? (MissionType)(-1)) as MissionType?;
        if (newTypeFilter.HasValue && (int)newTypeFilter.Value == -1)
        {
            newTypeFilter = null;
        }
        typeFilter = newTypeFilter;
        
        if (GUILayout.Button("Clear Filters", GUILayout.Width(100)))
        {
            searchFilter = "";
            typeFilter = null;
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // Repository content
        EditorGUILayout.LabelField("Missions", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
        
        if (repository.missions.Count == 0)
        {
            EditorGUILayout.LabelField("No missions found in repository");
        }
        else
        {
            foreach (var missionData in repository.missions)
            {
                MissionBase mission = missionData.Deserialize();
                
                // Apply filters
                if (!string.IsNullOrEmpty(searchFilter))
                {
                    if (!mission.title.ToLower().Contains(searchFilter.ToLower()) &&
                        !mission.description.ToLower().Contains(searchFilter.ToLower()) &&
                        !mission.id.ToLower().Contains(searchFilter.ToLower()))
                    {
                        continue;
                    }
                }
                
                if (typeFilter.HasValue && mission.type != typeFilter.Value)
                {
                    continue;
                }
                
                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(mission.title, EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Type: {mission.type}", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField($"ID: {mission.id}");
                EditorGUILayout.LabelField($"Chapter: {mission.chapter}");
                EditorGUILayout.LabelField("Description:", EditorStyles.boldLabel);
                EditorGUILayout.LabelField(mission.description, EditorStyles.wordWrappedLabel);
                
                if (!string.IsNullOrEmpty(mission.nextMissionId))
                {
                    EditorGUILayout.LabelField($"Next Mission: {mission.nextMissionId}");
                }
                
                // Show type-specific details
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Mission Details:", EditorStyles.boldLabel);
                
                if (mission is AttackMission attackMission)
                {
                    EditorGUILayout.LabelField($"Target: {attackMission.targetName}");
                    EditorGUILayout.LabelField($"Target Count: {attackMission.targetCount}");
                }
                else if (mission is DeliveryMission deliveryMission)
                {
                    EditorGUILayout.LabelField($"From: {deliveryMission.departureLocation}");
                    EditorGUILayout.LabelField($"To: {deliveryMission.destinationLocation}");
                    EditorGUILayout.LabelField($"Item: {deliveryMission.itemToDeliver}");
                }
                else if (mission is MiningMission miningMission)
                {
                    EditorGUILayout.LabelField($"Item: {miningMission.itemToMine}");
                    EditorGUILayout.LabelField($"Amount: {miningMission.requiredAmount}");
                }
                else if (mission is TalkMission talkMission)
                {
                    EditorGUILayout.LabelField($"Character: {talkMission.characterName}");
                    EditorGUILayout.LabelField($"Required Phase: {talkMission.requiredPhase}");
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        // Active Missions
        if (repository.activeMissions.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Active Missions", EditorStyles.boldLabel);
            
            foreach (var missionData in repository.activeMissions)
            {
                MissionBase mission = missionData.Deserialize();
                EditorGUILayout.LabelField($"• {mission.title} ({mission.type})");
            }
        }
        
        // Completed Missions
        if (repository.completedMissions.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Completed Missions", EditorStyles.boldLabel);
            
            foreach (var missionData in repository.completedMissions)
            {
                MissionBase mission = missionData.Deserialize();
                EditorGUILayout.LabelField($"• {mission.title} ({mission.type})");
            }
        }
        
        // Raw JSON viewer
        EditorGUILayout.Space();
        showJson = EditorGUILayout.Foldout(showJson, "Raw JSON");
        
        if (showJson)
        {
            jsonScrollPosition = EditorGUILayout.BeginScrollView(jsonScrollPosition, GUILayout.Height(200));
            EditorGUILayout.TextArea(jsonText, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.EndVertical();
    }
}
#endif