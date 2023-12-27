using Godot;
using System;

public partial class AnimatedSprite2D : Godot.AnimatedSprite2D
{
	
	[Export]
	public float RotationSpeed { get; set; } = 1.5f;
	
	[Export]
	public float MovementSpeed { get; set; } = 10.5f;

	private float _rotationTarget = 45;
	private float _rotationDirection = 0;
	int i = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set starting location
		Position = new Vector2(400,400);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		GD.Print($"Processing frame : {i}"); i++;
		var vec = GetViewport().GetMousePosition() - Position;
		vec = SetLengthVector2(vec);
		GD.Print($"vec: {vec} POS: {Position}");
		// Position
		/*
		vec = SetLengthVector2(vec, delta * MovementSpeed);
		Position = Position + vec;
		*/	
		// Rotation

		// Calculate rotation direction
		var angle = GetAngleFromVector2(vec);
		_rotationDirection = GetRotationDirection(angle, Rotation);
		GD.Print($"Rotation: {Rotation} angle: {angle} _rotationDirection: {_rotationDirection}");
		// Rotation = angle;
		Rotation += _rotationDirection * RotationSpeed * (float)delta;
		
		//GetInput();
		//Rotation += _rotationDirection * RotationSpeed * (float)delta;
		//MoveAndSlide();
	}


	
	// Extra stuff, should probably go elsewhere
	public Vector2 SetLengthVector2(Vector2 v, double l = 1){
		double len = Mathf.Abs(v.Length());
		GD.Print($"len: {len} POS: {Position} l: {l}");
		if(len == 0){
			return v;
		}
		// Normalize vector and set new length
		double multiplier = l / len;
		v[0] *= (float) multiplier;
		v[1] *= (float) multiplier;
		return v;
	}

	public int GetRotationDirection(float a1, float a2){
		float diff = a2-a1; 
		if(diff==0){return 0;}
		if(Mathf.Sin(diff) < 0 ){return 1;}


		return -1;

	}
	public float GetAngleFromVector2(Vector2 vec){
		
		return Mathf.Atan2(vec[0], -vec[1]);
	}
	
/*
func _process(delta):
	var vec = GetViewport().GetMousePosition() - Transform.Origin # getting the vector from self to the mouse
	vec = vec.normalized() * delta * SPEED # normalize it and multiply by time and speed
	position += vec # move by that vector
*/	
	
}
