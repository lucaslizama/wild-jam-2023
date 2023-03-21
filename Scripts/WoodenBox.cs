using Godot;
using WildJam2023.Scripts.Extensions;

namespace WildJam2023.Scripts;

public partial class WoodenBox : StaticBody2D
{
	private void OnArea2DBodyEntered(Node2D body)
	{
		if (body is not Bomb bomb) return;
		var vfx = GetNode<CpuParticles2D>("VFX_WoodenBoxDestroy");
		vfx.Emitting = true;
		vfx.Reparent(this.GetRootNode());

		QueueFree();
	}
}