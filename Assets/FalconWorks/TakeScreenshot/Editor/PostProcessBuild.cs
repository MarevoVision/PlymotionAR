using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public static class PostProcessBuild {

	[PostProcessBuild]
	public static void OnPostProcessBuild( BuildTarget buildTarget, string path)
	{
		if(buildTarget == BuildTarget.iOS)
		{
			string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject pbxProject = new PBXProject();
			pbxProject.ReadFromFile(projectPath);

			string target = pbxProject.TargetGuidByName("Unity-iPhone");  

			string plistPath = Path.Combine(path, "Info.plist");
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);
//			Privacy - Photo Library Additions Usage Description
			plist.root.SetString("NSPhotoLibraryAddUsageDescription", "Add photos to the album.");
			plist.root.SetString("NSPhotoLibraryUsageDescription", "Save the photo to an album.");
			plist.root.SetString("NSCameraUsageDescription", "This app uses the camera function of the iPhone.");
			plist.WriteToFile(plistPath);          

			// ▽▽▽▽

			pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
			pbxProject.AddFrameworkToProject (target, "AssetsLibrary.framework", true);

//			pbxProject.AddFrameworkToProject (target, "AVFoundation.framework", true);
//			pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libsqlite3.tbd", "Frameworks/libsqlite3.tbd", PBXSourceTree.Sdk));
//			pbxProject.AddFileToBuild(target, pbxProject.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));



			//CopyAndReplaceDirectory("Assets/Lib/mylib.framework", Path.Combine(path, "Frameworks/mylib.framework"));
            //proj.AddFileToBuild(target, proj.AddFile("Frameworks/mylib.framework", "Frameworks/mylib.framework", PBXSourceTree.Source));

            // ファイルを追加
           // var fileName = "my_file.xml";
            //var filePath = Path.Combine("Assets/Lib", fileName);
            //File.Copy(filePath, Path.Combine(path, fileName));
            //proj.AddFileToBuild(target, proj.AddFile(fileName, fileName, PBXSourceTree.Source));

			// △△△△△

			pbxProject.WriteToFile (projectPath);
		}
	}
}
