using UnityEngine;
using UnityEditor;

public class DisableAllAPIPrompts
{
    [MenuItem("Tools/彻底禁用API同步提醒")]
    public static void DisableAllPrompts()
    {
        // 禁用脚本更新同意对话框 - 多个可能的键值
        EditorPrefs.SetBool("ScriptUpdaterConsent", true);
        EditorPrefs.SetBool("ScriptUpdater.Consent", true);
        EditorPrefs.SetBool("ScriptUpdaterConsentGiven", true);
        EditorPrefs.SetBool("ScriptUpdater.ConsentGiven", true);
        EditorPrefs.SetBool("ScriptUpdater_Consent", true);
        EditorPrefs.SetBool("ScriptUpdater_ConsentGiven", true);
        
        // 禁用API更新检查
        EditorPrefs.SetBool("APIUpdaterConsent", true);
        EditorPrefs.SetBool("APIUpdater.Consent", true);
        EditorPrefs.SetBool("APIUpdaterConsentGiven", true);
        EditorPrefs.SetBool("APIUpdater.ConsentGiven", true);
        
        // 禁用自动刷新
        EditorPrefs.SetBool("kAutoRefresh", false);
        EditorPrefs.SetBool("AutoRefresh", false);
        
        // 禁用脚本更新
        EditorPrefs.SetBool("ScriptUpdating", false);
        EditorPrefs.SetBool("ScriptUpdater.Enabled", false);
        EditorPrefs.SetBool("ScriptUpdaterEnabled", false);
        
        // 禁用API更新器
        EditorPrefs.SetBool("APIUpdater", false);
        EditorPrefs.SetBool("APIUpdater.Enabled", false);
        EditorPrefs.SetBool("APIUpdaterEnabled", false);
        
        // 禁用自动导入
        EditorPrefs.SetBool("AutoImport", false);
        EditorPrefs.SetBool("AutoImportAssets", false);
        
        // 禁用脚本重新编译提示
        EditorPrefs.SetBool("ScriptRecompilePrompt", false);
        EditorPrefs.SetBool("ScriptRecompile.Prompt", false);
        
        // 禁用API变更检测
        EditorPrefs.SetBool("APIChecking", false);
        EditorPrefs.SetBool("APICheck.Enabled", false);
        
        // 禁用版本控制提示
        EditorPrefs.SetBool("VersionControlPrompt", false);
        EditorPrefs.SetBool("VersionControl.Prompt", false);
        
        // 禁用包管理器更新提示
        EditorPrefs.SetBool("PackageManagerUpdatePrompt", false);
        EditorPrefs.SetBool("PackageManager.UpdatePrompt", false);
        
        // 禁用编辑器更新提示
        EditorPrefs.SetBool("EditorUpdatePrompt", false);
        EditorPrefs.SetBool("Editor.UpdatePrompt", false);
        
        // 禁用项目设置变更提示
        EditorPrefs.SetBool("ProjectSettingsChangePrompt", false);
        EditorPrefs.SetBool("ProjectSettings.ChangePrompt", false);
        
        // 禁用脚本编译错误提示（谨慎使用）
        // EditorPrefs.SetBool("ScriptCompileErrorPrompt", false);
        
        // 新增：禁用所有可能的更新提示
        EditorPrefs.SetBool("UpdatePrompts", false);
        EditorPrefs.SetBool("ShowUpdatePrompts", false);
        EditorPrefs.SetBool("EnableUpdatePrompts", false);
        EditorPrefs.SetBool("UpdateNotifications", false);
        EditorPrefs.SetBool("ShowUpdateNotifications", false);
        
        // 新增：禁用脚本更新相关的所有设置
        EditorPrefs.SetBool("ScriptUpdater.ShowConsentDialog", false);
        EditorPrefs.SetBool("ScriptUpdater.ShowDialog", false);
        EditorPrefs.SetBool("ScriptUpdater.PromptForConsent", false);
        EditorPrefs.SetBool("ScriptUpdater.AskForConsent", false);
        
        // 新增：设置自动同意所有更新
        EditorPrefs.SetBool("AutoConsentToUpdates", true);
        EditorPrefs.SetBool("AutoConsentToScriptUpdates", true);
        EditorPrefs.SetBool("AutoConsentToAPIUpdates", true);
        
        Debug.Log("=== 所有API同步提醒已彻底禁用 ===");
        Debug.Log("已禁用的功能包括：");
        Debug.Log("- 脚本更新同意对话框（多个键值）");
        Debug.Log("- API更新检查（多个键值）");
        Debug.Log("- 自动刷新");
        Debug.Log("- 脚本更新");
        Debug.Log("- API更新器");
        Debug.Log("- 自动导入");
        Debug.Log("- 脚本重新编译提示");
        Debug.Log("- API变更检测");
        Debug.Log("- 版本控制提示");
        Debug.Log("- 包管理器更新提示");
        Debug.Log("- 编辑器更新提示");
        Debug.Log("- 项目设置变更提示");
        Debug.Log("- 所有更新提示和通知");
        Debug.Log("- 自动同意所有更新");
        Debug.Log("注意：某些设置可能需要重启Unity才能生效");
    }
    
    [MenuItem("Tools/检查当前API设置")]
    public static void CheckAllAPISettings()
    {
        Debug.Log("=== 当前API设置状态 ===");
        Debug.Log($"ScriptUpdaterConsent: {EditorPrefs.GetBool("ScriptUpdaterConsent")}");
        Debug.Log($"ScriptUpdater.Consent: {EditorPrefs.GetBool("ScriptUpdater.Consent")}");
        Debug.Log($"ScriptUpdaterConsentGiven: {EditorPrefs.GetBool("ScriptUpdaterConsentGiven")}");
        Debug.Log($"APIUpdaterConsent: {EditorPrefs.GetBool("APIUpdaterConsent")}");
        Debug.Log($"APIUpdater.Consent: {EditorPrefs.GetBool("APIUpdater.Consent")}");
        Debug.Log($"kAutoRefresh: {EditorPrefs.GetBool("kAutoRefresh")}");
        Debug.Log($"ScriptUpdating: {EditorPrefs.GetBool("ScriptUpdating")}");
        Debug.Log($"ScriptUpdater.Enabled: {EditorPrefs.GetBool("ScriptUpdater.Enabled")}");
        Debug.Log($"APIUpdater: {EditorPrefs.GetBool("APIUpdater")}");
        Debug.Log($"APIUpdater.Enabled: {EditorPrefs.GetBool("APIUpdater.Enabled")}");
        Debug.Log($"AutoImport: {EditorPrefs.GetBool("AutoImport")}");
        Debug.Log($"ScriptRecompilePrompt: {EditorPrefs.GetBool("ScriptRecompilePrompt")}");
        Debug.Log($"APIChecking: {EditorPrefs.GetBool("APIChecking")}");
        Debug.Log($"VersionControlPrompt: {EditorPrefs.GetBool("VersionControlPrompt")}");
        Debug.Log($"PackageManagerUpdatePrompt: {EditorPrefs.GetBool("PackageManagerUpdatePrompt")}");
        Debug.Log($"EditorUpdatePrompt: {EditorPrefs.GetBool("EditorUpdatePrompt")}");
        Debug.Log($"ProjectSettingsChangePrompt: {EditorPrefs.GetBool("ProjectSettingsChangePrompt")}");
        Debug.Log($"AutoConsentToUpdates: {EditorPrefs.GetBool("AutoConsentToUpdates")}");
        Debug.Log($"ScriptUpdater.ShowConsentDialog: {EditorPrefs.GetBool("ScriptUpdater.ShowConsentDialog")}");
    }
    
    [MenuItem("Tools/恢复默认API设置")]
    public static void RestoreDefaultAPISettings()
    {
        // 恢复默认设置 - 删除所有相关键值
        EditorPrefs.DeleteKey("ScriptUpdaterConsent");
        EditorPrefs.DeleteKey("ScriptUpdater.Consent");
        EditorPrefs.DeleteKey("ScriptUpdaterConsentGiven");
        EditorPrefs.DeleteKey("ScriptUpdater.ConsentGiven");
        EditorPrefs.DeleteKey("ScriptUpdater_Consent");
        EditorPrefs.DeleteKey("ScriptUpdater_ConsentGiven");
        EditorPrefs.DeleteKey("APIUpdaterConsent");
        EditorPrefs.DeleteKey("APIUpdater.Consent");
        EditorPrefs.DeleteKey("APIUpdaterConsentGiven");
        EditorPrefs.DeleteKey("APIUpdater.ConsentGiven");
        EditorPrefs.DeleteKey("kAutoRefresh");
        EditorPrefs.DeleteKey("AutoRefresh");
        EditorPrefs.DeleteKey("ScriptUpdating");
        EditorPrefs.DeleteKey("ScriptUpdater.Enabled");
        EditorPrefs.DeleteKey("ScriptUpdaterEnabled");
        EditorPrefs.DeleteKey("APIUpdater");
        EditorPrefs.DeleteKey("APIUpdater.Enabled");
        EditorPrefs.DeleteKey("APIUpdaterEnabled");
        EditorPrefs.DeleteKey("AutoImport");
        EditorPrefs.DeleteKey("AutoImportAssets");
        EditorPrefs.DeleteKey("ScriptRecompilePrompt");
        EditorPrefs.DeleteKey("ScriptRecompile.Prompt");
        EditorPrefs.DeleteKey("APIChecking");
        EditorPrefs.DeleteKey("APICheck.Enabled");
        EditorPrefs.DeleteKey("VersionControlPrompt");
        EditorPrefs.DeleteKey("VersionControl.Prompt");
        EditorPrefs.DeleteKey("PackageManagerUpdatePrompt");
        EditorPrefs.DeleteKey("PackageManager.UpdatePrompt");
        EditorPrefs.DeleteKey("EditorUpdatePrompt");
        EditorPrefs.DeleteKey("Editor.UpdatePrompt");
        EditorPrefs.DeleteKey("ProjectSettingsChangePrompt");
        EditorPrefs.DeleteKey("ProjectSettings.ChangePrompt");
        EditorPrefs.DeleteKey("UpdatePrompts");
        EditorPrefs.DeleteKey("ShowUpdatePrompts");
        EditorPrefs.DeleteKey("EnableUpdatePrompts");
        EditorPrefs.DeleteKey("UpdateNotifications");
        EditorPrefs.DeleteKey("ShowUpdateNotifications");
        EditorPrefs.DeleteKey("ScriptUpdater.ShowConsentDialog");
        EditorPrefs.DeleteKey("ScriptUpdater.ShowDialog");
        EditorPrefs.DeleteKey("ScriptUpdater.PromptForConsent");
        EditorPrefs.DeleteKey("ScriptUpdater.AskForConsent");
        EditorPrefs.DeleteKey("AutoConsentToUpdates");
        EditorPrefs.DeleteKey("AutoConsentToScriptUpdates");
        EditorPrefs.DeleteKey("AutoConsentToAPIUpdates");
        
        Debug.Log("=== API设置已恢复默认 ===");
        Debug.Log("所有自定义API设置已被清除，Unity将使用默认设置");
        Debug.Log("注意：某些设置可能需要重启Unity才能生效");
    }
    
    [MenuItem("Tools/强制重启Unity")]
    public static void ForceRestartUnity()
    {
        Debug.Log("=== 准备重启Unity ===");
        Debug.Log("请手动重启Unity以确保所有设置生效");
        Debug.Log("建议步骤：");
        Debug.Log("1. 保存当前场景");
        Debug.Log("2. 关闭Unity");
        Debug.Log("3. 重新打开项目");
        Debug.Log("4. 检查Console确认设置已生效");
    }
    
    [MenuItem("Tools/立即应用设置（无需重启）")]
    public static void ApplySettingsImmediately()
    {
        // 尝试立即应用设置
        DisableAllPrompts();
        
        // 强制刷新编辑器
        EditorUtility.RequestScriptReload();
        
        Debug.Log("=== 设置已立即应用 ===");
        Debug.Log("已尝试立即应用所有设置，无需重启Unity");
        Debug.Log("如果仍有提示，请重启Unity");
    }
}
