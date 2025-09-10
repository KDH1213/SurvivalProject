using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildProcessor
{
    private const string KeystorePath = "KeystorePath/keystore.keystore";
    private const string keystorePass = "keystorePass1234";
    private const string KeyaliasName = "keyAliasName";
    private const string KeyaliasPass = "keyAliasPass";


    private const string ArgName_BuildNum = "buildNum";
    private const string ArgName_OutputPath = "outputPath";
    private const string ArgName_BuildType = "buildType";
    private const string ArgName_BuildVersion = "buildVersion";
    private const string ArgName_EnableDev = "enableDev";
    private const string ArgName_EnableDeepProfiling = "enableDeepProfiling";
    private const string ArgName_OutputFileName = "outputFileName";

    private static string GetCommandLineArgument(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == $"-{name}" && i + 1 < args.Length)
                return args[i + 1];
        }
        return null;
    }

    public static void BuildAndroid()
    {
#if UNITY_ANDROID
        // Jenkins 에서 세팅한 Arguments들
        var buildNum = int.Parse(GetCommandLineArgument(ArgName_BuildNum));
        var outputPath = GetCommandLineArgument(ArgName_OutputPath);
        var version = GetCommandLineArgument(ArgName_BuildVersion);
        var extension = GetCommandLineArgument(ArgName_BuildType);
        var enableAab = extension == "aab"; // AAB 빌드 여부
        var enableDev = GetCommandLineArgument(ArgName_EnableDev) == "true"; // Dev Build 여부
        var enableDeepProfiling = GetCommandLineArgument(ArgName_EnableDeepProfiling) == "true"; // Dev Build 여부
        var outputFileName = GetCommandLineArgument(ArgName_OutputFileName);

        // BuildPlayerOptions 설정하는 부분. 빌드 세팅에서 추가한 Scene들
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = FindEnabledEditorScenes(),
            locationPathName = outputPath,
            target = BuildTarget.Android
        };

        EditorUserBuildSettings.buildAppBundle = enableAab; //.aab로 추출한것인지
        EditorUserBuildSettings.development = enableDev;
        EditorUserBuildSettings.buildWithDeepProfilingSupport = enableDeepProfiling;

        // PlayerSettings
        PlayerSettings.bundleVersion = version;
        PlayerSettings.Android.bundleVersionCode = buildNum;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = KeystorePath;
        PlayerSettings.Android.keystorePass = keystorePass;
        PlayerSettings.Android.keyaliasName = KeyaliasName;
        PlayerSettings.Android.keyaliasPass = KeyaliasPass;

        // 여기서 백그라운드를 통해 batchmode로 실제로 빌드가 실행된다. 
        // 자세한 로그는 fastlane cmd 나 Jenkins log에서 확인이 가능하다. (에러 터지면 얘네들로 찾아야함)
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        switch (report.summary.result)
        {
            case BuildResult.Succeeded:
            case BuildResult.Failed:
            case BuildResult.Unknown:
            case BuildResult.Cancelled:
                Debug.Log($"빌드 결과 : {report.summary.result}");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
#endif
    }

    // Enable 처리된 Scene들 가져오는 부분
    private static string[] FindEnabledEditorScenes()
    {
        var editorScenes = new List<string>();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            editorScenes.Add(scene.path);
        }

        return editorScenes.ToArray();
    }
}