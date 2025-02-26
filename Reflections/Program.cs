var world = new World();
// spawn some entities
world.Spawn(new TestClass() { Name = "1" });
world.Spawn(new TestClass() { Name = "2" });

var testQuery = (Query<TestClass> query) =>
{
  foreach (var q in query.Iter())
  {
    Console.WriteLine(q.Name);
  }
};

world.RegisterSystem(testQuery);
world.Update();


[Component]
public class TestClass
{
  public string Name { get; set; } = string.Empty;
}
