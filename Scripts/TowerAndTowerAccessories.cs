using Godot;
using System;
using System.Collections.Generic;

public partial class TowerAndTowerAccessories : Node2D
{
	public float time_factor = 1f;
	// Called when the node enters the scene tree for the first time.

	
	[Export]
	public float rotation_speed { get; set; } = 1.5f;
	[Export]
	public float range = 500f;
	[Export]
	public float damage = 1f; 
	[Export]
	public float attack_rate = 1f; 
	[Export]
	public float projectile_speed = 1f; 
	[Export]
	public float projectile_lifetime = 300f; 
	[Export]
	public int cost  { get; set; } = 10; 


	BuildingManager _BuildingManager;
	TowerBase _TowerBase;
	Tower _Tower;

	public void Initialize(stats s){
		rotation_speed = s.rotation_speed;
		range = s.range;
		damage = s.damage;
		attack_rate = s.attack_rate;
		cost = s.cost;
		projectile_speed = s.projectile_speed;
	}

	public override void _Ready()
	{
		// Set starting location
		//Position = new Vector2(400,400);
		_BuildingManager = GetParent().GetNode<BuildingManager>("BuildingManager");
		_BuildingManager.AddTower(this);
		_TowerBase = GetNode<TowerBase>("Tower/TowerBase");
		_Tower = GetNode<Tower>("Tower");

	// Select on click
		// _Tower.InputEvent += (Node n, InputEvent @e, long l) => 	CheckIfSelect(@e);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void SetTimeFactor(float t){
		time_factor = t;
	}

	public void SetTowerBaseSprite(SpriteFrames s){
		_TowerBase.SpriteFrames = s;
	}



	public void Select(){
		GD.Print($"SELECTED TOWER: D: {damage}");
	}

	public struct stats{
		public float rotation_speed { get; set; } = 1.5f;
		public float range = 500f;
		public float damage = 1f; 
		public float attack_rate = 1f; 
		public float projectile_speed = 1f; 
		public float projectile_lifetime = 100f; 
	public int cost  { get; set; } = 10; 
		public stats(float _rotation_speed, float _range, float _damage, float _attack_rate, int _cost, float _projectile_speed, float _projectile_lifetime){
			rotation_speed = rotation_speed;
			range = _range;
			damage = _damage;
			attack_rate = _attack_rate;
			cost = _cost;
			projectile_speed = _projectile_speed;
			projectile_lifetime = _projectile_lifetime;

		}
	}
}
