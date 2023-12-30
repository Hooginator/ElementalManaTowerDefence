
using Godot;
using System;

public partial class Tower : Area2D
{
	#region Variables
		[Export]
	public PackedScene boolet { get; set; }


	private TowerAndTowerAccessories _root;

	private Main _Main;
	private TowerAnimation _animations;

	public Vector2 _target = new Vector2(0, 0);
	
	[Signal]
	public delegate void TargetChangedEventHandler(float target);
	int i = 0;
	private float _rotationTarget = 1;
	private float _rotationDirection = 0;

	#endregion
	#region Initialization
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_root = GetParent<TowerAndTowerAccessories>();
		_animations = GetNode<TowerAnimation>("TowerAnimation");
		boolet = GD.Load<PackedScene>("res://Projectile.tscn");
		_Main = GetNode<Main>("/root/Main");
	}
	#endregion
	#region Process

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// GD.Print($"Processing frame : {i}"); i++;
		
		// var vec = GetViewport().GetMousePosition();
		var vec = GetNode<WaveManager>("/root/Main/WaveManager").GetClosest(_root.Position);
		
		var diff = vec - _root.Position;
		// GD.Print("l:: " + diff.Length());
		// Update target
		if(vec != _target && diff.Length() <= _root.range){
			UpdateTarget(vec);
		}
		
		// Rotatte tower
		_animations.Rotation += _rotationDirection * Mathf.Min(_root.rotation_speed * _root.time_factor * (float)delta, Mathf.Abs(_animations.Rotation - _rotationTarget));


		// Fire a projectile
		if(i*_root.time_factor * _root.attack_rate> 80  &&diff.Length() <= _root.range){
			// Create projectile node
			var projectile = boolet.Instantiate();
			_root.AddChild(projectile);

			// Initialize projectile
			var projectile2 = projectile.GetNode<Projectile>(".");
			projectile2.Position = new Vector2(0,0);
			projectile2.SetDirection(new Vector2(Mathf.Sin(_animations.Rotation), -Mathf.Cos(_animations.Rotation)));
			projectile2.Initialize(lifetime: 200, speed: 2.2f, damage: 1);

			// Manage time
			projectile2.SetTimeFactor(_root.time_factor);
			_Main.TimeFactorUpdate += (float t) => projectile2.SetTimeFactor(t);
			i = 0;
		}
		i++;

	}
	#endregion
	#region TargetManagement

	public void UpdateTarget(Vector2 v){
		// Set Target
		_target = v;

		// Find Angle
		v =  Tools.SetLengthVector2(v- _root.Position);
		var angle = Tools.GetAngleFromVector2(v);
		SetRotationDirection(angle, _animations.Rotation);
		SetRotationTarget(angle);

		// Tell the worls we switched targets
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
	#endregion

}
