using RedBlackTree;
namespace Linear;
 
public class LinearSearch
{
    private List<MusicInstrument> items = new List<MusicInstrument>();

    public void Insert(MusicInstrument value)
    {
        items.Add(value);
    }

    public int Find(int key)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].Id == key)
                return i;

        return -1;
    }

    // Получить элемент
    public MusicInstrument Get(int key)
    {
        return items[Find(key)];
    }

    public void Delete(int key)
    {
        int index = Find(key);
        if (index != -1)
            items.RemoveAt(index);
    }

    public void ShowAll()
    {
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
}
