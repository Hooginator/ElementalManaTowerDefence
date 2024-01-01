using Godot;
using System;

public partial class GameUserInterface : Control
{
	private Main _Main;
	private BuildingManager _BuildingManager;
	private RichTextLabel _LivesCount;
	private RichTextLabel _GoldCount;
	private RichTextLabel _WaveCount;
	private GameOverMenu _GameOverMenu;
	private MouseCursor _MouseCursor;

	[Signal]
	public delegate void ResetEventHandler();
	[Signal]
	public delegate void QuitEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_Main = GetParent<Main>();
		_BuildingManager = _Main.GetNode<BuildingManager>("BuildingManager");
		// Life & Gold UI
		_LivesCount = GetNode<RichTextLabel>("LivesCount");
		_Main.LivesUpdated += (int l) => UpdateLives(l);
		_GoldCount = GetNode<RichTextLabel>("GoldCount");
		_Main.GoldUpdated += (int g) => UpdateGold(g);
		_WaveCount = GetNode<RichTextLabel>("WaveCount");
		_Main.WaveUpdated += (int w) => UpdateWave(w);

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
	public void GameOverMenuVisible(){
		_GameOverMenu.Visible = true;
	}
}
