using Godot;
using WildJam2023.Scripts.Extensions;

namespace WildJam2023.Scripts;

public partial class Bomb : Teleportable.TeleportableRigidbody2D
{
  [Export] private float throwVelocity;

  public void Throw(Vector2 direction)
  {
    Freeze = false;
    Reparent(this.GetRootNode());
    ApplyImpulse(direction * throwVelocity);
  }

  private async void OnBodyEntered(Node2D body)
  {
    if (body is Portal) return;

    SetDeferred(RigidBody2D.PropertyName.Freeze, true);
    await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("explosion");
    await ToSignal(GetNode<AnimatedSprite2D>("AnimatedSprite2D"), AnimatedSprite2D.SignalName.AnimationFinished);
    QueueFree();
  }

  public override void _IntegrateForces(PhysicsDirectBodyState2D state)
  {
    if (WillTeleport && TeleportTarget != Vector2.Zero)
    {
      var transform = state.Transform;
      transform.Origin = TeleportTarget;
      state.Transform = transform;
      WillTeleport = false;
      TeleportTarget = Vector2.Zero;
    }
  }

  public override void Teleport(Vector2 destination)
  {
    TeleportTarget = destination;
    WillTeleport = true;
  }
}