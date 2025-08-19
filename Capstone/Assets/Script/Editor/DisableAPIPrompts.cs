using UnityEngine;
using UnityEditor;

public class DisableAPIPrompts
{
    [MenuItem("Tools/Disable API Update Prompts")]
    public static void DisablePrompts()
    {
        // 禁用脚本更新同意对话框
        EditorPrefs.SetBool("ScriptUpdaterConsent", true);
        
        // 禁用API更新检查
        EditorPrefs.SetBool("APIUpdaterConsent", true);
        
        // 禁用自动刷新
        EditorPrefs.SetBool("kAutoRefresh", false);
        
        Debug.Log("API Update prompts have been disabled!");
        Debug.Log("You may need to restart Unity for changes to take effect.");
    }
    
    [MenuItem("Tools/Check API Settings")]
    public static void CheckAPISettings()
    {
        Debug.Log("Current API Settings:");
        Debug.Log("ScriptUpdaterConsent: " + EditorPrefs.GetBool("ScriptUpdaterConsent"));
        Debug.Log("APIUpdaterConsent: " + EditorPrefs.GetBool("APIUpdaterConsent"));
        Debug.Log("kAutoRefresh: " + EditorPrefs.GetBool("kAutoRefresh"));
    }
}
