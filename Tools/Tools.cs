using Godot;
using System;

public partial class Tools : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
