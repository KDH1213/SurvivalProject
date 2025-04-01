
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
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
        Converters = { new Vector3Converter() }
       
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

    public static bool Load(int slot = 0)
    {
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);

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
