using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public partial class EnemyAndEnemyAccessories : Node2D
{
	public String name = "No Name Given";
	public int max_health = 1;
	public int current_health;
	// Path taken through map
	public float speed = 1f;
	public int damage = 1;
	public int gold = 1;
	public int score = 1;
	private List<Vector2> waypoints = new List<Vector2>(){new Vector2(0, 0), new Vector2(300, 300), new Vector2(300, 600), new Vector2(600, 600), new Vector2(600, 300), new Vector2(900,300), new Vector2(1200,300)};

	private int _waypoint_index = 0;

	// Animation
	float rotation_speed = 1.1f;
	int rotation_direction = 1;
	float time_factor = 1f;
	
	[Signal]
	public delegate void HealthUpdatedEventHandler(int health, int max_health);
	[Signal]
	public delegate void CreepReachedEndEventHandler(EnemyAndEnemyAccessories e);
	[Signal]
	public delegate void CreepDiedEventHandler(EnemyAndEnemyAccessories e);

	HealthBar _Healthbar;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		current_health = max_health;
		_Healthbar = GetNode<HealthBar>("Enemy/HealthBar");
		HealthUpdated += (c, m) => _Healthbar.SetHealth(c, m);
		EmitSignal(SignalName.HealthUpdated, current_health, max_health);
		CreepDied += (EnemyAndEnemyAccessories e) => GD.Print("Hello!");
	}

	public void Initialize(int w, stats s, SpriteFrames sf){
		name = s.name;
		max_health = (int) (s.max_health * Mathf.Sqrt(w));
		current_health = max_health;
		speed = s.speed;
		damage = s.damage;
		gold = (int)(s.gold * Mathf.Sqrt(w));
		score = (int)(s.score * Mathf.Sqrt(w));
		GetNode<AnimatedSprite2D>("Enemy").SpriteFrames = sf;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 raw = waypoints[_waypoint_index] - Position;
		Vector2 direction = Tools.SetLengthVector2(raw, 1);
		Vector2 v_delta =  direction * speed * time_factor;

		Position += v_delta;

		// Next in queue
		if(Mathf.Abs(raw[0]) + Mathf.Abs(raw[1]) < speed * time_factor *2){
			_waypoint_index = (_waypoint_index +1)%waypoints.Count;
			if(_waypoint_index == 0){
				//GD.Print("Reached end point");
				EmitSignal (SignalName.CreepReachedEnd, this);
				QueueFree();
			}
		}

		// Animate

	}

	public void TakeDamage(int a){
		current_health -= a;
		EmitSignal(SignalName.HealthUpdated, current_health, max_health);
		if(current_health <= 0){
			//GD.Print("DEAD");
				EmitSignal (SignalName.CreepDied, this);
			QueueFree();
		}
	}

	public void SetLevel(int l){
		max_health = 1 + l;
		current_health = max_health;
		EmitSignal(SignalName.HealthUpdated, current_health, max_health);
	}

	
	public void SetTimeFactor(float t){
		time_factor = t;
	}


	public struct stats{
		public String name = "Generic Enemy Name";
		public int max_health { get; set; } = 1;
		public float speed = 1.5f;
		public int damage = 1; 
		public int spawn_rate = 150;
		public int spawn_total = 20;
		public int gold = 2;
		public int score = 2;
	public int cost  { get; set; } = 10; 
		public stats(String _name, int  _max_health, float _speed, int _damage, int _spawn_rate , int _spawn_total, int _gold, int _score){
			name = _name;
			max_health = _max_health;
			speed = _speed;
			damage = _damage;
			spawn_rate = _spawn_rate;
			spawn_total = _spawn_total;
			gold = _gold;
			score = _score;
		}
	}

}


