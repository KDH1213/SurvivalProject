using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildProcessor
{
    private const string KeystorePath = "user.keystore";
    private const string KeystorePass = "944512";
    private const string KeyaliasName = "release";
    private const string KeyaliasPass = "944512";

    private const string ArgName_BuildNum = "buildNum";
    private const string ArgName_OutputPath = "outputPath";
    private const string ArgName_BuildType = "buildType";
    private const string ArgName_BuildVersion = "buildVersion";
    private const string ArgName_EnableDev = "enableDev";
    private const string ArgName_EnableDeepProfiling = "enableDeepProfiling";
    private const string DefaultOutputName = "SurvivalProject";

    private static string GetCommandLineArgument(string name)
    {
        var args = Environment.GetCommandLineArgs();
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == $"-{name}" && i + 1 < args.Length)
                return args[i + 1];
        }
        return null;
    }

    public static void BuildAndroid()
    {
#if !UNITY_ANDROID
        // ----- Jenkins Arguments -----
        int buildNum = int.Parse(GetCommandLineArgument(ArgName_BuildNum) ?? "1");
        string buildVersion = GetCommandLineArgument(ArgName_BuildVersion) ?? "0.0.1";
        string buildType = GetCommandLineArgument(ArgName_BuildType) ?? "apk";
        bool enableDev = GetCommandLineArgument(ArgName_EnableDev) == "true";
        bool enableDeepProfiling = GetCommandLineArgument(ArgName_EnableDeepProfiling) == "true";
        string outputPathArg = GetCommandLineArgument(ArgName_OutputPath);

        // ----- Determine Output Path -----
        string workspacePath = Application.dataPath + "/.."; // Unity 프로젝트 루트
        string outputDirectory = string.IsNullOrEmpty(outputPathArg) ? System.IO.Path.Combine(workspacePath, "Builds/Android") : outputPathArg;

        if (!System.IO.Directory.Exists(outputDirectory))
        {
            System.IO.Directory.CreateDirectory(outputDirectory);
        }

        bool enableAab = buildType.Equals("aab", StringComparison.OrdinalIgnoreCase);
        string fileExtension = enableAab ? "aab" : "apk";
        string outputFileName = $"{DefaultOutputName}_{buildVersion}_{buildNum}.{fileExtension}";
        string fullOutputPath = System.IO.Path.Combine(outputDirectory, outputFileName);

        Debug.Log($"[BuildProcessor] Building Android => {fullOutputPath}");
        Debug.Log($"[BuildProcessor] Dev={enableDev}, DeepProfile={enableDeepProfiling}, AAB={enableAab}");

        // ----- BuildPlayerOptions -----
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = FindEnabledEditorScenes(),
            locationPathName = fullOutputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        if (enableDev)
        {
            buildPlayerOptions.options |= BuildOptions.Development;
        }
        if (enableDeepProfiling)
        {
            buildPlayerOptions.options |= BuildOptions.EnableDeepProfilingSupport;
        }

        // ----- Player Settings -----
        PlayerSettings.bundleVersion = buildVersion;
        PlayerSettings.Android.bundleVersionCode = buildNum;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = System.IO.Path.Combine(workspacePath, KeystorePath);
        PlayerSettings.Android.keystorePass = KeystorePass;
        PlayerSettings.Android.keyaliasName = KeyaliasName;
        PlayerSettings.Android.keyaliasPass = KeyaliasPass;

        // ----- Google AAB 빌드 설정 -----
        EditorUserBuildSettings.buildAppBundle = enableAab;
        EditorUserBuildSettings.development = enableDev;
        EditorUserBuildSettings.buildWithDeepProfilingSupport = enableDeepProfiling;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        // ----- 실제 빌드 실행 -----
        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        Debug.Log($"[BuildProcessor] Result={summary.result}, Time={summary.totalTime.TotalSeconds:F1}s, Size={(summary.totalSize / (1024f * 1024f)):F1} MB");

        switch (summary.result)
        {
            case BuildResult.Succeeded:
                Debug.Log("[BuildProcessor] ✅ Android Build succeeded!");
                break;
            case BuildResult.Failed:
                Debug.LogError("[BuildProcessor] ❌ Android Build failed!");
                break;
            case BuildResult.Cancelled:
                Debug.LogWarning("[BuildProcessor] ⚠️ Android Build cancelled!");
                break;
            default:
                Debug.LogWarning("[BuildProcessor] ⚠️ Unknown build result.");
                break;
        }
#endif
    }

    private static string[] FindEnabledEditorScenes()
    {
        var editorScenes = new List<string>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                editorScenes.Add(scene.path);
            }
        }
        return editorScenes.ToArray();
    }
}