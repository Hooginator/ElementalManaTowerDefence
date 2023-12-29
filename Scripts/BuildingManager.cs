using Godot;
using System;
using System.Collections.Generic;

public partial class BuildingManager : Node2D
{
	public PackedScene _tower { get; set; }

	private List<TowerAndTowerAccessories> tower_list = new List<TowerAndTowerAccessories>();
	int cost = 10;
	
	[Export]
	private Main _root;
	bool just_clicked = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_root = GetParent<Main>();
		_tower = GD.Load<PackedScene>("res://Tower.tscn");

		// Initialize any existing towers... 
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if (Input.IsActionPressed("Confirm1") && _root.GetGold() >= cost)
		{
			// Build
			if(!just_clicked)
			{		
				_root.SpendGold(cost);	
			var just_built = _tower.Instantiate();
			_root.AddChild(just_built);
			var tower = just_built.GetNode<TowerAndTowerAccessories>(".");
			tower.Position = GetViewport().GetMousePosition();
			tower.SetTimeFactor(_root.time_factor);
			just_clicked = true;
			}
		}else{
			just_clicked = false;
		}
	}

	public void AddTower(TowerAndTowerAccessories tw){
			GD.Print("Adding tower: ");
			// Should be _root :/
			GetParent<Main>().TimeFactorUpdate += (float t) => tw.SetTimeFactor(t);
			tower_list.Add(tw);
	}
	
	public void DestroyAll(){
		foreach(var t in tower_list){
			t.QueueFree();
		}
		tower_list.Clear();
	}
}
