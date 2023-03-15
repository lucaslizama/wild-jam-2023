using Godot;

public partial class Bomb : RigidBody2D
{
  [Export] private float throwVelocity;

  public void Throw(Vector2 direction)
  {
    Freeze = false;
    ApplyImpulse(direction * throwVelocity);
  }
}
