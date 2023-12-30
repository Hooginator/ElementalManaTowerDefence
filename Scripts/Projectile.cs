using Godot;
using System;
using System.Collections.Generic;

public partial class Projectile : Node2D
{
	#region Variables
	private Vector2 _direction = new Vector2(1,0);
	private float _speed = 2f;
	private float i = 0f;
	private int _lifetime = 200;
	private int _damage = 1;
	private static int latest_id = 0;
	private int _id;
	float time_factor = 1f;
	#endregion
	#region Initialization

	public void Initialize(int lifetime = 200, float speed = 2f, int damage = 1){

	_lifetime = lifetime;
	_speed = speed;
	_damage = damage;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Bullet Alive!!");
		_id = latest_id;
		latest_id +=1;
	}
	#endregion
	#region Process
	private void _on_area_entered(EnemyAndEnemyAccessories e)
{
	// Damage enemy
	e.TakeDamage(_damage);

	// Destroy projectile
	GD.Print("Projectile area entered");
	QueueFree();
}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print($"PROJECTIILE!!! : {_direction} , {_speed}    {Position}"); 

		// Move
		Position += _direction * _speed * time_factor;

		// Expire one lifetime has elapsed
		i+= time_factor;
		if(i > _lifetime){
			QueueFree();
		}

		// Check collision?
		// How are we firing projectiles? 
		// AoE HITBOXES: figure out physics built in
		// 'on a line' homing: maybe later
	}
	#endregion
	public void SetDirection(Vector2 v){
		
		_direction = Tools.SetLengthVector2(v, 1);
	}

	public void SetVelocity(Vector2 v){
		SetDirection(v);
		_speed = v.Length();
	}


	public void SetTimeFactor(float t){
		time_factor = t;
	}
}



