using Godot;
using System;

public partial class CentralOrb : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	int i = 0;

	float _speed = 5f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		i ++;
		int dir = 1;
		if( (i /100)%2 == 0 ){
			dir = -1;
		}
		
			Position = new Vector2(Position.X, Position.Y+ dir*_speed*(float)delta);

	}
}
