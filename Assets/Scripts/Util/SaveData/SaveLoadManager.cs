
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveDataVC = SaveDataV1;

public static class SaveLoadManager
{
    // 클라이언트의 버전
    public static int SaveDataVersion { get; private set; } = 1;

    // Data를 게임에 맞게 제작해도 됨
    // 현재 진행되고 있는 게임의 데이터
    // 세이브, 로드에 데이터를 넘겨 받아서 정보를 불러오거나 저장해도 됨
    public static SaveDataVC Data { get; set; }

    private static readonly string[] SaveFileName =
    {
        "SaveAuto.json",
        "Save1.json",
        "Save2.json",
        "Save3.json",
    };

    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        TypeNameHandling = TypeNameHandling.All,
        Converters = { new Vector3Converter() , new QuaternionConverter(), new Vector3IntConverter() }
       
    };

    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    static SaveLoadManager()
    {
        if (!Load())
        {
            Data = new SaveDataVC();
            Save();
        }
    }

    public static bool Save(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        var json = JsonConvert.SerializeObject(Data, settings);
        File.WriteAllText(path, json);

        return true;
    }

    public static bool Save(string name)
    {
        if (Data == null || name == string.Empty)
            return false;

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }
        var path = Path.Combine(SaveDirectory, $"{name}.json");

        var json = JsonConvert.SerializeObject(Data, settings);

        File.WriteAllText(path, json);

        return true;
    }

    public static bool SaveStage(int slot = 0)
    {
        if (Data == null || slot < 0 || slot >= SaveFileName.Length)
            return false;

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        // var json = File.ReadAllText(path);

        //var jsonPlayer = JsonConvert.SerializeObject(Data.PlayerSaveData, settings);

        //var jobject = JObject.Parse(json);
        //var jToken = jobject["PlayerSaveData"];
        //jToken.Replace(jsonPlayer);

        var json = JsonConvert.SerializeObject(Data, settings);
        File.WriteAllText(path, json);

        return true;
    }

    public static bool Load(int slot = 0)
    {
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);

        if (!File.Exists(path))
            return false;

        var json = File.ReadAllText(path);

        var jobject = JObject.Parse(json);
        var jToken = jobject["PlayerSaveData"];
        // var jTokenStageSaveData1 = jobject["StageSaveData"];
        // var aaaa = jobject["stageSaveDatas"].;

        // Debug.Log(jToken.ToString());
        // Debug.Log(jTokenStageSaveData1.ToString());
        // Debug.Log(jTokenStageSaveData.ToString());
        // Debug.Log(jTokenStageSaveData2.ToString());
        // var jTokens = jobject["stageSaveDatas"]["StageSaveData"].ToArray();
        // var jTokenStageSaveData = jobject["stageSaveDatas"]["StageSaveData"];

        //foreach (var jTokenz in jTokens)
        //{
        //    Debug.Log(jTokenz.ToString());
        //}

        
        var playerData = JsonConvert.DeserializeObject<PlayerSaveData>(jToken.ToString(), settings);
        // var stageData = JsonConvert.DeserializeObject<StageSaveData>(jTokenStageSaveData.ToString(), settings);

        // Debug.Log(stageData);

        //Data = new SaveDataVC();

        Data.PlayerSaveData = playerData;

        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }

        // Data.stageSaveDatas[0] = stageData;

        // while (saveData.Version < SaveDataVersion)
        // {
        //     saveData = saveData.VersionUp();
        // }

        Data = saveData as SaveDataVC;

        return true;
    }

    public static bool Load(string name)
    {
        var path = Path.Combine(SaveDirectory, $"{name}.json");

        if (!File.Exists(path))
            return false;

        var json = File.ReadAllText(path);
        var saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

        while (saveData.Version < SaveDataVersion)
        {
            saveData = saveData.VersionUp();
        }

        Data = saveData as SaveDataVC;

        return true;
    }
}
