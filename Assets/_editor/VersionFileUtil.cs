using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace Utils
{
    public class VersionFileUtil
    {
        public static MD5 md5 = MD5.Create();
        [MenuItem("Custom Editor/Create Version File")]
        public static void CreateVersionFile()
        {
            

            List<string> allAssetBundleFullPath = new List<string>();
            List<VersionInfo> versionInfoList = new List<VersionInfo>();
            FileUtil.GetAllFileFullPath(Define.StreamingAssetsRoot, ref allAssetBundleFullPath,".unity3d");
            for (int i = 0; i < allAssetBundleFullPath.Count; i++)
            {
                string fullName = allAssetBundleFullPath[i];
                FileInfo fileInfo = new FileInfo(fullName);
                if (!fileInfo.Exists)
                    continue;
                byte[] bytes = FileUtil.ReadBytes(fullName);
                byte[] hashBytes = md5.ComputeHash(bytes);
                StringBuilder sBuilder = new StringBuilder();
                for (int j = 0; j < hashBytes.Length; j++)
                {
                    sBuilder.Append(hashBytes[j].ToString("x2"));
                }
                string fileName = fullName.Substring(fullName.IndexOf("StreamingAssets") + "StreamingAssets".Length + 1, fullName.Length - fullName.IndexOf("StreamingAssets") - "StreamingAssets".Length - 1);
                VersionInfo versionInfo = new VersionInfo();
                versionInfo.fileName = fileName;
                versionInfo.md5 = sBuilder.ToString();
                versionInfoList.Add(versionInfo);
            }
            string jsonStr = JsonMapper.ToJson(versionInfoList);
            string versionFile = Define.StreamingAssetsRootByPlatForm + "version.dat";

            FileStream fs = null;
            if (!File.Exists(versionFile))
                 fs = new FileStream(versionFile, FileMode.Create, FileAccess.Write);//创建写入文件
            else
                fs = new FileStream(versionFile, FileMode.Open, FileAccess.Write);  //打开写入文件
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(jsonStr);//开始写入值
            sw.Close();
            fs.Close();
            AssetDatabase.Refresh();
        }
        
    }

    struct VersionInfo
    {
        public string fileName;
        public string md5;
       
    }
}

