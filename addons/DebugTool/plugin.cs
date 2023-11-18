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
		interfaceContainer =  (Control)GD.Load<PackedScene>("res://addons/DebugTool/Scene/Interface.tscn").Instantiate<Control>();
		var customNode3dDebug = 	GD.Load<Script>("res://addons/DebugTool/Scene/DebugNode3d.cs");
		var ExposedScene = 	GD.Load<Script>("res://addons/DebugTool/Scene/DebugNode3d.cs");
		
		AddCustomType("DebugNode3d","StaticBody3D",customNode3dDebug,null);
		AddControlToBottomPanel(interfaceContainer,"Debug tool");
	}

	public override void _ExitTree()
	{
		RemoveCustomType("DebugNode3d");
		RemoveControlFromBottomPanel(interfaceContainer);
		// Clean-up of the plugin goes here.
	}
}
#endif
