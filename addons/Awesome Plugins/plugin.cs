#if TOOLS
using Godot;
using System;

[Tool]
public partial class plugin : EditorPlugin
{
	private Control interfaceContainer;

	
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		interfaceContainer =  (Control)GD.Load<PackedScene>("res://addons/Awesome Plugins/Scene/interface.tscn").Instantiate<Control>();
		AddControlToBottomPanel(interfaceContainer,"Debug tool");
	}

	public override void _ExitTree()
	{
		RemoveControlFromBottomPanel(interfaceContainer);
		// Clean-up of the plugin goes here.
	}
}
#endif
