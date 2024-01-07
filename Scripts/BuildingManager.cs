using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class BuildingManager : Node2D
{
	#region variables

	
	
	[Signal]
	public delegate void FailedBuildEventHandler();
	[Signal]
	public delegate void SuccessfulBuildEventHandler();
	[Signal]
	public delegate void SuccessfulBuildContinueEventHandler();
	[Signal]
	public delegate void CancelBuildEventHandler();
	[Signal]
	public delegate void SelectBuildEventHandler(SpriteFrames s);
	// Tower resources
	public List<PackedScene> _towers = new List<PackedScene>();
	List<TowerAndTowerAccessories> TowerAndTowerAccessoriesList = new List<TowerAndTowerAccessories>();
	List<TowerDetails> initial_towers = new List<TowerDetails>(){
		{new TowerDetails(200, 400, 0)},
		{new TowerDetails(1000, 600, 2)}
	};
	// Tower parameters
	List<int> costs = new List<int>(){  };
	private int _default_cost = 20;
	private float tower_size = 100f;
	List<SpriteFrames> tower_bases = new List<SpriteFrames>();
	List<TowerAndTowerAccessories.stats> tower_stats = new List<TowerAndTowerAccessories.stats>()
	{
		// Basic
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 2f, _range: 500f, _damage: 3f, _attack_rate: 1f, _cost: 10, _projectile_speed: 2f, _projectile_lifetime: 400f)},
		//  Rapid Fire
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 8f, _range: 400f, _damage: 1f, _attack_rate: 4f, _cost: 10, _projectile_speed: 4f, _projectile_lifetime: 200f)},
		// Sniper
		{new TowerAndTowerAccessories.stats(
			_rotation_speed: 1f, _range: 1000f, _damage: 10f, _attack_rate: 0.4f, _cost: 10, _projectile_speed: 16f, _projectile_lifetime: 1000f)}
		
		
	};

	// Tower Management
	private List<TowerAndTowerAccessories> tower_list = new List<TowerAndTowerAccessories>();
	
	
	// Placement Management
	bool just_clicked = false;
	private int _tower_index = -1;

	// Other
	private Main _root;
	private Vector2 building_pos = new Vector2(0,0);
	private EnemyPath _EnemyPath;
	// To get from elsehwewre later
		private List<Vector2> waypoints = new List<Vector2>(){new Vector2(0, 0), new Vector2(300, 300), new Vector2(300, 600), new Vector2(600, 600), new Vector2(600, 300), new Vector2(900,300), new Vector2(1200,300)};

	

	#endregion
	#region Initialization

	public void StartGame(){
		foreach (var t in initial_towers){
			building_pos = new Vector2(t.x, t.y);
			_tower_index = t.type;
			BuildTower(already_paid: true);
		}
		_tower_index = -1;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_root = GetParent<Main>();
		_root.GameStarted += () => StartGame(); 
		_EnemyPath = GetParent().GetNode<EnemyPath>("EnemyPath");
		_towers.Add( GD.Load<PackedScene>("res://Tower.tscn"));

		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower1Sprite.tres"));
		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower2Sprite.tres"));
		tower_bases.Add(GD.Load<SpriteFrames>("res://Images/Tower3Sprite.tres"));
		// _towers.Add( GD.Load<PackedScene>("res://Towers/Tower2.tscn"));

		int i = 0;
		foreach (var t in _towers){
			// Memory leak waiting to happen...
			var temp = t.Instantiate();
			var all_children = temp.GetChildren();
			all_children.Add(temp);
			bool found_it = false;
			foreach (var c in all_children){
				if (c is TowerAndTowerAccessories){
					GD.Print($"FOUND TOWERSEESSES! + {i}");
					TowerAndTowerAccessories tt = (TowerAndTowerAccessories) c;
					TowerAndTowerAccessoriesList.Add(tt);

					break;
				}
			}
			
			i++;
		}

		foreach (var t in tower_stats){
			costs.Add(t.cost);
		}




	}

	private TowerAndTowerAccessories GetTowerAndTowerAccessories(Node n){
		var all_children = n.GetChildren();
			all_children.Add(n);
			foreach (var c in all_children){
				if (c is TowerAndTowerAccessories){
					return (TowerAndTowerAccessories) c;
				}
			}
		return null;
		}
	#endregion
	#region Process
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	int i=0;
	public override void _Process(double delta)
	{
		// do this early, but not in ready... should be in the strart
		if(i==1){

		}
		i++;

		// Select Tower
		if (Input.IsActionPressed("SelectBuild1")){
			_tower_index = 0;
			EmitSignal(SignalName.SelectBuild, tower_bases[_tower_index]);
		}
		if (Input.IsActionPressed("SelectBuild2")){
			_tower_index = 1;
			EmitSignal(SignalName.SelectBuild, tower_bases[_tower_index]);
		}
		if (Input.IsActionPressed("SelectBuild3")){
			_tower_index = 2;
			EmitSignal(SignalName.SelectBuild, tower_bases[_tower_index]);
		}
		
		if (Input.IsActionPressed("Cancel")){
			_tower_index = -1;
			EmitSignal(SignalName.CancelBuild);
		}

		
		 if (Input.IsActionPressed("Confirm1") )
		{

		if(_tower_index < 0){
			// Nothing selected to build,  select tower maybe
			
			Vector2 selection_pos = GetViewport().GetMousePosition();
			// select tower :/
				foreach(var t in tower_list){
					var diff_vec = t.Position - selection_pos;
					if(diff_vec.Length() < tower_size){
						t.Select();
						// GD.Print("Too Close To roqwer");
						break;
					}
				}

			return;
		}

			if(_root.GetGold() >= costs[0]){
				bool too_close = false;
				building_pos = GetViewport().GetMousePosition();
				// GD.Print($"Build POS:  {building_pos}");
				foreach(var t in tower_list){
					var diff_vec = t.Position - building_pos;
					if(diff_vec.Length() < tower_size){
						too_close = true;
						// GD.Print("Too Close To roqwer");
						break;
					}
				}
				if(!too_close){
					// Too close to path?
					if(_EnemyPath.GetClosestDistance(building_pos) < tower_size/2){
						// GD.Print($"TOO CLOSE TO PATH");
						too_close = true;
					}

				}

					// GD.Print($"Click to build  {just_clicked}    {too_close}");
				// Build
				if(!just_clicked && !too_close)
				{	
					// GD.Print("Click to build");
					just_clicked = true;

					BuildTower();
				}else{
					EmitSignal(SignalName.FailedBuild);
				}
			}
		}else{
			just_clicked = false;
		}
	}
	#endregion
	#region Tower Management
	public void BuildTower(bool already_paid = false){
		if(_tower_index < 0){
			// Nothing selected
			return;
		}
		// Spend the gold needed
		if(!already_paid){
			try{
				GD.Print($"Gonna spend  {_tower_index}  {costs.Count}");
				_root.SpendGold(costs[_tower_index]);	
			}catch(NotEnoughGoldException){
				GD.Print($"Not enough gold");
				return;
			}
		}
		GD.Print($"GONNA BUILD ({_towers.Count})");
		// Build the tower
		var just_built = _towers[0].Instantiate();
		_root.AddChild(just_built);

		// Initialize the tower
		var tower = just_built.GetNode<TowerAndTowerAccessories>(".");
		tower.Position = building_pos;
		tower.SetTimeFactor(_root.time_factor);

		GD.Print($"Building tower with {_tower_index}");
		tower.SetTowerBaseSprite(tower_bases[_tower_index]);
		tower.Initialize(tower_stats[_tower_index]);
		GD.Print($"Buildi2222222222222222ng tower with {_tower_index}");

		// Shift to keep  building
		if(Input.IsActionPressed("ContinueBuilding")){
			GD.Print("ZAQXSWCEDRVGBYBUH");
					EmitSignal(SignalName.SuccessfulBuildContinue);

		}else{
		_tower_index = -1;
					EmitSignal(SignalName.SuccessfulBuild);
		}
		// Don't double purchase
		just_clicked = true;
	}

	public void AddTower(TowerAndTowerAccessories tw){
			GD.Print("Adding tower: ");
			// Should be _root :/
			// Link tower to time
			GetParent<Main>().TimeFactorUpdate += (float t) => tw.SetTimeFactor(t);

			// Tower monitoring list
			tower_list.Add(tw);
	}
	
	public void DestroyAll(){
		// Destroy the Nodes
		foreach(var t in tower_list){
			t.QueueFree();
		}
		// Clear tower management lists
		tower_list.Clear();
	}
	#endregion

	public struct TowerDetails{
		public int type;
		public int x;
		public int y;
		public TowerDetails(int x, int y, int type){
			this.type = type;
			this.x = x;
			this.y = y;
		}
	}
}
