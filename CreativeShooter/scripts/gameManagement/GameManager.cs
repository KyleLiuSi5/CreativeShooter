using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{		
	public int Points { get; private set; }
	public float TimeScale { get; private set; }
	public bool Paused { get; set; } 
	public bool CanMove=true;
	public CharacterBehavior Player { get; set; }
    public static bool spawned = false;
    private float _savedTimeScale;


    public void Reset()
	{
		Points = 0;
		TimeScale = 1f;
		Paused = false;
		CanMove=false;
	}	
		
	public void AddPoints(int pointsToAdd)
	{
		Points += pointsToAdd;
	}

	public void SetPoints(int points)
	{
		Points = points;
	}

	public void SetTimeScale(float newTimeScale)
	{
		_savedTimeScale = Time.timeScale;
		Time.timeScale = newTimeScale;
	}
	
	public void ResetTimeScale()
	{
		Time.timeScale = _savedTimeScale;
	}

}
