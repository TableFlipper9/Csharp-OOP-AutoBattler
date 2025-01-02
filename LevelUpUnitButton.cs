using Godot;
using System;
using System.Reflection;

public partial class LevelUpUnitButton : Control
{

	private Global.HeroTypes type;
	private int levelUPPrice = 0;
	public int purchasePrice = 0;
	private bool buyLevel;
	private bool buyPurchase;
	private int level = 1;

	private TextureRect icon;

	public void CheckBuyable()
	{
		if (Global.Gold > purchasePrice){
			this.SelfModulate = new Color(1, 1, 1, 1);

			buyPurchase = true;
		}
		else{
			this.SelfModulate = new Color(255, 255, 255, 115);
			buyPurchase = false;
		}
	}

	public void ChangeIcon()
	{
		if(purchasePrice/2 <= Global.Gold){
			icon.Texture = GD.Load<Texture2D>("res://Icons/Units/" + Global.heroNames[Global.heroStringMap[this.type]] + ".png");
		}
	}

	[Signal]
	public delegate void OnPurchaseEventHandler(int type, int Price);
	[Signal]
	public delegate void OnLevelUpEventHandler();

	public void OnLevelUPButtuonPressed()
	{
		GD.Print("PP ", Global.heroLevels[(int)this.type]);
		if (Global.Gold > levelUPPrice){
			Global.SpendGold(levelUPPrice);
			EmitSignal(LevelUpUnitButton.SignalName.OnLevelUp);
			CheckBuyable();
			Global.SpendGold(levelUPPrice);
			Global.heroLevels[(int)this.type]++;
			GD.Print("OO ", Global.heroLevels[(int)this.type]);
		}
	}

	public void OnButtonPressed()
	{
		if (buyPurchase){
		   EmitSignal(LevelUpUnitButton.SignalName.OnPurchase, (int)this.type, this.purchasePrice);
		   CheckBuyable() ;
		}
	}
	public void Init(Global.HeroTypes type)
	{
		this.type = type;
		this.levelUPPrice = Global.levelUpPrices[Global.heroStringMap[this.type]];
		this.purchasePrice = Global.purchaseUpPrices[Global.heroStringMap[this.type]];
		CheckBuyable(); /// need to create globalsignal to update money
	}
	public override void _Ready()
	{
		icon = GetNode<TextureRect>("MarginContainer/TextureRect");
	}

	public override void _Process(double delta)
	{

	}
}
