using Godot;
using System;

public partial class TowerAndTowerAccessories : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set starting location
		Position = new Vector2(400,400);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
