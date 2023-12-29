using Godot;
using System;

public partial class TowerAndTowerAccessories : Node2D
{
	public float time_factor = 1f;
	// Called when the node enters the scene tree for the first time.

	BuildingManager _BuildingManager;

	public override void _Ready()
	{
		// Set starting location
		//Position = new Vector2(400,400);
		_BuildingManager = GetParent().GetNode<BuildingManager>("BuildingManager");
		_BuildingManager.AddTower(this);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetTimeFactor(float t){
		time_factor = t;
	}
}
