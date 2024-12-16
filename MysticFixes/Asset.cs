using UnityEngine;
using System.IO;

namespace HaloBrr
{
    public static class Asset
    {
        public static AssetBundle mainBundle;

        public static string AssetBundlePath => Path.Combine(Path.GetDirectoryName(HaloBrrPlugin.PInfo.Location), "smg");

        public static void Init()
        {
            mainBundle = AssetBundle.LoadFromFile(AssetBundlePath);
        }
    }
}