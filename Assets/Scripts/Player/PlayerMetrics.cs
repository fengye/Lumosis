using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMetrics : MonoBehaviour {

	public int completedPlanets = 0;
	public int completedBossPlanets = 0;
    public float boostLeft = 0f;

    public int performanceReviewInterval = 20;
    HashSet<GameObject> associatedObjects = new HashSet<GameObject>();

    public static PlayerMetrics instance
    {
    	get {
    		return _instance;
    	}
    }

    public static PlayerMetrics _instance;

    struct PlayerPerformance
    {
    	public float timestamp;
    	public int point;

    	public PlayerPerformance(float time, int point)
    	{
    		this.timestamp = time;
    		this.point = point;
    	}
    }

    Queue<PlayerPerformance> playerPerformanceRecord = new Queue<PlayerPerformance>();
    float lastAvgPoint;

	// Use this for initialization
	void Start () {
		
		_instance = this;

		lastAvgPoint = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		while(playerPerformanceRecord.Count > 0)
		{
			PlayerPerformance performance = playerPerformanceRecord.Peek();
			if (Time.time - performance.timestamp > performanceReviewInterval) 
			{
				playerPerformanceRecord.Dequeue();
			}
			else
			{
				break;
			}
		}

		int allPoints = 0;
		foreach(PlayerPerformance perf in playerPerformanceRecord)
		{
			allPoints += perf.point;
		}

		float avgPoint = (float)allPoints / (float)performanceReviewInterval;
		lastAvgPoint = avgPoint * 0.25f + lastAvgPoint * 0.75f;
		lastAvgPoint = Mathf.Clamp(lastAvgPoint, -1.0f, 1.0f);
	}

	public void MarkPoint(int point, GameObject gameObject)
	{
		if (gameObject != null)
		{
			if (!associatedObjects.Contains(gameObject))
			{
				associatedObjects.Add(gameObject);
				playerPerformanceRecord.Enqueue(new PlayerPerformance(Time.time, point));
			}
		}
		else
		{
			playerPerformanceRecord.Enqueue(new PlayerPerformance(Time.time, point));
		}
	}

	// [-1, 1] 0 means doing average
	public float IsDoingGreat()
	{
		

		
		PlayerPhysics physics = GetComponent<PlayerPhysics>();
		float speedScore = Mathf.Min(0, ((physics.currentSpeed - 10.0f) / 5.0f));

		float combinedScore = lastAvgPoint + speedScore;

		return Mathf.Clamp(combinedScore, -1.0f, 1.0f);
	}
}
