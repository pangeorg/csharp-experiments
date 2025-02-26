
public record Component
{
  public Guid Id { get; } = Guid.NewGuid();
  public Type Type { get; set; } = null!;

  public static Component New<T>()
  {
    return new Component()
    {
      Type = typeof(T),
    };
  }
}
