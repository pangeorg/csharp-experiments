namespace Tests;
using Bag;

public class BagTests
{
    [Fact]
    public void Add()
    {
        // arrange
        var bag = new Bag<string>();

        // act
        bag.Add("one");
        bag.Add("one");
        bag.Add("two");

        // assert
        Assert.Equal((uint)2, bag["one"]);
        Assert.Equal((uint)1, bag["two"]);
    }

    [Fact]
    public void GetThrowsExceptionIfNotExists()
    {
        //arrange
        var bag = new Bag<string>();

        // act
        bag.Add("one");
        bag.Add("one");
        bag.Add("two");

        // assert
        Assert.Throws<KeyNotFoundException>(() => bag["three"]);
    }

    [Fact]
    public void GetOrDefaultReturnsZeroIfNotExists()
    {
        //arrange
        var bag = new Bag<string>();

        // act
        bag.Add("one");
        bag.Add("one");
        bag.Add("two");

        // assert
        Assert.Equal((uint)0, bag.GetOrDefault("three"));
    }

    [Fact]
    public void Remove()
    {
        //arrange
        var bag = new Bag<string>();

        // act
        bag.Add("one");
        bag.Add("one");
        bag.Add("two");
        bag.Add("three");
        bag.Remove("three", 2);

        // assert
        Assert.Equal((uint)0, bag.GetOrDefault("three"));
        Assert.Equal((uint)2, bag["one"]);
        Assert.Equal((uint)1, bag["two"]);
    }

    [Fact]
    public void Delete()
    {
        //arrange
        var bag = new Bag<string>();

        // act
        bag.Add("one");
        bag.Add("one");
        bag.Add("two");
        bag.Delete("one");

        // assert
        Assert.Equal((uint)0, bag.GetOrDefault("one"));
        Assert.Equal((uint)1, bag["two"]);
    }

    [Fact]
    public void Count()
    {
        //arrange
        var bag = new Bag<int>();
        int count = 100;

        // act
        for (int i = 0; i < count; i++)
        {
            bag.Add(i);
        }

        // assert
        Assert.Equal(count, bag.Count());
    }

    [Fact]
    public void IntersectWith()
    {
        //arrange
        int count = 100;
        var bag = new Bag<int>();
        var l = new List<int>();

        // act
        for (int i = 0; i < count; i++)
        {
            bag.Add(i);
            if (i >= 30 && i <= 60)
            {
                l.Add(i);
            }
        }
        bag.IntersectWith(l);

        // assert
        for (int i = 0; i < count; i++)
        {
            if (i >= 30 && i <= 60)
            {
                Assert.Equal((uint)2, bag[i]);
            }
            else
            {
                Assert.Equal((uint)0, bag.GetOrDefault(i));
            }
        }
    }

    [Fact]
    public void Overlaps()
    {
        //arrange
        int count = 100;
        var bag = new Bag<int>();
        var l = new List<int>();

        // act
        for (int i = 0; i < count; i++)
        {
            bag.Add(i);
            if (i >= 30 && i <= 60)
            {
                l.Add(i);
            }
        }

        // assert
        Assert.True(bag.Overlaps(l));
    }
}
