using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class MissionRepository
{
    public List<MissionData> missions = new List<MissionData>();
    public List<MissionData> activeMissions = new List<MissionData>();
    public List<MissionData> completedMissions = new List<MissionData>();
    
    // Helper class to serialize/deserialize different mission types
    [System.Serializable]
    public class MissionData
    {
        public MissionType type;
        public string serializedMission;
        
        public MissionBase Deserialize()
        {
            switch (type)
            {
                case MissionType.Attack:
                    return JsonUtility.FromJson<AttackMission>(serializedMission);
                case MissionType.Delivery:
                    return JsonUtility.FromJson<DeliveryMission>(serializedMission);
                case MissionType.Mining:
                    return JsonUtility.FromJson<MiningMission>(serializedMission);
                case MissionType.Talk:
                    return JsonUtility.FromJson<TalkMission>(serializedMission);
                default:
                    return null;
            }
        }
        
        public static MissionData Serialize(MissionBase mission)
        {
            return new MissionData
            {
                type = mission.type,
                serializedMission = JsonUtility.ToJson(mission)
            };
        }
    }
    
    // Add a mission to the available missions pool
    public void AddMission(MissionBase mission)
    {
        missions.Add(MissionData.Serialize(mission));
    }
    
    // Start a mission (move from available to active)
    public MissionBase ActivateMission(int index)
    {
        if (index >= 0 && index < missions.Count)
        {
            MissionData missionData = missions[index];
            activeMissions.Add(missionData);
            missions.RemoveAt(index);
            return missionData.Deserialize();
        }
        return null;
    }
    
    // Complete a mission (move from active to completed)
    public void CompleteMission(int index)
    {
        if (index >= 0 && index < activeMissions.Count)
        {
            completedMissions.Add(activeMissions[index]);
            activeMissions.RemoveAt(index);
        }
    }

    // Helper method to find mission by ID
    public MissionBase FindMissionById(string id)
    {
        // Check available missions
        foreach (var missionData in missions)
        {
            MissionBase mission = missionData.Deserialize();
            if (mission != null && mission.id == id)
            {
                return mission;
            }
        }
        
        // Check active missions
        foreach (var missionData in activeMissions)
        {
            MissionBase mission = missionData.Deserialize();
            if (mission != null && mission.id == id)
            {
                return mission;
            }
        }
        
        return null;
    }
}