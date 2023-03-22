using Godot;
using WildJam2023.Scripts.Extensions;

namespace WildJam2023.Scripts;

public partial class WoodenBox : StaticBody2D
{
	private Sprite2D sprite;
	private Timer destroyTimer;
	private CpuParticles2D destroyVfx;
	private CollisionShape2D collider;
	private CollisionShape2D trigger;

	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Sprite2D");
		destroyTimer = GetNode<Timer>("CleanTimer");
		destroyVfx = GetNode<CpuParticles2D>("DestroyVFX");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
		trigger = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
	}

	private async void OnArea2DBodyEntered(Node2D body)
	{
		if (body is not Bomb) return;
		sprite.Hide();
		destroyVfx.Emitting = true;
		collider.SetDeferred("disabled", true);
		trigger.SetDeferred("disabled", true);
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		destroyTimer.Start();
	}

	private void OnCleanTimerTimeout() => QueueFree();
}