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
	private RichTextLabel _HighScoreCount;
	private RichTextLabel _SpeedCount;
	private RichTextLabel _WaveTypeDetails;
	private RichTextLabel _SelectedTowerDetails;
	private Button _Sell;
	private Button _Upgrade;
	private GameOverMenu _GameOverMenu;

	private Button _SpeedUp;
	private Button _SlowDown;
	private MouseCursor _MouseCursor;

	[Signal]
	public delegate void ResetEventHandler();
	[Signal]
	public delegate void QuitEventHandler();

	TowerAndTowerAccessories selected_tower;

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

		_HighScoreCount = GetNode<RichTextLabel>("HighScoreCount");
		_Main.HighScoreUpdated += (int s) => UpdateHighScore(s);
		_Main.NewHighScore += (int s) => GD.Print($"New High Score::  {s}");

		_ScoreCount = GetNode<RichTextLabel>("ScoreCount");
		_Main.ScoreUpdated += (int s) => UpdateScore(s);

		_SpeedCount = GetNode<RichTextLabel>("SpeedCount");
		_Main.TimeFactorUpdate += (float s) => UpdateSpeed(s);

		_WaveTypeDetails = GetNode<RichTextLabel>("WaveTypeDetails");
		_WaveManager.WaveStarted += () => UpdateWaveType();
		_SelectedTowerDetails = GetNode<RichTextLabel>("Selected/SelectedDetails");
		_Sell = GetNode<Button>("Selected/Sell");
		_Sell.ButtonDown += () => SellSelectedTower();
		_Upgrade = GetNode<Button>("Selected/Upgrade");
		_Upgrade.ButtonDown += () => UpgradeSelectedTower();

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
		_BuildingManager.FailedBuild += () => CancelSelectedTower();

		_BuildingManager.SuccessfulBuild += () => _MouseCursor.BaseMouse();
		_BuildingManager.SuccessfulBuild += () => CancelSelectedTower();

		_BuildingManager.CancelBuild += () => _MouseCursor.BaseMouse();
		_BuildingManager.CancelBuild += () => CancelSelectedTower();

		_BuildingManager.SelectBuild += (SpriteFrames s) => _MouseCursor.TowerMouse(s);
		_BuildingManager.SelectBuild += (SpriteFrames s) => CancelSelectedTower();

		_BuildingManager.DeselectTower += () => CancelSelectedTower();
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
	public void UpdateHighScore(int s){
		_HighScoreCount.Text = ""+s;
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

	public void SetSelectedTower(TowerAndTowerAccessories t){
		GD.Print("Gonna UI some TOWER SELECTION");
		_SelectedTowerDetails.Text = t.element.ToString() +" " + t.level;
		selected_tower = t;
		_Sell.Visible = true;
		_Upgrade.Visible = true;
	}

	public void CancelSelectedTower(){
		GD.Print("Gonna CANCALELLELEL UI some TOWER SELECTION");
		_SelectedTowerDetails.Text = "";

		selected_tower = null;
		_Sell.Visible = false;
		_Upgrade.Visible = false;
	}

	public void SellSelectedTower(){
		_Main.SellTower(selected_tower);
		CancelSelectedTower();
	}

	public void UpgradeSelectedTower(){
		_Main.UpgradeTower(selected_tower);
		SetSelectedTower(selected_tower);
	}
}
