using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

namespace SanAndreasUnity.Editor
{
    public static class BuildWebGLScript
    {
        [MenuItem("SanAndreasUnity/Build WebGL")]
        public static void Build()
        {
            Debug.Log("[BuildWebGL] Starting SanAndreasUnity WebGL batch build...");

            // Configure WebGL player settings
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Brotli;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.runInBackground = true;

            // Set Scenes from EditorBuildSettings
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            string[] scenePaths = new string[scenes.Length];
            for (int i = 0; i < scenes.Length; i++)
            {
                scenePaths[i] = scenes[i].path;
                Debug.Log($"[BuildWebGL] Including scene {i}: {scenes[i].path}");
            }

            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "Builds", "WebGL");
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenePaths,
                locationPathName = outputFolder,
                target = BuildTarget.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"[BuildWebGL] Build succeeded! Size: {summary.totalSize} bytes, Time: {summary.totalTime.TotalSeconds:F1}s");
            }
            else
            {
                Debug.LogError($"[BuildWebGL] Build failed with {summary.totalErrors} errors.");
                EditorApplication.Exit(1);
            }
        }
    }
}
