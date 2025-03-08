using System.IO;
using UnityEngine;

public static class JsonLoader
{
    public static T LoadJson<T>(string fileName) where T : class
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);

#if !UNITY_EDITOR && UNITY_ANDROID
        try
        {
            using (var request = UnityEngine.Networking.UnityWebRequest.Get(path))
            {
                request.SendWebRequest();
                while (!request.isDone) { } 

                if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    return JsonUtility.FromJson<T>(json);
                }
                else
                {
                    Debug.LogError($"Erreur de chargement du fichier {fileName} : {request.error}");
                    return null;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception lors du chargement de {fileName} : {e.Message}");
            return null;
        }
#elif !UNITY_EDITOR && UNITY_IOS
        try
        {
            // iOS requires proper file:// protocol
            string iosPath = "file://" + path;
            
            using (var request = UnityEngine.Networking.UnityWebRequest.Get(iosPath))
            {
                request.SendWebRequest();
                while (!request.isDone) { }

                if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    string json = request.downloadHandler.text;
                    return JsonUtility.FromJson<T>(json);
                }
                else
                {
                    Debug.LogError($"Erreur iOS: {fileName} : {request.error}");
                    Debug.LogError($"Chemin tenté: {iosPath}");
                    return null;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Exception iOS pour {fileName} : {e.Message}");
            return null;
        }
#else
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogError($"Fichier JSON non trouvé : {fileName}");
            Debug.LogError($"Chemin complet : {path}");
            return null;
        }
#endif
    }
}
