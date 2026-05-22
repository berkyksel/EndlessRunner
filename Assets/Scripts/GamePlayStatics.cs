using UnityEngine;

public static class GamePlayStatics 
{
    static GameMode gameMode;
   public static bool IsPositionOccupied(Vector3 position, Vector3 DetectionHalfExtend, string OccupationOnChecking)
    {
        Collider[] cols = Physics.OverlapBox(position, DetectionHalfExtend);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == OccupationOnChecking)
            {
                return true;
            }
        }
        return false;
    } 

    public static GameMode GetGameMode()
    {
        if (gameMode == null)
        {
            gameMode = GameObject.FindAnyObjectByType < GameMode>();
        }
        return gameMode;
    }
}
