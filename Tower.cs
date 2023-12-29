
using Godot;
using System;

public partial class Tower : Area2D
{
		[Export]
	public PackedScene boolet { get; set; }


	private TowerAndTowerAccessories _root;
	private TowerAnimation _animations;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_root = GetParent<TowerAndTowerAccessories>();
		_animations = GetNode<TowerAnimation>("TowerAnimation");
		boolet = GD.Load<PackedScene>("res://Projectile.tscn");
	}
	public Vector2 _target = new Vector2(0, 0);
	
	[Signal]
	public delegate void TargetChangedEventHandler(float target);
	int i = 0;
	private float _rotationTarget = 1;
	private float _rotationDirection = 0;
	private float _range = 500f; // I don't think its pixels..
	[Export]
	public float RotationSpeed { get; set; } = 1.5f;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print($"Processing frame : {i}"); i++;

		// Fire a projectile
		if(i%80 == 0){
			var projectile = boolet.Instantiate();
			_root.AddChild(projectile);
			var projectile2 = projectile.GetNode<Projectile>(".");
			projectile2.Position = new Vector2(0,0);
			projectile2.SetDirection(new Vector2(Mathf.Sin(_animations.Rotation), -Mathf.Cos(_animations.Rotation)));
			projectile2.SetLifetime(100);
			projectile2.SetSpeed(5f);
		}
		i++;
		// var vec = GetViewport().GetMousePosition();
		var vec = GetNode<WaveManager>("/root/Main/WaveManager").GetClosest(_root.Position);
		
		var diff = vec - _root.Position;
		GD.Print("l:: " + diff.Length());
		// Update target
		if(vec != _target && diff.Length() <= _range){
			UpdateTarget(vec);
		}
		
		// Rotatte tower
		_animations.Rotation += _rotationDirection * Mathf.Min(RotationSpeed * (float)delta, Mathf.Abs(_animations.Rotation - _rotationTarget));


	}

	public void UpdateTarget(Vector2 v){
		_target = v;
		v =  SetLengthVector2(v- _root.Position);
		var angle = GetAngleFromVector2(v);
		SetRotationDirection(angle, _animations.Rotation);
		SetRotationTarget(angle);
		EmitSignal(SignalName.TargetChanged, angle);
	}

	
	public void SetRotationTarget(float t){
		_rotationTarget = t;
	}

	public void SetRotationDirection(float a1, float a2){
		float diff = a2-a1; 
		if(diff==0){_rotationDirection = 0; return;}
		if(Mathf.Sin(diff) < 0 ){_rotationDirection = 1; return;}
		_rotationDirection = -1;
	}


		// TOOLS
	public static Vector2 SetLengthVector2(Vector2 v, double l = 1){
		double len = Mathf.Abs(v.Length());
		if(len == 0){
			return v;
		}
		// Normalize vector and set new length
		double multiplier = l / len;
		v[0] *= (float) multiplier;
		v[1] *= (float) multiplier;
		return v;
	}


	public static float GetAngleFromVector2(Vector2 vec){
		
		return Mathf.Atan2(vec[0], -vec[1]);
	}

}
