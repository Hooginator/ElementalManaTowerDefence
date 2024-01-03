using Godot;
using System;

public partial class Enemy : AnimatedSprite2D
{
	[Export ]
	public float animation_speed = 0.5f;
	
	private PathFollow2D _path;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_path = GetParent<PathFollow2D>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_path.ProgressRatio = _path.ProgressRatio + (float) delta*animation_speed;
	}
}
