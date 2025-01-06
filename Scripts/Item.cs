using Godot;
using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

public partial class Item : Control
{
	public Global.Iteams type;
	public Label text;
	public string name;
	public TextureRect image;
	public string stat;
	public int index;
	public override void _Ready()
	{
		this.text = GetNode<Label>("MarginContainer2/HBoxContainer/VBoxContainer/Label");
		this.image = GetNode<TextureRect>("MarginContainer/TextureRect");

		text.Text = name;
		Godot.Color color = new Godot.Color(0,0,0);
		switch ((int)type)
		{
			case 0 or 1 or 2:
				color = new Godot.Color(100,100,100);
				stat = " HP";
				break;
			case 3 or 4:
				color = new Godot.Color(120, 120, 120);
				stat = " MaxHp";
				break;
			case 5 or 6:
				color = new Godot.Color(50, 50, 50);
				stat = " Damage";
				break;
			case 7:
				color = new Godot.Color(42, 42, 160);
				stat = " Level";
				break;
		}
			text.AddThemeColorOverride("font_shadow_color", color);

		image.Texture = GD.Load<Texture2D>("res://Icons/FreePixelFood/" + Global.itemsNames[(int)type] + ".png");
	}

	public ToolTip info = null;
	public void MouseEntered()
	{
		var tt = GD.Load<PackedScene>("res://Scenes/ToolTip.tscn").Instantiate<ToolTip>();
		tt.Init(Global.itemsDescriptions[(int)type] + " [color=#00FF00]+" + Global.itemValues[(int)(type)] + stat + "[/color]", Global.itemsNames[(int)type]);
		GetTree().Root.AddChild(tt);
		info = tt;
	}

	public void MouseExited()
	{
		info.QueueFree();
	}

	public void Init(Global.Iteams type,string text,int index)
	{
		this.name = text;
		this.type = type;
		this.index = index;
	}

	[Signal]
	public delegate void ButtonPressedEventHandler(int type,int value,int index);
	public void OnItemPressed(InputEvent evnt)
	{
		if (evnt.IsActionPressed("MouseLeftClick"))
		{
			EmitSignal(Item.SignalName.ButtonPressed, (int)this.type, Global.itemValues[(int)type],this.index);
		}
	}
}
