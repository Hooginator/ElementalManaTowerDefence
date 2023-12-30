using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyPath : Line2D
{
	
	private List<Vector2> waypoints = new List<Vector2>(){new Vector2(0, 0), new Vector2(300, 300), new Vector2(300, 600), new Vector2(600, 600), new Vector2(600, 300), new Vector2(900,300), new Vector2(1200,300)};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Points = waypoints.ToArray();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public float GetClosestDistance(Vector2 v){
		float to_return = 9999f;
		int i = 0;
		foreach(var w in waypoints){
			i++;
			if(i == 1){continue;}
			
		float d = Tools.GetDistanceToLine(waypoints[i-2], w, v);
			GD.Print($"Path points:   {v}, {waypoints[i-2]}, {w} :  D ::{d}");
		if(d < to_return){
			to_return = d;
		}

		}
		return to_return;
	}
}
