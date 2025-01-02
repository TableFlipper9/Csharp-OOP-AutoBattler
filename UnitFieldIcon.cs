using Godot;
using System;

public partial class UnitFieldIcon : Control
{
	public int indexInField;
	public string type = null;

	public TextureRect backIcon;
	public TextureRect charIcon;

	public UnitFieldIcon() { }

	public void Init(string type, int index)
	{
		this.type = type;
		this.indexInField = index;
	}

	public void update(string type)
	{
		this.type = type;
		updateIcon();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(Global.heroLevels[(int)Global.HeroTypes.Chaverly]);
		backIcon = GetNode<TextureRect>("TextureRect");
		charIcon = GetNode<TextureRect>("MarginContainer/TextureRect");
		updateIcon();
	}

	[Signal]
	public delegate void OnMouseEnteredEventHandler(UnitFieldIcon self);
	[Signal]
	public delegate void OnMouseExitedEventHandler();
	[Signal]
	public delegate void MousePressedEventHandler(UnitFieldIcon self);
	[Signal]
	public delegate void MouseReleasedEventHandler();

	public void updateIcon()
	{
		if (this.type != null) { 
			charIcon.Texture = GD.Load<Texture2D>("res://Icons/Units/" + this.type + ".png");
		}
		else { 
			charIcon.Texture = null;
		}
	}
	public void GuiMouseInput(InputEvent evnt)
	{
		if (evnt.IsActionPressed("MouseLeftClick")){
			EmitSignal(UnitFieldIcon.SignalName.MousePressed,this);
		}
		else if (evnt.IsActionReleased("MouseLeftClick")){
			EmitSignal(UnitFieldIcon.SignalName.MouseReleased);
		}
	}
	new public void MouseEntered()
	{
		EmitSignal(UnitFieldIcon.SignalName.OnMouseEntered,this);
	}

	new public void MouseExited()
	{
		EmitSignal(UnitFieldIcon.SignalName.OnMouseExited);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
