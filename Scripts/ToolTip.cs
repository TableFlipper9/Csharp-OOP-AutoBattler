using Godot;
using System;

public partial class ToolTip : Control
{
	public Label title;
	public RichTextLabel description;
	public string des;
	public string ttl;
	public override void _Ready()
	{
		title = GetNode<Label>("MarginContainer/MarginContainer/VBoxContainer/MarginContainer/Label");
		description = GetNode<RichTextLabel>("MarginContainer/MarginContainer/VBoxContainer/MarginContainer2/Label");
		title.Text = ttl;
		description.Text = des;
		description.BbcodeEnabled = true;
	}

	public void Init(string des, string ttl)
	{
		this.des = des;
		this.ttl = ttl;
	}
	
	public void UpdateSTring(string des, string ttl)
	{
		title.Text = ttl;
		description.Text = des;
	}

	public override void _Process(double delta)
	{
		this.GlobalPosition = new Vector2 (GetGlobalMousePosition().X + 10,GetGlobalMousePosition().Y + 10);
	}
}
