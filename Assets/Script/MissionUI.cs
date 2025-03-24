using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject missionFramelPrefab;
    [SerializeField] private GameObject missionFramelFin;


    private Transform missionListContainer;
    private MissionRepository missionRepository;
    
    public enum MissionListType
    {
        Available,
        Active,
        Completed
    }

    public void Awake()
    {
        missionListContainer = transform.Find("[Container]").GetChild(0).GetChild(0).GetChild(0);
    }

    public void CloseMissions()
    {
        GameObject.Find("Canvas").GetComponent<CanvasGroup>().blocksRaycasts = true;
        //GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = true;
       // GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 1f;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void DisplayMissions()
    {
        GameObject.Find("Canvas").GetComponent<CanvasGroup>().blocksRaycasts = false;
       // GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = false;
        //GameObject.Find("Canvas").GetComponent<CanvasGroup>().alpha = 0f;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        missionRepository = GameObject.Find("[MissionManager]").GetComponent<MissionManager>().repository;

        MissionListType listType = MissionListType.Available;
        // Supprimer les panels existants
        ClearMissionPanels();
        
        List<MissionRepository.MissionData> missionsToDisplay = new List<MissionRepository.MissionData>();
        
        // Sélectionner la liste à afficher
        switch (listType)
        {
            case MissionListType.Available:
                missionsToDisplay = missionRepository.missions;
                break;
            case MissionListType.Active:
                missionsToDisplay = missionRepository.activeMissions;
                break;
            case MissionListType.Completed:
                missionsToDisplay = missionRepository.completedMissions;
                break;
        }
        
        // Instancier un panel pour chaque mission
        foreach (var missionData in missionsToDisplay)
        {
            GameObject panelInstance = Instantiate(missionFramelPrefab, missionListContainer);
            MissionBase mission = missionData.Deserialize();
            ConfigureMissionPanel(panelInstance, mission);
        }
        Instantiate(missionFramelFin, missionListContainer);


    }
    
    private void ClearMissionPanels()
    {
        // Détruire tous les enfants du conteneur
        foreach (Transform child in missionListContainer)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void ConfigureMissionPanel(GameObject panel, MissionBase mission)
    {

        
        panel.transform.Find("titre").GetComponent<Text>().text = mission.title;
        panel.transform.Find("titre").Find("Sous-titre").GetComponent<Text>().text ="   -" + mission.chapter;
        panel.transform.Find("description").GetComponent<Text>().text = mission.description;

        panel.transform.Find("reward-text").Find("xp").GetComponent<Text>().text = mission.reward.xp.ToString() + "xp";
        panel.transform.Find("reward-text").Find("argent").GetComponent<Text>().text = mission.reward.Argent.ToString() + "$";

        // Set progress bar position based on mission progress
        Transform progressBar = panel.transform.Find("bar-frame").Find("mask").Find("bar");
        if (progressBar != null)
        {
            RectTransform barRect = progressBar.GetComponent<RectTransform>();
            float fullWidth = barRect.sizeDelta.x;
            float progressWidth = fullWidth * mission.ProgressPercentage ;
            
            barRect.sizeDelta = new Vector2(progressWidth, barRect.sizeDelta.y);
            
            Text progressText = panel.transform.Find("bar-frame").Find("progression-text").GetComponent<Text>();
            if (progressText != null)
            {
                progressText.text = mission.getProgressText();
            }
        }

        //Button acceptButton = panel.transform.Find("AcceptButton").GetComponent<Button>();
        
        // Configurer les éléments UI avec les données de la mission
       
       
        //if (rewardText != null) rewardText.text = "Récompense: " + mission.rewardAmount;
        
        // Ajouter un listener au bouton pour accepter la mission
        // if (acceptButton != null)
        // {
        //     acceptButton.onClick.AddListener(() => {
        //         int index = missionRepository.missions.IndexOf(MissionRepository.MissionData.Serialize(mission));
        //         missionRepository.ActivateMission(index);
        //         // Rafraîchir l'affichage
        //         DisplayMissions();
        //     });
        // }
        
        // // Personnalisation additionnelle selon le type de mission
        // CustomizeMissionPanel(panel, mission);
    }
    
    private void CustomizeMissionPanel(GameObject panel, MissionBase mission)
    {
        // Personnaliser le panel en fonction du type de mission
        switch (mission.type)
        {
            case MissionType.Attack:
                AttackMission attackMission = mission as AttackMission;
                TextMeshProUGUI targetText = panel.transform.Find("TargetText")?.GetComponent<TextMeshProUGUI>();
                if (targetText != null && attackMission != null) 
                    targetText.text = "Cible: " + attackMission.targetName;
                break;
                
            case MissionType.Delivery:
                DeliveryMission deliveryMission = mission as DeliveryMission;
                TextMeshProUGUI destinationText = panel.transform.Find("DestinationText")?.GetComponent<TextMeshProUGUI>();
                // if (destinationText != null && deliveryMission != null) 
                //     destinationText.text = "Destination: " + deliveryMission.destinationName;
                 break;
                
            case MissionType.Mining:
                MiningMission miningMission = mission as MiningMission;
                TextMeshProUGUI resourceText = panel.transform.Find("ResourceText")?.GetComponent<TextMeshProUGUI>();
                // if (resourceText != null && miningMission != null) 
                //     resourceText.text = "Ressource: " + miningMission.resourceType;
                 break;
                
            case MissionType.Talk:
                TalkMission talkMission = mission as TalkMission;
                TextMeshProUGUI npcText = panel.transform.Find("NPCText")?.GetComponent<TextMeshProUGUI>();
                // if (npcText != null && talkMission != null) 
                //     npcText.text = "PNJ: " + talkMission.npcName;
                break;
        }
    }
}