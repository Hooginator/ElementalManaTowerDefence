using Godot;
using System;

public partial class MouseCursor : AnimatedSprite2D
{
	SpriteFrames error_cursor;
	SpriteFrames base_cursor;
	int frames_since_change = 0;
	int min_frames_since_change = 30;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base_cursor = GD.Load<SpriteFrames>("res://Images/MouseGood.tres");
		error_cursor = GD.Load<SpriteFrames>("res://Images/MouseBad.tres");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		frames_since_change ++;
		Position = GetViewport().GetMousePosition();
	}

	public void ErrorMouse(){
		if(frames_since_change > min_frames_since_change){
			SpriteFrames = error_cursor;
			frames_since_change = 0;
		}
	}
	
	public void BaseMouse(){
		
		if(frames_since_change > min_frames_since_change){
		SpriteFrames = base_cursor;
			frames_since_change = 0;
		}
	}

	public void TowerMouse(SpriteFrames s){
		
		if(frames_since_change > min_frames_since_change){
		SpriteFrames = s;
			frames_since_change = 0;
		}
	}
}
