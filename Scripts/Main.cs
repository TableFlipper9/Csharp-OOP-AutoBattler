using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using static Global;

public partial class Main : Node2D
{ 
	public EnemyUnitField EFF;
	public EnemyUnitField EBF;
	public HeroUnitField HFF;
	public HeroUnitField HBF;
	public BackGround BackGround;
	public UnitFieldControl FieldControl;
	public UnitShopControl ShopControl;
	public Inventory inventory;

	public PlayButton playButton;
	public CapacityUpgrade frontUpgrade;
	public CapacityUpgrade backUpgrade;

	public Timer timer;

	public override async void _Ready()
	{
		this.HFF = GetNode<HeroUnitField>("HeroFrontField");
		this.HBF = GetNode<HeroUnitField>("HeroBackField");
		this.EFF = GetNode<EnemyUnitField>("EnemyFrontField");
		this.EBF = GetNode<EnemyUnitField>("EnemyBackField");
		this.BackGround = GetNode<BackGround>("BackGround");
		this.FieldControl = GetNode<UnitFieldControl>("FieldControl");
		this.ShopControl = GetNode<UnitShopControl>("UnitShopControl");
		this.inventory = GetNode<Inventory>("Inventory");

		this.playButton = GetNode<PlayButton>("PlayButton");
		this.frontUpgrade = GetNode<CapacityUpgrade>("FrontUpgrade");
		this.backUpgrade = GetNode<CapacityUpgrade>("BackUpgrade");

		this.timer = GetNode<Timer>("Timer");

		this.frontUpgrade.Init("Front","Increase front field capacity by ");
		this.backUpgrade.Init("Back", "Increase front field capacity by ");

		this.EFF.AllEnemiesDeafeted += this.ChangeState;
		this.EBF.AllEnemiesDeafeted += this.ChangeState;

		this.EBF.FindNewEnemy += this.FindNewEnemy;
		this.EFF.FindNewEnemy += this.FindNewEnemy;

		this.HFF.AllHerosDeafeted += this.CheckGameOver;
		this.HBF.AllHerosDeafeted += this.CheckGameOver;

		this.frontUpgrade.UpgradeCap += this.HFF.Upgrade;
		this.backUpgrade.UpgradeCap += this.HBF.Upgrade;

		Global.GainGold(20000);

		ShopControl.PurchaseHero += this.CreateNewHero;
		playButton.StartFight += this.StartFight;
		BackGround.StopMoving += this.UnlockPlaningState;

		HFF.Init(0, new Vector2(400, 370));
		HBF.Init(0, new Vector2(200, 370));
		EFF.Init(0, new Vector2(800, 370));
		EBF.Init(0, new Vector2(1000, 370));

		FieldControl.Init(HFF,HBF);

		HFF.LevelUp(1);
		HBF.LevelUp(1);

		if (Global.SaveFile == false)
		{
			CreateNewHero(0, 0);
		}
		else
		{
			LoadFile("user://Save.txt");
			Global.SaveFile = false;
		}
		EFF.isHero = false;
		EBF.isHero = false;

		EFF.CreateNewFrontField();
		EBF.CreateNewBackField();

		BackGround.StopMoving += HFF.Idle;
		BackGround.StopMoving += HBF.Idle;
	}

	public void SaveFile(string filePath)
	{
		if (true)
		{
			var file = Godot.FileAccess.Open("user://Save.txt", Godot.FileAccess.ModeFlags.Write);

			file.StoreString("Stage=" + Global.Stage+"\n");
			file.StoreString("Gold=" + Global.Gold + "\n");

			int[] PurchaseLevels = new int[5];
			file.StoreString("ButtonsLevels=");
			for(int i = 0; i < ShopControl.units.Count; i++)
			{
				file.StoreString(ShopControl.units[i].level + ",");
			}
			file.StoreString("\n");

			file.StoreString("frontUpgrade=" + frontUpgrade.level+"\n");
			file.StoreString("backUpgrade=" + backUpgrade.level + "\n");

			List<int> Items = new List<int>();
			file.StoreString("Items=");
			for (int i = 0; i < inventory.items.Count; i++)
			{
				file.StoreString((int)inventory.items[i].type + ",");
				Items.Add((int)inventory.items[i].type);
			}
			file.StoreString("\n");

			file.StoreString("HerosFrontField=");
			for(int i = 0; i < HFF.units.Count; i++)
			{
				if (HFF.units[i] != null){
					HFF.units[i].SaveHero(file);
					if(i != HFF.units.Count - 1)
					file.StoreString("|");
				}

			}
			file.StoreString("\n");

			file.StoreString("HerosBackField=");
			List<HeroSave> back = new List<HeroSave>();
			for (int i = 0; i < HBF.units.Count; i++)
			{
				if (HBF.units[i] != null)
				{
					HBF.units[i].SaveHero(file);
					if (i != HBF.units.Count - 1)
						file.StoreString("|");
				}
			}
			file.StoreString("\n");

			GD.Print("Data loaded from file.");
			file.Close();
		}
		else
		{
			GD.Print("File not found.");
		}
	}

	public void LoadFile(string filePath)
	{
		if (Godot.FileAccess.FileExists(filePath))
		{
			var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
			string content = file.GetAsText(); 

			string[] lines = content.Split('\n');

			foreach (var line in lines)
			{
				if (string.IsNullOrWhiteSpace(line))
					continue;

				var parts = line.Split(new[] { '=' }, 2);

				if (parts.Length < 2)
					continue;

				var key = parts[0].Trim();
				var value = parts[1].Trim();

				switch (key)
				{
					case "Stage":
						Global.Stage = int.Parse(value);
						break;
					case "Gold":
						Global.Gold = int.Parse(value);
						break;
					case "ButtonsLevels":
						ParseButtonLevels(value);
						break;
					case "frontUpgrade":
						frontUpgrade.Load(int.Parse(value));
						break;
					case "backUpgrade":
						backUpgrade.Load(int.Parse(value));
						break;
					case "Items":
						ParseItems(value);
						break;
					case "HerosFrontField":
						ParseHeroField(value);
						break;
					case "HerosBackField":
						ParseHeroField(value);
						break;
					default:
						GD.Print($"Unrecognized key: {key}");
						break;
				}
			}

			GD.Print("Data loaded successfully.");
			file.Close();
		}
		else
		{
			GD.Print("File not found.");
		}
	}

	private void ParseButtonLevels(string value)
	{
		var levels = value.Split(',');
		for (int i = 0; i < levels.Length; i++)
		{
			if (int.TryParse(levels[i], out int level))
			{
				ShopControl.units[i].level = level;
			}
		}
	}

	private void ParseItems(string value)
	{
		var items = value.Split(',');
		foreach (var itemStr in items)
		{
			if (int.TryParse(itemStr, out int itemType))
			{
				inventory.AddUnit((Iteams)itemType);
			}
		}
	}

	private void ParseHeroField(string value)
	{
		var heroesData = value.Split('|');
		foreach (var heroData in heroesData)
		{
			var heroParams = heroData.Split(',');
			if (heroParams.Length >= 5)
			{
				Global.LoadingHero = new HeroSave
				{
					health = int.Parse(heroParams[0]),
					maxHealth = int.Parse(heroParams[1]),
					type = int.Parse(heroParams[2]),
					damage =  int.Parse(heroParams[3]) ,
					level = int.Parse(heroParams[4]),
					charge = int.Parse(heroParams[5]),
				};
				CreateNewHero(Global.LoadingHero.type, 0);
			}
		}
	}

	//////////////////////////////////////////////////////////////////////////////////////////

	public void StartFight()
	{
		Global.stateChanger = true;
		HFF.Attack();
		EFF.Attack();
		HBF.Attack();
		EBF.Attack();
	}
	public void UnlockPlaningState()
	{
		Global.stateChanger = false;
		playButton.StartPlan();
		if(Global.Stage == 5)
		{
			GetTree().Quit();
		}
		EFF.CreateNewFrontField();
		EBF.CreateNewBackField();
	}

	/// need to fix front line and back line creation to match the signals, and pay attention when swapping unit between fields
	public void FindNewEnemy(Unit guy,int prio)
	{
		List<Unit> primary = null;
		List<Unit> secondary = null;

		switch ((Global.priority)prio)
		{
			case Global.priority.backfrontE:
				primary = EBF.units;
				secondary = EFF.units;
				break;
			case Global.priority.frontbackE:
				primary = EFF.units;
				secondary = EBF.units;
				break;
			case Global.priority.backfrontH:
				primary = HBF.units;
				secondary = HFF.units;
				break;
			case Global.priority.frontbackH:
				primary = HFF.units;
				secondary = HBF.units;
				break;
		}
		bool okp = false;
		foreach (Unit unit in primary) {
			if (unit != null)
				{ okp = true; break; }
		}
		bool oks = false;
		foreach (Unit unit in secondary){
			if (unit != null)
			{ oks = true; break; }
		}

		if (okp) guy.moveToAttack(guy.closestEnemy(primary).GlobalPosition);
		else if (oks) guy.moveToAttack(guy.closestEnemy(secondary).GlobalPosition);
		else{
			guy.enemy = null;
			guy.updateInRowLocation(guy.locationInrow);
		}
	}

	public void CheckGameOver()
	{
		if (HFF.isEmpty == true && HBF.isEmpty == true)
		{
			GetTree().ChangeSceneToFile("res://Scenes/GameOver.tscn");
		}
	}
	
	public void OnTimerTimeout()
	{
		Global.Stage++;
		SaveFile("user://Save.txt");
		HFF.Move();
		HBF.Move();
		BackGround.Move();
	}

	public int itemDrop = 0;
	public void ChangeState()
	{
		if (EFF.isEmpty == true && EBF.isEmpty == true)
		{
			Global.stateChanger = true;
			timer.Start(2);
			inventory.AddUnit((Global.Iteams)itemDrop);
			itemDrop++;
		}
	}

	public void CreateNewHero(int type, int Price)
	{
		GD.Print(type);
		if (HFF.HasAvalibleSpace()){
			switch (type)
			{
				case 0:
					var hr = GD.Load<PackedScene>("res://Scenes/Solder.tscn").Instantiate<Solder>();
					hr.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					hr.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += hr.LevelUp;
					HFF.AddNewUnit(hr);
					if (Global.SaveFile) { hr.UploadData(Global.LoadingHero); }
					break;
				case 1:
					var ar = GD.Load<PackedScene>("res://Scenes/Archer.tscn").Instantiate<Archer>();
					ar.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					ar.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ar.LevelUp;
					HFF.AddNewUnit(ar);
					if (Global.SaveFile) { ar.UploadData(Global.LoadingHero); }
					break;
				case 2:
					var hl = GD.Load<PackedScene>("res://Scenes/Healer.tscn").Instantiate<Healer>();
					hl.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					hl.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += hl.LevelUp;
					HFF.AddNewUnit(hl);
					if (Global.SaveFile) { hl.UploadData(Global.LoadingHero); }
					break;
				case 3:
					var ch = GD.Load<PackedScene>("res://Scenes/Chaverly.tscn").Instantiate<Chaverly>();
					ch.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					ch.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ch.LevelUp;
					HFF.AddNewUnit(ch);
					if (Global.SaveFile) { ch.UploadData(Global.LoadingHero); }
					break;
				case 4:
					var ex = GD.Load<PackedScene>("res://Scenes/Executioner.tscn").Instantiate<Executioner>();
					ex.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					ex.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ex.LevelUp;
					HFF.AddNewUnit(ex);
					if (Global.SaveFile) { ex.UploadData(Global.LoadingHero); }
					break;
				case 5:
					var kn = GD.Load<PackedScene>("res://Scenes/Knight.tscn").Instantiate<Knight>();
					kn.Init((Global.HeroTypes)type, HFF.locations[HFF.nextavalibleLocation]);
					kn.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += kn.LevelUp;
					HFF.AddNewUnit(kn);
					if (Global.SaveFile) { kn.UploadData(Global.LoadingHero); }
					break;
			}
			Global.SpendGold(Price);
		}
		else if (HBF.HasAvalibleSpace())
		{
			switch (type)
			{
				case 0:
					var hr = GD.Load<PackedScene>("res://Scenes/Solder.tscn").Instantiate<Solder>();
					hr.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					hr.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += hr.LevelUp;
					HBF.AddNewUnit(hr);
					if (Global.SaveFile) { hr.UploadData(Global.LoadingHero); }
					break;
				case 1:
					var ar = GD.Load<PackedScene>("res://Scenes/Archer.tscn").Instantiate<Archer>();
					ar.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					ar.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ar.LevelUp;
					HBF.AddNewUnit(ar);
					if (Global.SaveFile) { ar.UploadData(Global.LoadingHero); }
					break;
				case 2:
					var hl = GD.Load<PackedScene>("res://Scenes/Healer.tscn").Instantiate<Healer>();
					hl.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					hl.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += hl.LevelUp;
					HBF.AddNewUnit(hl);
					if (Global.SaveFile) { hl.UploadData(Global.LoadingHero); }
					break;
				case 3:
					var ch = GD.Load<PackedScene>("res://Scenes/Chaverly.tscn").Instantiate<Chaverly>();
					ch.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					ch.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ch.LevelUp;
					HBF.AddNewUnit(ch);
					if (Global.SaveFile) { ch.UploadData(Global.LoadingHero); }
					break;
				case 4:
					var ex = GD.Load<PackedScene>("res://Scenes/Executioner.tscn").Instantiate<Executioner>();
					ex.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					ex.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += ex.LevelUp;
					HBF.AddNewUnit(ex);
					if (Global.SaveFile) { ex.UploadData(Global.LoadingHero); }
					break;
				case 5:
					var kn = GD.Load<PackedScene>("res://Scenes/Knight.tscn").Instantiate<Knight>();
					kn.Init((Global.HeroTypes)type, HBF.locations[HBF.nextavalibleLocation]);
					kn.SearchNewOpponent += this.FindNewEnemy;
					this.ShopControl.units[type].OnLevelUp += kn.LevelUp;
					HBF.AddNewUnit(kn);
					if (Global.SaveFile) { kn.UploadData(Global.LoadingHero); }
					break;
			}
			Global.SpendGold(Price);
		}
	}
	public override void _Process(double delta)
	{
	}
}
