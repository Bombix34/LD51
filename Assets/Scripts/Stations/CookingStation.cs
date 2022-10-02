using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingStation", menuName = "ScriptableObjects/Station/CookingStation", order = 1)]
public class CookingStation : StationScriptableObject
{
    private Coroutine cookWithDelayCoroutine;

    [field: SerializeField]
    public float CookingDuration { get; private set; } = 2f;
    public bool CookingOngoing { get; private set; }
    public float CookingTimePassed { get; private set; }
    
    public override void OnEnable()
    {
        base.OnEnable();
        CookingOngoing = false;
    }
    
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
        cookWithDelayCoroutine = ScoreBoard.Instance.StartCoroutine(CookWithDelay(spawnPosition));
    }
    IEnumerator CookWithDelay(Vector2 spawnPosition)
    {
        CookingOngoing = true;
        var startTime = Time.time;
        while (Time.time < startTime + CookingDuration)
        {
            CookingTimePassed = Time.time - startTime;
            yield return new WaitForEndOfFrame();
        }
        base.Cook(spawnPosition);
        CookingOngoing = false;
    }

    public override void HandleNewTurn()
    {
        CookingOngoing = false;
        if (cookWithDelayCoroutine != null)
        {
            ScoreBoard.Instance.StopCoroutine(cookWithDelayCoroutine);
        }
        base.HandleNewTurn();
    }
}
