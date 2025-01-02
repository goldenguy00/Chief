using System.Security.Permissions;
using System.Security;
using BepInEx.Logging;
using HunkMod.Modules.Weapons;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace ChiefMod
{
    internal static class Log
    {
        internal static ManualLogSource _logSource;

        internal static void Init(ManualLogSource logSource)
        {
            _logSource = logSource;
        }

        internal static void Debug(object data) => _logSource.LogDebug(data);
        internal static void Error(object data) => _logSource.LogError(data);
        internal static void ErrorAsset(string assetName) =>
            Log.Error($"failed to load asset, {assetName}, because it does not exist in any asset bundle");
        internal static void ErrorAssetBundle(string assetName) =>
            Log.Error($"failed to load asset bundle {assetName}");
        internal static void ErrorTargetMethod(string typeName) =>
            Log.Error($"failed to find target property {nameof(BaseWeapon.modelPrefab)} because it does not exist in the class {typeName}");
        internal static void Fatal(object data) => _logSource.LogFatal(data);
        internal static void Info(object data) => _logSource.LogInfo(data);
        internal static void Message(object data) => _logSource.LogMessage(data);
        internal static void Warning(object data) => _logSource.LogWarning(data);
    }
}
