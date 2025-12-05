namespace RedBlackTree;

public class MusicInstrument
{
    public int Id;
    public string Name;
    public int Price;

    public MusicInstrument(int id, string name, int price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Price: {Price}";
    }
}