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

	public static float GetDistanceToLine(Vector2 A, Vector2 B, Vector2 E){
		// Need to line segment, not line... 

 
    // vector AB
    Vector2 AB = new Vector2();
    AB[0] = B[0] - A[0];
    AB[1] = B[1] - A[1];
 
    // vector BP
    Vector2 BE = new Vector2();
    BE[0] = E[0] - B[0];
    BE[1] = E[1] - B[1];
 
    // vector AP
    Vector2 AE = new Vector2();
    AE[0] = E[0] - A[0];
    AE[1] = E[1] - A[1];
 
    // Variables to store dot product
    double AB_BE, AB_AE;
 
    // Calculating the dot product
    AB_BE = (AB[0] * BE[0] + AB[1] * BE[1]);
    AB_AE = (AB[0] * AE[0] + AB[1] * AE[1]);
 
    // Minimum distance from
    // point E to the line segment
    float reqAns = 0;
 
    // Case 1
    if (AB_BE > 0) 
    {
 
        // Finding the magnitude
        float y = E[1] - B[1];
        float x = E[0] - B[0];
        reqAns = Mathf.Sqrt(x * x + y * y);
    }
 
    // Case 2
    else if (AB_AE < 0)
    {
        float y = E[1] - A[1];
        float x = E[0] - A[0];
        reqAns = Mathf.Sqrt(x * x + y * y);
    }
 
    // Case 3
    else
    {
 
        // Finding the perpendicular distance
        float x1 = AB[0];
        float y1 = AB[1];
        float x2 = AE[0];
        float y2 = AE[1];
        float mod = Mathf.Sqrt(x1 * x1 + y1 * y1);
        reqAns = Mathf.Abs(x1 * y2 - y1 * x2) / mod;
    }
    return reqAns;
}
 

	public static float GetAngleFromVector2(Vector2 vec){
		
		return Mathf.Atan2(vec[0], -vec[1]);
	}
}
