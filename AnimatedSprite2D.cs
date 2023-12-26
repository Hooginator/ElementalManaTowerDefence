using Godot;
using System;

public partial class AnimatedSprite2D : Godot.AnimatedSprite2D
{
	
	const int SPEED = 500;
	[Export]
	public float RotationSpeed { get; set; } = 1.5f;

	private float _rotationDirection;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{/*
		var vec = GetViewport().GetMousePosition() - Transform.Origin;
		vec = SetLengthVector2(vec, delta * SPEED);
		Transform = new Transform(vec);*/
		
	}

	public void GetInput()
	{
		//_rotationDirection = Input.GetAxis("left", "right");
		//Velocity = Transform.X * Input.GetAxis("down", "up") * SPEED;
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = Position + new Vector2(10,50);		//GetInput();
		//Rotation += _rotationDirection * RotationSpeed * (float)delta;
		//MoveAndSlide();
	}


	
	// Extra stuff, should probably go elsewhere
	public Vector2 SetLengthVector2(Vector2 v, double l = 1){
		double len = Mathf.Abs(v.Length());
		if(len == 0){
			return v;
		}
		// Normalize vector and set new length
		double multiplier = len / l;
		v[0] *= (float) multiplier;
		v[1] *= (float) multiplier;
		return v;
	}
	
/*
func _process(delta):
	var vec = GetViewport().GetMousePosition() - Transform.Origin # getting the vector from self to the mouse
	vec = vec.normalized() * delta * SPEED # normalize it and multiply by time and speed
	position += vec # move by that vector
*/	
	
}
