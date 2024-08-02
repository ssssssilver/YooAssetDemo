using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AppSetting", menuName = "ScriptableObject/AppSettingScriptableAsset", order = 1)]
public class AppSettingScriptableAsset : ScriptableObject
{
    public string appVersion;
    public string hostServerIP;
}