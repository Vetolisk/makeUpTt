// Добавьте в папку Assets/Editor скрипт для оптимизации сборки
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildOptimizer : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Удаляем неиспользуемые слои
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;

        // Включаем оптимизацию
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.stripUnusedMeshComponents = true;
    }
}