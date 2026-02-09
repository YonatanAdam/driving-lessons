namespace Model;

public abstract class BaseEntity
{
    private int id;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public override string ToString()
    {
        return $"Id: {Id} - ";
    }
}
