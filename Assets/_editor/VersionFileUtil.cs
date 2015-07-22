using UnityEngine;
using System.Collections;
using UnityEditor;
using LitJson;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LitJson;
public class VersionFileUtil
{
    public static MD5 md5 = MD5.Create();
    [MenuItem("Custom Editor/Create Version File")]
    public static void CreateVersionFile()
    {
        List<string> allAssetBundleFullPath = new List<string>();
        List<VersionInfo> versionInfoList = new List<VersionInfo>();
        FileHelper.GetAllFileFullPath(Define.StreamingAssetsRoot, ref allAssetBundleFullPath, ".unity3d");
        for (int i = 0; i < allAssetBundleFullPath.Count; i++)
        {
            string fullName = allAssetBundleFullPath[i];
            FileInfo fileInfo = new FileInfo(fullName);
            if (!fileInfo.Exists)
                continue;
            byte[] bytes = FileHelper.ReadBytes(fullName);
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

    public static void ReadVersionFile(Dictionary<string, string> list)
    {
        string versionFile = Define.VersionFileFullPath;
        FileStream fs = null;
        if (!File.Exists(versionFile))
            return;
        else
            fs = new FileStream(versionFile, FileMode.Open, FileAccess.Read);  //打开读文件
        
        StreamReader sr = new StreamReader(fs);
        StringBuilder sb = new StringBuilder();
        string line = "";
        while((line = sr.ReadLine()) != null){
            sb.Append(line);
        }
        sr.Close();
        fs.Close();

        List<VersionInfo> tmp = JsonMapper.ToObject<List<VersionInfo>>(sb.ToString());
        if (tmp == null || tmp.Count == 0)
            return;
        for (int i = 0; i < tmp.Count; i++)
            list.Add(tmp[i].fileName, tmp[i].md5);
    }
}

public struct VersionInfo
{
    public string fileName;
    public string md5;

}


