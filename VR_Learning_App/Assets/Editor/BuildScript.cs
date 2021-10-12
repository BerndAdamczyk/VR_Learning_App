using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Ionic.Zip; // this uses the Unity port of DotNetZip https://github.com/r2d2rigo/dotnetzip-for-unity

// place in an "Editor" folder in your Assets folder
public class BuildScript
{
	// TODO: turn this into a wizard or something??? whatever

	[MenuItem("BuildScript/Build Windows/Development")]
	public static void StartWindowsDev()
	{
		//Development Flag muss hier noch einprogrammiert werden.

		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Build out WINDOWS to...",
													GetProjectFolderPath() + "/Builds/",
													"");
		var filename = path.Split('/'); // do this so I can grab the project folder name
		BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
	}
	[MenuItem("BuildScript/Build Windows/Release")]
	public static void StartWindowsRelease()
	{
		//Release Flag muss hier noch einprogrammiert werden.

		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Build out WINDOWS to...",
													GetProjectFolderPath() + "/Builds/",
													"");
		var filename = path.Split('/'); // do this so I can grab the project folder name
		BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");
	}

	[MenuItem("BuildScript/Build Windows + Mac OSX")]
	public static void StartAll()
	{
		// Get filename.
		string path = EditorUtility.SaveFolderPanel("Build out ALL STANDALONES to...",
													GetProjectFolderPath() + "/Builds/",
													"");
		var filename = path.Split('/'); // do this so I can grab the project folder name
		BuildPlayer(BuildTarget.StandaloneOSX, filename[filename.Length - 1], path + "/");
		BuildPlayer(BuildTarget.StandaloneWindows, filename[filename.Length - 1], path + "/");

	}

	// this is the main player builder function
	static void BuildPlayer(BuildTarget buildTarget, string filename, string path)
	{
		string fileExtension = "";
		string dataPath = "";
		string modifier = "";

		// configure path variables based on the platform we're targeting
		switch (buildTarget)
		{
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				modifier = "_windows";
				fileExtension = ".exe";
				dataPath = "_Data/";
				break;
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSX:
				modifier = "_mac-osx";
				fileExtension = ".app";
				dataPath = fileExtension + "/Contents/";
				break;
		}

		Debug.Log("====== BuildPlayer: " + buildTarget.ToString() + " at " + path + filename);
		EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);

		// build out the player
		string buildPath = path + filename + modifier + "/";
		Debug.Log("buildpath: " + buildPath);
		string playerPath = buildPath + filename + modifier + fileExtension;
		Debug.Log("playerpath: " + playerPath);
		BuildPipeline.BuildPlayer(GetScenePaths(), playerPath, buildTarget, buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None);

		// Copy files over into builds
		string fullDataPath = buildPath + filename + modifier + dataPath;
		Debug.Log("fullDataPath: " + fullDataPath);
		CopyFromProjectAssets(fullDataPath, "languages"); // language text files that Radiator uses
														  // TODO: copy over readme

		// ZIP everything
		CompressDirectory(buildPath, path + "/" + filename + modifier + ".zip");
	}

	// from http://wiki.unity3d.com/index.php?title=AutoBuilder
	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		return scenes;
	}

	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}

	static string GetProjectFolderPath()
	{
		var s = Application.dataPath;
		s = s.Substring(s.Length - 7, 7); // remove "Assets/"
		return s;
	}

	// copies over files from somewhere in my project folder to my standalone build's path
	// do not put a "/" at beginning of assetsFolderName
	static void CopyFromProjectAssets(string fullDataPath, string assetsFolderPath, bool deleteMetaFiles = true)
	{
		Debug.Log("CopyFromProjectAssets: copying over " + assetsFolderPath);
		FileUtil.ReplaceDirectory(Application.dataPath + "/" + assetsFolderPath, fullDataPath + assetsFolderPath); // copy over languages

		// delete all meta files
		if (deleteMetaFiles)
		{
			var metaFiles = Directory.GetFiles(fullDataPath + assetsFolderPath, "*.meta", SearchOption.AllDirectories);
			foreach (var meta in metaFiles)
			{
				FileUtil.DeleteFileOrDirectory(meta);
			}
		}
	}

	// compress the folder into a ZIP file, uses https://github.com/r2d2rigo/dotnetzip-for-unity
	static void CompressDirectory(string directory, string zipFileOutputPath)
	{
		Debug.Log("attempting to compress " + directory + " into " + zipFileOutputPath);
		// display fake percentage, I can't get zip.SaveProgress event handler to work for some reason, whatever
		EditorUtility.DisplayProgressBar("COMPRESSING... please wait", zipFileOutputPath, 0.38f);
		using (ZipFile zip = new ZipFile())
		{
			zip.ParallelDeflateThreshold = -1; // DotNetZip bugfix that corrupts DLLs / binaries http://stackoverflow.com/questions/15337186/dotnetzip-badreadexception-on-extract
			zip.AddDirectory(directory);
			zip.Save(zipFileOutputPath);
		}
		EditorUtility.ClearProgressBar();
	}

}