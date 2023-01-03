using DomainModels.Interfaces;

namespace DomainModels.Entities;

public class Item : IIdentifiable
{
    protected Item() { }

    public Item(string name)
    {
        Name = name;
    }

    public long Id { get ; set ; }
    public string Name { get ; set ; }
}