﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class FileHelper
{

    public static void GetAllFileFullPath(string rootPath, ref List<string> prefabsFullPaths, string formStr)
    {
        string[] files = Directory.GetFiles(rootPath);
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].EndsWith(formStr))
                {
                    prefabsFullPaths.Add(files[i].Replace("/", "\\"));
                }

            }

        }

        string[] directories = Directory.GetDirectories(rootPath);
        if (directories != null)
        {
            for (int i = 0; i < directories.Length; i++)
            {
                GetAllFileFullPath(directories[i], ref prefabsFullPaths, formStr);
            }

        }

    }

    public Dictionary<string, List<string>> GetAllPrefabsFullPath()
    {
        Dictionary<string, List<string>> prefabsFullPaths = new Dictionary<string, List<string>>();




        return prefabsFullPaths;
    }

    public static byte[] ReadBytes(string fullPath)
    {
        FileStream fsSource = File.OpenRead(fullPath);
        byte[] bytes = new byte[fsSource.Length];
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
            if (n == 0)
                break;
            numBytesRead += n;
            numBytesToRead -= n;
        }
        fsSource.Close();
        return bytes;
    }
}


