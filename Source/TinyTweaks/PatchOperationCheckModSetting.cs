using System;
using System.Reflection;
using System.Xml;
using Verse;

namespace TinyTweaks;

public class PatchOperationCheckModSetting : PatchOperation
{
    private readonly string settingName;

    private readonly Type settingsType;

    protected override bool ApplyWorker(XmlDocument xml)
    {
        if (settingsType == null)
        {
            LogPatchOperationError($"Could not find settings type {settingsType}");
            return false;
        }

        var settingInfo = settingsType.GetField(settingName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (settingInfo != null)
        {
            return (bool)settingInfo.GetValue(null);
        }

        LogPatchOperationError($"{settingName} could not be found");
        return false;
    }

    private void LogPatchOperationError(string message)
    {
        Log.Error($"Error with PatchOperationCheckModSetting in {sourceFile}: {message}");
    }
}