using Godot;
using System;

public partial class BuildingManager : Node2D
{
	public PackedScene _tower { get; set; }
	
	private Main _root;
	bool just_clicked = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_tower = GD.Load<PackedScene>("res://Tower.tscn");
		_root = GetParent<Main>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if (Input.IsActionPressed("Confirm1"))
		{
			// Build
			if(!just_clicked)
			{			
			var just_built = _tower.Instantiate();
			_root.AddChild(just_built);
			var tower = just_built.GetNode<TowerAndTowerAccessories>(".");
			tower.Position = GetViewport().GetMousePosition();
			just_clicked = true;
			}
		}else{
			just_clicked = false;
		}
	}
}
