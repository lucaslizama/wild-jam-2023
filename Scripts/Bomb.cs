using Godot;

public partial class Bomb : RigidBody2D
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
    SetDeferred(PropertyName.Freeze, true);
    await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("explosion");
    await ToSignal(GetNode<AnimatedSprite2D>("AnimatedSprite2D"), AnimatedSprite2D.SignalName.AnimationFinished);
    QueueFree();
  }
}
