using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class MissionSaveSystem
{
    private static readonly string START_FOLDER = Application.dataPath + "/JSON/";
    private static readonly string MISSIONS_FOLDER = Application.persistentDataPath + "/Saves/";
    
    public static void Initialize()
    {
        if (!Directory.Exists(MISSIONS_FOLDER))
        {
            Directory.CreateDirectory(MISSIONS_FOLDER);
        }
    }
    
    // Save repository to JSON file
    public static void SaveRepository(MissionRepository repository, string saveSlot = "default")
    {
        Initialize();
        string json = JsonUtility.ToJson(repository, true); // true for pretty print

        string path = MISSIONS_FOLDER + saveSlot + "_missions.json";
        if(saveSlot == "start")
        {
            path = START_FOLDER + "start_missions.json";
        }

        File.WriteAllText(path, json);
        Debug.Log($"Mission repository saved to {MISSIONS_FOLDER + saveSlot + "_missions.json"}");
    }
    
    // Load repository from JSON file
    public static MissionRepository LoadRepository(string saveSlot = "default")
    {
        Initialize();
        string path = MISSIONS_FOLDER + saveSlot + "_missions.json";
        

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            MissionRepository repository = JsonUtility.FromJson<MissionRepository>(json);
            Debug.Log($"Mission repository loaded from {path}");
            return repository;
        }
        else
        {
            //Debug.Log($"No mission repository found at {path}, creating a new one");
            path = START_FOLDER + "start_missions.json";
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<MissionRepository>(json);          
        }
    }
    
    // Add a single mission to the repository
    public static void SaveMission(MissionBase mission, string saveSlot = "default")
    {
        // Load existing repository
        MissionRepository repository = LoadRepository(saveSlot);
        
        // Check if mission already exists (by ID)
        bool missionExists = false;
        for (int i = 0; i < repository.missions.Count; i++)
        {
            MissionBase existingMission = repository.missions[i].Deserialize();
            if (existingMission.id == mission.id)
            {
                // Replace the existing mission
                repository.missions[i] = MissionRepository.MissionData.Serialize(mission);
                missionExists = true;
                break;
            }
        }
        
        // If mission doesn't exist, add it
        if (!missionExists)
        {
            repository.AddMission(mission);
        }
        
        // Save the updated repository
        SaveRepository(repository, saveSlot);
    }
    
    // Delete a mission from the repository
    public static bool DeleteMission(string missionId, string saveSlot = "default")
    {
        MissionRepository repository = LoadRepository(saveSlot);
        
        for (int i = 0; i < repository.missions.Count; i++)
        {
            MissionBase mission = repository.missions[i].Deserialize();
            if (mission.id == missionId)
            {
                repository.missions.RemoveAt(i);
                SaveRepository(repository, saveSlot);
                return true;
            }
        }
        
        return false;
    }
}