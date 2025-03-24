using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

public class MissionManager : MonoBehaviour
{
    public MissionRepository repository = new MissionRepository();
    private List<MissionBase> runtimeActiveMissions = new List<MissionBase>();
    private Dictionary<string, System.Action> missionEvents = new Dictionary<string, System.Action>();
    private Dictionary<string, MissionBase> missionLookup = new Dictionary<string, MissionBase>();
    
    // Singleton pattern (optional)
    public static MissionManager Instance { get; private set; }


    public void Load()
    {
        
        
        repository = MissionSaveSystem.LoadRepository("default");
        BuildMissionLookup();
    }


    public void Save()
    {
        MissionSaveSystem.SaveRepository(repository);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
       // Load();
    }
    
    // Register event handlers for mission events
    public void RegisterMissionEvent(string eventId, System.Action action)
    {
        missionEvents[eventId] = action;
    }
    
    // Trigger a mission event
    public void TriggerMissionEvent(string eventId)
    {
        if (missionEvents.TryGetValue(eventId, out var action))
        {
            action?.Invoke();
        }
        else
        {
            Debug.LogWarning($"No handler registered for mission event: {eventId}");
        }
    }
    
    // Load a mission from JSON file
    public void LoadMissionFromFile(string filePath)
    {
        try
        {
            string json = System.IO.File.ReadAllText(filePath);
            // Determine mission type
            JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(json);
            
            MissionBase mission = null;
            switch (wrapper.type)
            {
                case MissionType.Attack:
                    mission = JsonUtility.FromJson<AttackMission>(json);
                    break;
                case MissionType.Delivery:
                    mission = JsonUtility.FromJson<DeliveryMission>(json);
                    break;
                case MissionType.Mining:
                    mission = JsonUtility.FromJson<MiningMission>(json);
                    break;
                case MissionType.Talk:
                    mission = JsonUtility.FromJson<TalkMission>(json);
                    break;
            }
            
            if (mission != null)
            {
                mission.Initialize();
                repository.AddMission(mission);
                Debug.Log($"Loaded mission: {mission.title}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load mission from {filePath}: {e.Message}");
        }
    }
    
    // Start a mission
    public void StartMission(int missionIndex)
    {
        MissionBase mission = repository.ActivateMission(missionIndex);
        if (mission != null)
        {
            mission.Initialize();
            runtimeActiveMissions.Add(mission);
            
            // Trigger onStart event if specified
            if (!string.IsNullOrEmpty(mission.onStartEvent))
            {
                TriggerMissionEvent(mission.onStartEvent);
            }
            
            Debug.Log($"Started mission: {mission.title}");
        }
    }
    
    // Start a mission by ID
    public void StartMissionById(string missionId)
    {
        // Find mission in available missions
        for (int i = 0; i < repository.missions.Count; i++)
        {
            MissionBase mission = repository.missions[i].Deserialize();
            if (mission != null && mission.id == missionId)
            {
                StartMission(i);
                return;
            }
        }
        
        Debug.LogWarning($"Mission with ID {missionId} not found or not available");
    }
    
    // Update mission progress
    public void UpdateMissions()
    {
        for (int i = 0; i < runtimeActiveMissions.Count; i++)
        {
            MissionBase mission = runtimeActiveMissions[i];
            mission.UpdateProgress();
            
            // Check if mission was just completed
            if (mission.IsComplete)
            {
                CompleteMission(i);
                i--; // Adjust index since we removed an element
            }
        }
    }
    
    // Complete a mission
    private void CompleteMission(int index)
    {
        if (index >= 0 && index < runtimeActiveMissions.Count)
        {
            MissionBase mission = runtimeActiveMissions[index];
            
            // Save reference to next mission ID before removing the mission
            string nextMissionId = mission.nextMissionId;
            
            // Handle mission completion
            if (!string.IsNullOrEmpty(mission.onCompleteEvent))
            {
                TriggerMissionEvent(mission.onCompleteEvent);
            }
            
            // Handle rewards
            if (mission.reward != null)
            {
                // Process rewards
                Debug.Log($"Rewarding player: XP: {mission.reward.xp}, Money: {mission.reward.Argent}");
            }
            
            // Update repository and runtime lists
            repository.CompleteMission(index);
            runtimeActiveMissions.RemoveAt(index);
            
            Debug.Log($"Completed mission: {mission.title}");
            
            // Start next mission in chain if specified
            if (!string.IsNullOrEmpty(nextMissionId))
            {
                StartMissionById(nextMissionId);
            }
        }
    }
    
    // Save all mission data to PlayerPrefs
    public void SaveMissions()
    {
        string json = JsonUtility.ToJson(repository);
        PlayerPrefs.SetString("SavedMissions", json);
        PlayerPrefs.Save();
        Debug.Log("Missions saved to PlayerPrefs");
    }
    
    // Load mission data from PlayerPrefs
    // public void LoadMissions()
    // {
    //     if (PlayerPrefs.HasKey("SavedMissions"))
    //     {
    //         string json = PlayerPrefs.GetString("SavedMissions");
    //         repository = JsonUtility.FromJson<MissionRepository>(json);
    //         Debug.Log("Missions loaded from PlayerPrefs");
            
    //         // Recreate runtime active missions
    //         runtimeActiveMissions.Clear();
    //         foreach (var missionData in repository.activeMissions)
    //         {
    //             MissionBase mission = missionData.Deserialize();
    //             if (mission != null)
    //             {
    //                 mission.Initialize();
    //                 runtimeActiveMissions.Add(mission);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("No saved missions found");
    //         repository = new MissionRepository();
    //     }
    // }
    
    // Helper class for deserializing mission type
    [Serializable]
    private class JsonWrapper
    {
        public MissionType type;
    }
    
    // After loading missions, build the ID lookup dictionary
    private void BuildMissionLookup()
    {
        missionLookup.Clear();
        
        // Add available missions to lookup
        foreach (var missionData in repository.missions)
        {
            MissionBase mission = missionData.Deserialize();
            if (mission != null && !string.IsNullOrEmpty(mission.id))
            {
                missionLookup[mission.id] = mission;
            }
        }
    }
}