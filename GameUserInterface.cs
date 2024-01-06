using Godot;
using System;

public partial class GameUserInterface : Control
{
	private Main _Main;
	private WaveManager _WaveManager;
	private BuildingManager _BuildingManager;
	private RichTextLabel _LivesCount;
	private RichTextLabel _GoldCount;
	private RichTextLabel _WaveCount;
	private RichTextLabel _ScoreCount;
	private RichTextLabel _SpeedCount;
	private RichTextLabel _WaveTypeDetails;
	private GameOverMenu _GameOverMenu;

	private Button _SpeedUp;
	private Button _SlowDown;
	private MouseCursor _MouseCursor;

	[Signal]
	public delegate void ResetEventHandler();
	[Signal]
	public delegate void QuitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Main = GetParent<Main>();
		_WaveManager = _Main.GetNode<WaveManager>("WaveManager");
		_BuildingManager = _Main.GetNode<BuildingManager>("BuildingManager");
		// Life & Gold UI
		_LivesCount = GetNode<RichTextLabel>("LivesCount");
		_Main.LivesUpdated += (int l) => UpdateLives(l);
		_GoldCount = GetNode<RichTextLabel>("GoldCount");
		_Main.GoldUpdated += (int g) => UpdateGold(g);
		_WaveCount = GetNode<RichTextLabel>("WaveCount");
		_Main.WaveUpdated += (int w) => UpdateWave(w);
		_ScoreCount = GetNode<RichTextLabel>("ScoreCount");
		_Main.ScoreUpdated += (int s) => UpdateScore(s);
		_SpeedCount = GetNode<RichTextLabel>("SpeedCount");
		_Main.TimeFactorUpdate += (float s) => UpdateSpeed(s);
		_WaveTypeDetails = GetNode<RichTextLabel>("WaveTypeDetails");
		_WaveManager.WaveStarted += () => UpdateWaveType();

		// Buttons
		_SpeedUp = GetNode<Button>("SpeedUp");
		_SpeedUp.ButtonDown += () => _Main.SpeedUp();
		_SlowDown = GetNode<Button>("SlowDown");
		_SlowDown.ButtonDown += () => _Main.SlowDown();
		

		// Game over menu
		_GameOverMenu = GetNode<GameOverMenu>("GameOverMenu");
		_Main.GameOver += () => GameOverMenuVisible();
		_GameOverMenu.GetNode<Button>("Quit").Pressed += () => EmitSignal(SignalName.Quit);
		_GameOverMenu.GetNode<Button>("Retry").Pressed += () => EmitSignal(SignalName.Reset);
		_GameOverMenu.GetNode<Button>("Retry").Pressed += () => _GameOverMenu.Visible = false;
		_GameOverMenu.Visible = false;

		// Mouse
		_MouseCursor = GetNode<MouseCursor>("MouseCursor");
		_BuildingManager.FailedBuild += () => _MouseCursor.ErrorMouse();
		_BuildingManager.SuccessfulBuild += () => _MouseCursor.BaseMouse();
		_BuildingManager.CancelBuild += () => _MouseCursor.BaseMouse();
		_BuildingManager.SelectBuild += (SpriteFrames s) => _MouseCursor.TowerMouse(s);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void UpdateLives(int l){
		_LivesCount.Text = ""+l;
	}
	public void UpdateGold(int g){
		_GoldCount.Text = ""+g;
	}
	public void UpdateWave(int w){
		_WaveCount.Text = ""+w;
	}
	public void UpdateScore(int s){
		_ScoreCount.Text = ""+s;
	}
	public void UpdateSpeed(float s){
		_SpeedCount.Text = ""+s;
	}
	public void UpdateWaveType(){
		_WaveTypeDetails.Text = ""+_WaveManager.GetNextWaveName();
	}
	public void GameOverMenuVisible(){
		_GameOverMenu.Visible = true;
	}
}
