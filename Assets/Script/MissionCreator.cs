using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

public class MissionCreator : EditorWindow
{
    private MissionType selectedType = MissionType.Attack;
    private MissionBase currentMission;
    private string saveSlot = "start";
    private MissionRepository repository;
    private Vector2 scrollPosition;
    private bool showSavedMissions = false;
    private int selectedMissionIndex = -1;
    
    [MenuItem("Tools/Mission Creator")]
    public static void ShowWindow()
    {
        GetWindow<MissionCreator>("Mission Creator");
    }
    
    [MenuItem("Window/Mission Tools/Mission Creator")]
    public static void ShowWindowAlternate()
    {
        ShowWindow();
    }
    
    void OnEnable()
    {
        // Load repository when window opens
        repository = MissionSaveSystem.LoadRepository(saveSlot);
    }
    
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        // Save slot selection
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Save Slot:", GUILayout.Width(70));
        saveSlot = EditorGUILayout.TextField(saveSlot);
        if (GUILayout.Button("Load", GUILayout.Width(60)))
        {
            repository = MissionSaveSystem.LoadRepository(saveSlot);
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // Mission Creator Section
        EditorGUILayout.LabelField("Create/Edit Mission", EditorStyles.boldLabel);
        
        selectedType = (MissionType)EditorGUILayout.EnumPopup("Mission Type:", selectedType);
        
        // Create appropriate mission object based on selected type
        if (currentMission == null || currentMission.type != selectedType)
        {
            switch (selectedType)
            {
                case MissionType.Attack:
                    currentMission = new AttackMission();
                    break;
                case MissionType.Delivery:
                    currentMission = new DeliveryMission();
                    break;
                case MissionType.Mining:
                    currentMission = new MiningMission();
                    break;
                case MissionType.Talk:
                    currentMission = new TalkMission();
                    break;
            }
            currentMission.Initialize();
        }
        
        // Draw mission-specific fields
        EditorGUILayout.LabelField("Mission Details", EditorStyles.boldLabel);
        
        // Title et ID - version corrigée
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 40; // Réduire la largeur du label
        currentMission.title = EditorGUILayout.TextField("Title:", currentMission.title);
        EditorGUIUtility.labelWidth = 20; // Réduire encore plus pour ID
        currentMission.id = EditorGUILayout.TextField("ID:", currentMission.id);
        EditorGUIUtility.labelWidth = 0; // Réinitialiser
        EditorGUILayout.EndHorizontal();

        // Generate ID button
        if (string.IsNullOrEmpty(currentMission.id) && !string.IsNullOrEmpty(currentMission.title))
        {
            if (GUILayout.Button("Generate ID from Title", GUILayout.Width(150)))
            {
                currentMission.id = currentMission.title.ToLower().Replace(" ", "_");
            }
        }

        // Chapter et Description restent les mêmes
        currentMission.chapter = EditorGUILayout.TextField("Chapter:", currentMission.chapter);
        currentMission.description = EditorGUILayout.TextField("Description:", currentMission.description);

        // Type-specific fields
        if (currentMission is AttackMission attackMission)
        {
            // Version corrigée
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 80; // Adapter pour "Target Name:"
            attackMission.targetName = EditorGUILayout.TextField("Target Name:", attackMission.targetName);
            EditorGUIUtility.labelWidth = 50; // Adapter pour "Count:"
            attackMission.targetCount = EditorGUILayout.IntField("Count:", attackMission.targetCount, GUILayout.Width(100));
            EditorGUIUtility.labelWidth = 0; // Réinitialiser
            EditorGUILayout.EndHorizontal();
        }
        else if (currentMission is DeliveryMission deliveryMission)
        {
            deliveryMission.departureLocation = EditorGUILayout.TextField("Departure:", deliveryMission.departureLocation);
            deliveryMission.destinationLocation = EditorGUILayout.TextField("Destination:", deliveryMission.destinationLocation);
            deliveryMission.itemToDeliver = EditorGUILayout.TextField("Item:", deliveryMission.itemToDeliver);
        }
        else if (currentMission is MiningMission miningMission)
        {
            miningMission.itemToMine = EditorGUILayout.TextField("Item to Mine:", miningMission.itemToMine);
            miningMission.requiredAmount = EditorGUILayout.IntField("Required Amount:", miningMission.requiredAmount);
        }
        else if (currentMission is TalkMission talkMission)
        {
            talkMission.characterName = EditorGUILayout.TextField("Character Name:", talkMission.characterName);
            talkMission.requiredPhase = EditorGUILayout.IntField("Required Phase:", talkMission.requiredPhase);
        }
        
        // Next mission field
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mission Chain", EditorStyles.boldLabel);
        currentMission.nextMissionId = EditorGUILayout.TextField("Next Mission ID:", currentMission.nextMissionId);
        
        // Mission Events section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mission Events", EditorStyles.boldLabel);

        currentMission.onStartEvent = EditorGUILayout.TextField("On Start Event:", currentMission.onStartEvent);
        currentMission.onProgressEvent = EditorGUILayout.TextField("On Progress Event:", currentMission.onProgressEvent);
        currentMission.onCompleteEvent = EditorGUILayout.TextField("On Complete Event:", currentMission.onCompleteEvent);

        // Help box to explain events
        EditorGUILayout.HelpBox("Enter event IDs that will be triggered at different mission stages. These IDs must be registered with MissionManager at runtime.", MessageType.Info);

        // Mission Rewards section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mission Rewards", EditorStyles.boldLabel);

        // Create reward if it doesn't exist
        if (currentMission.reward == null)
        {
            currentMission.reward = new RecompenseClass();
        }

        // Basic rewards - version corrigée
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 30; // Réduire la largeur du label
        currentMission.reward.xp = EditorGUILayout.FloatField("XP:", currentMission.reward.xp);
        EditorGUIUtility.labelWidth = 50; // Réduire la largeur du label
        currentMission.reward.Argent = EditorGUILayout.IntField("Argent:", currentMission.reward.Argent); 
        EditorGUIUtility.labelWidth = 40; // Réduire la largeur du label
        currentMission.reward.index = EditorGUILayout.IntField("Index:", currentMission.reward.index);
        EditorGUIUtility.labelWidth = 0; // Réinitialiser
        EditorGUILayout.EndHorizontal();

        // Pieces section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pieces Rewards", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        // Initialize pieces list if needed
        if (currentMission.reward.pieces == null)
        {
            currentMission.reward.pieces = new List<string>();
        }

        // Add piece button
        if (GUILayout.Button("+ Add Piece", GUILayout.Width(120)))
        {
            currentMission.reward.pieces.Add("");
        }

        // Display pieces
        if (currentMission.reward.pieces != null && currentMission.reward.pieces.Count > 0)
        {
            EditorGUILayout.LabelField("Pieces:", EditorStyles.boldLabel);
            
            for (int i = 0; i < currentMission.reward.pieces.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                currentMission.reward.pieces[i] = EditorGUILayout.TextField($"Piece {i+1}:", currentMission.reward.pieces[i]);
                
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    currentMission.reward.pieces.RemoveAt(i);
                    i--; // Adjust index after removal
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        // Deck section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Deck Item Rewards", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        // Initialize deck list if needed
        if (currentMission.reward.Deck == null)
        {
            currentMission.reward.Deck = new List<string>();
        }

        // Add deck item button
        if (GUILayout.Button("+ Add Deck Item", GUILayout.Width(120)))
        {
            currentMission.reward.Deck.Add("");
        }

        // Display deck items
        if (currentMission.reward.Deck != null && currentMission.reward.Deck.Count > 0)
        {
            EditorGUILayout.LabelField("Deck Items:", EditorStyles.boldLabel);
            
            for (int i = 0; i < currentMission.reward.Deck.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                currentMission.reward.Deck[i] = EditorGUILayout.TextField($"Item {i+1}:", currentMission.reward.Deck[i]);
                
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    currentMission.reward.Deck.RemoveAt(i);
                    i--; // Adjust index after removal
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();

        // Pieces list (strings)
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pieces", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");

        // Initialize pieces list if needed
        if (currentMission.reward.pieces == null)
        {
            currentMission.reward.pieces = new List<string>();
        }

        // Add piece button
        // if (GUILayout.Button("+ Add Piece"))
        // {
        //     currentMission.reward.pieces.Add("");
        // }

        // // Display pieces
        // for (int i = 0; i < currentMission.reward.pieces.Count; i++)
        // {
        //     EditorGUILayout.BeginHorizontal();
        //     currentMission.reward.pieces[i] = EditorGUILayout.TextField($"Piece {i+1}:", currentMission.reward.pieces[i]);
            
        //     if (GUILayout.Button("X", GUILayout.Width(25)))
        //     {
        //         currentMission.reward.pieces.RemoveAt(i);
        //         i--; // Adjust index after removal
        //     }
        //     EditorGUILayout.EndHorizontal();
        // }
        EditorGUILayout.EndVertical();

        // // Deck list (strings)
        // EditorGUILayout.Space();
        // EditorGUILayout.LabelField("Deck Items", EditorStyles.boldLabel);
        // EditorGUILayout.BeginVertical("box");

        // // Initialize deck list if needed
        // if (currentMission.reward.Deck == null)
        // {
        //     currentMission.reward.Deck = new List<string>();
        // }

        // // Add deck item button
        // if (GUILayout.Button("+ Add Deck Item"))
        // {
        //     currentMission.reward.Deck.Add("");
        // }

        // // Display deck items
        // for (int i = 0; i < currentMission.reward.Deck.Count; i++)
        // {
        //     EditorGUILayout.BeginHorizontal();
        //     currentMission.reward.Deck[i] = EditorGUILayout.TextField($"Item {i+1}:", currentMission.reward.Deck[i]);
            
        //     if (GUILayout.Button("X", GUILayout.Width(25)))
        //     {
        //         currentMission.reward.Deck.RemoveAt(i);
        //         i--; // Adjust index after removal
        //     }
        //     EditorGUILayout.EndHorizontal();
        // }
        // EditorGUILayout.EndVertical();

        // Pieces section - Needs special handling because it's a list of PieceClass objects
        // EditorGUILayout.Space();
        // EditorGUILayout.LabelField("Piece Rewards", EditorStyles.boldLabel);
        // if (currentMission.reward.pieces == null)
        // {
        //     currentMission.reward.pieces = new List<PieceClass>();
        // }

        // // Add piece button
        // EditorGUILayout.BeginHorizontal();
        // if (GUILayout.Button("+ Add Piece", GUILayout.Width(120)))
        // {
        //     currentMission.reward.pieces.Add(new PieceClass());
        // }
        // EditorGUILayout.EndHorizontal();

        // // Display pieces
        // if (currentMission.reward.pieces != null && currentMission.reward.pieces.Count > 0)
        // {
        //     for (int i = 0; i < currentMission.reward.pieces.Count; i++)
        //     {
        //         EditorGUILayout.BeginVertical("box");
                
        //         EditorGUILayout.LabelField($"Piece {i+1}", EditorStyles.boldLabel);
                
        //         // ⚠️ Note: Replace these fields with the actual properties of your PieceClass
        //         // This is an example assuming PieceClass has properties like name, level, etc.
        //         // You'll need to modify this section based on what your PieceClass actually contains
                
        //         // Example fields - adapt to your actual PieceClass properties
        //         if (currentMission.reward.pieces[i] != null)
        //         {
        //             // Uncomment and modify these lines based on your PieceClass structure
        //             // currentMission.reward.pieces[i].name = EditorGUILayout.TextField("Name:", currentMission.reward.pieces[i].name);
        //             // currentMission.reward.pieces[i].level = EditorGUILayout.IntField("Level:", currentMission.reward.pieces[i].level);
        //             // currentMission.reward.pieces[i].type = (PieceType)EditorGUILayout.EnumPopup("Type:", currentMission.reward.pieces[i].type);
                    
        //             // Object field if you want to select pieces from scene/assets
        //             currentMission.reward.pieces[i] = EditorGUILayout.ObjectField("Piece:",  
        //             currentMission.reward.pieces[i], typeof(PieceClass), false) as PieceClass;
        //         }
                
        //         if (GUILayout.Button("Remove Piece"))
        //         {
        //             currentMission.reward.pieces.RemoveAt(i);
        //             i--;
        //         }
                
        //         EditorGUILayout.EndVertical();
        //     }
        // }

        // Action buttons
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Save to Repository"))
        {
            if (string.IsNullOrEmpty(currentMission.id))
            {
                EditorUtility.DisplayDialog("Error", "Mission ID cannot be empty", "OK");
            }
            else
            {
                MissionSaveSystem.SaveMission(currentMission, saveSlot);
                repository = MissionSaveSystem.LoadRepository(saveSlot);
                EditorUtility.DisplayDialog("Success", "Mission saved to repository", "OK");
            }
        }
        
        if (GUILayout.Button("Export to JSON"))
        {
            string path = EditorUtility.SaveFilePanel("Save Mission", "", currentMission.title + ".json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                string json = JsonUtility.ToJson(currentMission, true);
                System.IO.File.WriteAllText(path, json);
                EditorUtility.DisplayDialog("Success", "Mission exported to JSON file", "OK");
            }
        }
        
        if (GUILayout.Button("Clear Form"))
        {
            currentMission = null;
            selectedMissionIndex = -1;
        }
        
        EditorGUILayout.EndHorizontal();
        
        // Repository viewer section
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mission Repository", EditorStyles.boldLabel);
        
        showSavedMissions = EditorGUILayout.Foldout(showSavedMissions, "Saved Missions");
        
        if (showSavedMissions)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
            
            if (repository.missions.Count == 0)
            {
                EditorGUILayout.LabelField("No missions found in repository");
            }
            else
            {
                for (int i = 0; i < repository.missions.Count; i++)
                {
                    MissionBase mission = repository.missions[i].Deserialize();
                    
                    EditorGUILayout.BeginHorizontal("box");
                    
                    string missionTitle = $"{mission.title} ({mission.type})";
                    if (GUILayout.Button(missionTitle, EditorStyles.label, GUILayout.Width(250)))
                    {
                        selectedMissionIndex = i;
                        currentMission = mission;
                    }
                    
                    if (GUILayout.Button("Edit", GUILayout.Width(50)))
                    {
                        selectedMissionIndex = i;
                        currentMission = mission;
                    }
                    
                    if (GUILayout.Button("Delete", GUILayout.Width(50)))
                    {
                        if (EditorUtility.DisplayDialog("Delete Mission", 
                            $"Are you sure you want to delete '{mission.title}'?", "Yes", "No"))
                        {
                            MissionSaveSystem.DeleteMission(mission.id, saveSlot);
                            repository = MissionSaveSystem.LoadRepository(saveSlot);
                            if (selectedMissionIndex == i)
                            {
                                selectedMissionIndex = -1;
                                currentMission = null;
                            }
                        }
                    }
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            EditorGUILayout.EndScrollView();
        }
        
        EditorGUILayout.EndVertical();
    }
}
#endif