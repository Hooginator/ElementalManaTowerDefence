using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class EnemyAndEnemyAccessories : Node2D
{
	private int _max_health = 1;
	private int _current_health ;
	// Path taken through map
	private float _speed = 1f;
	private List<Vector2> waypoints = new List<Vector2>(){new Vector2(300, 300), new Vector2(300, 600), new Vector2(600, 600), new Vector2(600, 300)};
	private int _waypoint_index = 0;
	float time_factor = 1f;
	
	[Signal]
	public delegate void CreepReachedEndEventHandler();
	[Signal]
	public delegate void CreepDiedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_current_health =_max_health;
		CreepDied += () => GD.Print("Hello!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 raw = waypoints[_waypoint_index] - Position;
		Vector2 direction = Tools.SetLengthVector2(raw, 1);
		Vector2 v_delta =  direction * _speed * time_factor;

		Position += v_delta;

		// Next in queue
		if(Mathf.Abs(raw[0]) + Mathf.Abs(raw[1]) < _speed * time_factor *2){
			_waypoint_index = (_waypoint_index +1)%waypoints.Count;
			if(_waypoint_index == 0){
				GD.Print("Reached end point");
				EmitSignal (SignalName.CreepReachedEnd);
				QueueFree();
			}
		}

	}

	public void TakeDamage(int a){
			GD.Print($"Taking {a} damage from {_current_health}");
		_current_health -= a;
		if(_current_health < 0){
			GD.Print("DEAD");
				EmitSignal (SignalName.CreepDied);
			QueueFree();
		}
	}

	public void SetLevel(int l){
		_max_health = 1 + l;
		_current_health = _max_health;
	}

	
	public void SetTimeFactor(float t){
		time_factor = t;
	}

/*
private void _on_body_entered(Node2D body)
{
	// Replace with function body.
			GD.Print($"COLLISION");
	TakeDamage(1);
}
*/

}


