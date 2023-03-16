using Godot;

public partial class WoodenBox : StaticBody2D
{
  private PackedScene destroyedVfx;

  private void OnArea2DBodyEntered(Node2D body)
  {
    if (body is Bomb bomb)
    {
      var vfx = GetNode<CpuParticles2D>("VFX_WoodenBoxDestroy");
      vfx.Emitting = true;
      vfx.Reparent(this.GetRootNode());

      QueueFree();
    }
  }
}


