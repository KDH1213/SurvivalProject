#if UNITY_EDITOR
using Google.Android.AppBundle.Editor.AssetPacks;
using Google.Android.AppBundle.Editor.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
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
    private const string ArgName_EnableGoogleAppBundle = "enableGoogleAppBundle";
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
        bool useGoogleBundle = GetCommandLineArgument(ArgName_EnableGoogleAppBundle) == "true";

        if (useGoogleBundle)
        {
            BuildGoogleAppBundle();
        }
        else
        {
            BuildRegularApk();
        }
    }

    public static void BuildRegularApk()
    {
#if UNITY_ANDROID
        // ----- Jenkins Arguments -----
        int buildNum = int.Parse(GetCommandLineArgument(ArgName_BuildNum) ?? "1");
        string outputPath = GetCommandLineArgument(ArgName_OutputPath);
        string buildVersion = GetCommandLineArgument(ArgName_BuildVersion);
        string buildType = GetCommandLineArgument(ArgName_BuildType);
        bool enalbeAab = buildType == "aab";
        bool enableDev = GetCommandLineArgument(ArgName_EnableDev) == "true";
        bool enableDeepProfiling = GetCommandLineArgument(ArgName_EnableDeepProfiling) == "true";
        string outputFileName = $"{DefaultOutputName}.{buildType}";

        // ----- Determine Output Path -----
        string workspacePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        string outputDirectory = string.IsNullOrEmpty(outputPath) ? System.IO.Path.Combine(workspacePath, "Builds\\Android") : System.IO.Path.GetFullPath(outputPath);

        if (!System.IO.Directory.Exists(outputDirectory))
        {
            System.IO.Directory.CreateDirectory(outputDirectory);
        }

        string fullOutputPath = System.IO.Path.Combine(outputDirectory, outputFileName);

        Debug.Log($"[BuildProcessor] Building Android => {fullOutputPath}");

        // ----- BuildPlayerOptions -----
        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = FindEnabledEditorScenes(),
            locationPathName = fullOutputPath,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
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

        PlayerSettings.bundleVersion = buildVersion;
        PlayerSettings.Android.bundleVersionCode = buildNum;
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = System.IO.Path.Combine(workspacePath, KeystorePath);
        PlayerSettings.Android.keystorePass = KeystorePass;
        PlayerSettings.Android.keyaliasName = KeyaliasName;
        PlayerSettings.Android.keyaliasPass = KeyaliasPass;

        EditorUserBuildSettings.buildAppBundle = enalbeAab;
        EditorUserBuildSettings.development = enableDev;
        EditorUserBuildSettings.buildWithDeepProfilingSupport = enableDeepProfiling;
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        var summary = report.summary;

        Debug.Log($"[BuildProcessor] Result={summary.result}, Time={summary.totalTime.TotalSeconds:F1}s, Size={(summary.totalSize / (1024f * 1024f)):F1} MB");

        switch (summary.result)
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

    public static void BuildGoogleAppBundle()
    {
#if UNITY_ANDROID
        // 명령줄 인자 받기
        string buildVersion = GetCommandLineArgument("buildVersion") ?? "1.0.0";
        int buildNum = int.Parse(GetCommandLineArgument("buildNum") ?? "1");
        string outputPathArg = GetCommandLineArgument("outputPath");
        bool enableDev = GetCommandLineArgument("enableDev") == "true";

        // 출력 경로 계산
        string workspacePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(Application.dataPath, ".."));
        string outputDirectory = string.IsNullOrEmpty(outputPathArg)
            ? System.IO.Path.Combine(workspacePath, "Builds\\Android")
            : System.IO.Path.GetFullPath(outputPathArg);

        System.IO.Directory.CreateDirectory(outputDirectory);
        string outputFile = System.IO.Path.Combine(outputDirectory, $"SurvivalProject.aab");

        PlayerSettings.bundleVersion = buildVersion;
        PlayerSettings.Android.bundleVersionCode = buildNum;

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName  = System.IO.Path.Combine(workspacePath, KeystorePath);
        PlayerSettings.Android.keystorePass  = KeystorePass;
        PlayerSettings.Android.keyaliasName  = KeyaliasName;
        PlayerSettings.Android.keyaliasPass  = KeyaliasPass;

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        EditorUserBuildSettings.development = enableDev;
        EditorUserBuildSettings.buildWithDeepProfilingSupport = enableDev;

        // 4) BuildPlayerOptions 구성 (locationPathName = 최종 .aab 전체 경로)
        var scenes = FindEnabledEditorScenes();
        var buildOptions = enableDev ? BuildOptions.Development : BuildOptions.None;
        if (enableDev)
        {
            buildOptions |= BuildOptions.EnableDeepProfilingSupport;
        }

        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            targetGroup = BuildTargetGroup.Android,
            locationPathName = outputFile,
            options = buildOptions,
        };

        Debug.Log($"[AAB] Build start -> {outputFile}");
        bool isSucceeded = AppBundlePublisher.Build(buildPlayerOptions, AssetPackConfigSerializer.LoadConfig(), true);

        if (isSucceeded)
        {
            Debug.Log($"[AAB] Build Succeeded: {outputFile}");
        }
        else
        {
            Debug.LogError("[AAB] Build Failed (AppBundlePublisher.Build returned false)");
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
#endif