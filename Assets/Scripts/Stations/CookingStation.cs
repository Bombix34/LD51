using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingStation", menuName = "ScriptableObjects/Station/CookingStation", order = 1)]
public class CookingStation : StationScriptableObject
{
    [field: SerializeField]
    public float CookingDuration { get; private set; } = 2f;
    private bool CookingOngoing { get; set; }
    public override bool CanAddIngredient()
    {
        return Ingredients.Count < 3 && !CookingOngoing;
    }

    public override bool CanRemoveIngredient()
    {
        return !CookingOngoing;
    }

    public override void Cook(Vector2 spawnPosition)
    {
        if (CookingOngoing)
        {
            return;
        }
        ScoreBoard.Instance.StartCoroutine(CookWithDelay(spawnPosition));
    }
    IEnumerator CookWithDelay(Vector2 spawnPosition)
    {
        var startTime = Time.time;

        while (Time.time < startTime + CookingDuration)
        {
            yield return new WaitForEndOfFrame();
        }
        base.Cook(spawnPosition);
    }
}
