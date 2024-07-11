using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SolidLib.Utils.AssetLoading
{
    public class BundleUtils
    {
        public static byte[] GetResourceBytes(string filename)
        {
            foreach (var resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (resource.Contains(filename))
                {
                    using (Stream resFilestream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                    {
                        if (resFilestream == null) return null;
                        byte[] ba = new byte[resFilestream.Length];
                        resFilestream.Read(ba, 0, ba.Length);
                        return ba;
                    }
                }
            }
            return null;
        }

        public static AssetBundle LoadBundleFromInternalAssembly(string filename)
        {
            AssetBundle bundle = AssetBundle.LoadFromMemory(GetResourceBytes(filename));
            return bundle;
        }
    }
}
