using Godot;
using System;
using System.Collections.Generic;

public partial class Projectile : Node2D
{

	private Vector2 _direction = new Vector2(1,0);
	private float _speed = 2f;
	private int i = 0;
	private int _lifetime = 200;
	private int _damage = 1;
	private static int latest_id = 0;
	private int _id;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GD.Print("Bullet Alive!!");
		_id = latest_id;
					latest_id +=1;
	}
	private void _on_area_entered(EnemyAndEnemyAccessories e)
{
	e.TakeDamage(_damage);
	// Replace with function body.
	GD.Print("Projectile area entered");
			QueueFree();
}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print($"PROJECTIILE!!! : {_direction} , {_speed}    {Position}"); 
		Position += _direction * _speed;

		i++;
		if(i > _lifetime){
			QueueFree();
		}

		// Check collision?
		// How are we firing projectiles? 
		// AoE HITBOXES: figure out physics built in
		// 'on a line' homing: maybe later
	}

	public void SetDirection(Vector2 v){
		
		_direction = Tower.SetLengthVector2(v, 1);
	}

	public void SetVelocity(Vector2 v){
		SetDirection(v);
		_speed = v.Length();
	}

	public void SetLifetime(int l){
		_lifetime = l;
	}
	public void SetSpeed(float s){
		_speed = s;
	}
}



