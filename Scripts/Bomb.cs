using Godot;

public partial class Bomb : RigidBody2D
{
  [Export] private float throwVelocity;

  public bool willTeleport = false;
  public Vector2 teleportTarget = Vector2.Zero;

  public void Throw(Vector2 direction)
  {
    Freeze = false;
    Reparent(this.GetRootNode());
    ApplyImpulse(direction * throwVelocity);
  }

  private async void OnBodyEntered(Node2D body)
  {
    if (body is Portal) return;

    SetDeferred(PropertyName.Freeze, true);
    await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("explosion");
    await ToSignal(GetNode<AnimatedSprite2D>("AnimatedSprite2D"), AnimatedSprite2D.SignalName.AnimationFinished);
    QueueFree();
  }

  public override void _IntegrateForces(PhysicsDirectBodyState2D state)
  {
    if (willTeleport && teleportTarget != Vector2.Zero)
    {
      var transform = state.Transform;
      transform.Origin = teleportTarget;
      state.Transform = transform;
      willTeleport = false;
      teleportTarget = Vector2.Zero;
    }
  }
}
