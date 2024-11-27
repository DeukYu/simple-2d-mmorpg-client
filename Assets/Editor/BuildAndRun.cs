using UnityEditor;
using UnityEngine;

public class BuildAndRun
{
    [MenuItem("Tools/Run_MultPlayer/2 Players")]
    static void PerformWin64Build2()
    {
        PerformBuild(2);
    }
    [MenuItem("Tools/Run_MultPlayer/3 Players")]
    static void PerformWin64Build3()
    {
        PerformBuild(3);
    }
    [MenuItem("Tools/Run_MultPlayer/4 Players")]
    static void PerformWin64Build4()
    {
        PerformBuild(4);
    }
    static void PerformBuild(int playerCount)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        for (int i = 0; i < playerCount; i++)
        {
            BuildPipeline.BuildPlayer(GetScenePaths()
                , $"Builds/Win64/{GetProjectName()}{i}/{GetProjectName()}{i}.exe"
                , BuildTarget.StandaloneWindows
                , BuildOptions.AutoRunPlayer);

        }
        string[] scenes = GetScenePaths();
        string path = $"Builds/{GetProjectName()}_{playerCount}P";
        if (playerCount == 1)
            path += "/Single";
        else
            path += "/Multi";
    }
    static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        return s[s.Length - 2];
    }
    static string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
}
