using Godot;
using System;
using System.Collections.Generic;

public partial class TowerAndTowerAccessories : Node2D
{
	public float time_factor = 1f;
	// Called when the node enters the scene tree for the first time.
	[Signal]
	public delegate void TowerSelectedEventHandler(TowerAndTowerAccessories t);
	[Signal]
	public delegate void SoldEventHandler(TowerAndTowerAccessories t);

	
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
	[Export]
	public int base_cost  { get; set; } = 10; 

	public int level = 1;
	public Element element;


	BuildingManager _BuildingManager;
	AnimatedSprite2D _BaseAnimations;
	AnimatedSprite2D _CentralOrbAnimations;
	Tower _Tower;

	public void Initialize(stats s){
		rotation_speed = s.rotation_speed;
		range = s.range;
		damage = s.damage;
		attack_rate = s.attack_rate;
		base_cost = s.cost;
		cost = s.cost;
		projectile_speed = s.projectile_speed;
		element = s.element;
	}

	public override void _Ready()
	{
		// Set starting location
		//Position = new Vector2(400,400);
		_BuildingManager = GetParent().GetNode<BuildingManager>("BuildingManager");
		_BuildingManager.AddTower(this);
		_BaseAnimations = GetNode<AnimatedSprite2D>("Tower/Base/ElementalBase");
		_CentralOrbAnimations = GetNode<AnimatedSprite2D>("Tower/CentralOrb/ElementalOrb");
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

	public void SetTowerBaseSprite(SpriteFrames b, SpriteFrames o){
		_BaseAnimations.SpriteFrames = b;
		_CentralOrbAnimations.SpriteFrames = o;
	}



	public void Select(){
		GD.Print($"SELECTED TOWER: D: {damage}");
		EmitSignal(SignalName.TowerSelected, this);
	}

	public void LevelUp(){
		// We paid for it elsewhere, level up time!!!
		cost += GetUpgradeAmount();
		damage +=1;
		attack_rate *= 1.05f;
		range *= 1.05f;
		rotation_speed *= 1.05f;
		projectile_speed *= 1.05f;
		level +=1;

	}

	public void Sell(){
		// Made money elsewhere, this is just destroyI think
		EmitSignal(SignalName.Sold, this);
		QueueFree();
	}

	public int GetSaleAmount(){
		return (int)( cost * 0.5);
	}
	public int GetUpgradeAmount(){
		return (int)(level * base_cost * 0.5);
	}

	public struct stats{
		public float rotation_speed { get; set; } = 1.5f;
		public float range = 500f;
		public float damage = 1f; 
		public float attack_rate = 1f; 
		public float projectile_speed = 1f; 
		public float projectile_lifetime = 100f; 
		public Element element = Element.Fire;
	public int cost  { get; set; } = 10; 
		public stats(float _rotation_speed, float _range, float _damage, float _attack_rate, int _cost, 
			float _projectile_speed, float _projectile_lifetime, Element _element){
			rotation_speed = rotation_speed;
			range = _range;
			damage = _damage;
			attack_rate = _attack_rate;
			cost = _cost;
			projectile_speed = _projectile_speed;
			projectile_lifetime = _projectile_lifetime;
			element = _element;

		}
	}
}
