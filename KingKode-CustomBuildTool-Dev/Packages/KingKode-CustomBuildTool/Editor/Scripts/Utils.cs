using System.IO;
using UnityEditor;
using UnityEngine;

namespace KingKode.CustomBuildTool
{
    public static class Utils
    {
        public static void DisableFile(string path)
        {
            if (!File.Exists(path))
                return;

            var fileName = Path.GetFileName(path);
            var isDoted = fileName.StartsWith(".");
            if (isDoted)
                return;

            var newName = "." + fileName;
            var dir = Path.GetDirectoryName(path);
            var dotedPath = Path.Combine(dir, newName);
            if (File.Exists(dotedPath)) 
                File.Delete(dotedPath);
            File.Move(path, dotedPath);

            if (!fileName.EndsWith(".meta"))
            {
                if (!File.Exists(path.GetMetaPath()))
                    return;
                if (File.Exists(dotedPath.GetMetaPath()))
                    File.Delete(dotedPath.GetMetaPath());
                File.Move(path.GetMetaPath(), dotedPath.GetMetaPath());
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string GetMetaPath(this string path)
        {
            return path + ".meta";
        }
        public static void EnableFile(string path)
        {      
            var fileName = Path.GetFileName(path);
            var isDoted = fileName.StartsWith(".");
            if (!File.Exists(path))
            {
                if (!isDoted)
                {
                    fileName = "." + fileName;
                    path = Path.Combine(Path.GetDirectoryName(path), fileName);
                    if (!File.Exists(path))
                        return;
                }
                else
                {
                    return;
                }
            }
            isDoted = fileName.StartsWith(".");
            if (!isDoted)
                return;
            var newName = fileName.TrimStart('.');
            var dir = Path.GetDirectoryName(path);
            var noDotedPath = Path.Combine(dir, newName);
            if (File.Exists(noDotedPath))
                File.Delete(noDotedPath);

            File.Move(path, noDotedPath);
            if (!fileName.EndsWith(".meta"))
            {
                if (!File.Exists(path.GetMetaPath()))
                    return;
                if (File.Exists(noDotedPath.GetMetaPath()))
                    File.Delete(noDotedPath.GetMetaPath());

                File.Move(path.GetMetaPath(), noDotedPath.GetMetaPath());
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        public static void EnableDir(string path)
        {
            var fileName = Path.GetFileName(path);
            var isDoted = fileName.StartsWith(".");
            if (!Directory.Exists(path) || Directory.GetFiles(path).Length==0)
            {
                if (!isDoted)
                {
                    fileName = "." + fileName;
                    path = Path.Combine(Path.GetDirectoryName(path), fileName);
                    if (!Directory.Exists(path) || Directory.GetFiles(path).Length==0)
                        return;
                }
                else
                {
                    return;
                }
            }
            isDoted = fileName.StartsWith(".");
            if (!isDoted)
                return;
            var newName = fileName.TrimStart('.');
            var dir = Path.GetDirectoryName(path);
            var noDotedPath = Path.Combine(dir, newName);
            if (Directory.Exists(noDotedPath))
                Directory.Delete(noDotedPath,true);
            if (Directory.Exists(noDotedPath.GetMetaPath()))
                Directory.Delete(noDotedPath.GetMetaPath(),true);

            Directory.Move(path, noDotedPath);

            if (Directory.Exists(path))
                Directory.Delete(path,true);
            if (!fileName.EndsWith(".meta"))
            {
                if (!File.Exists(path.GetMetaPath()))
                    return;
                if (File.Exists(noDotedPath.GetMetaPath()))
                    File.Delete(noDotedPath.GetMetaPath());
                File.Move(path.GetMetaPath(), noDotedPath.GetMetaPath());
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DisableDir(string path)
        {
            if (!Directory.Exists(path))
                return;

            var fileName = Path.GetFileName(path);
            var isDoted = fileName.StartsWith(".");
            if (isDoted)
                return;

            var newName = "." + fileName;


            var dir = Path.GetDirectoryName(path);
            var dotedPath = Path.Combine(dir, newName);
            if (Directory.Exists(dotedPath))
                Directory.Delete(dotedPath,true);
            if (Directory.Exists(dotedPath.GetMetaPath()))
                Directory.Delete(dotedPath.GetMetaPath(),true);
            Directory.Move(path, dotedPath);

            if (Directory.Exists(path))
                Directory.Delete(path,true);
            if (!fileName.EndsWith(".meta"))
            {
                if (!File.Exists(path.GetMetaPath()))
                    return;
                if (File.Exists(dotedPath.GetMetaPath()))
                    File.Delete(dotedPath.GetMetaPath());
                File.Move(path.GetMetaPath(), dotedPath.GetMetaPath());
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string GetAbsolutePath(string relativePath)
        {
            DirectoryInfo assetsFolder = new DirectoryInfo(Application.dataPath);
            return assetsFolder.Parent.ToString().Replace("\\", "/") + "/" +relativePath;
        }
    }
}
