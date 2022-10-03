using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class PreprocessBuild : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        PlayerPrefs.DeleteAll();
    }
}
