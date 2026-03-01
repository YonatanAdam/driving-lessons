namespace Model;

public class Review : BaseEntity {
    private int bookingId;
    private int studentId;
    private int teacherId;
    private int rating;
    private string? reviewText;
    private DateTime createdAt;

    public int BookingId
    {
        get => bookingId;
        set => bookingId = value;
    }

    public int StudentId
    {
        get => studentId;
        set => studentId = value;
    }

    public int TeacherId
    {
        get => teacherId;
        set => teacherId = value;
    }

    public int Rating
    {
        get => rating;
        set => rating = value;
    }

    public string ReviewText
    {
        get => reviewText;
        set => reviewText = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime CreatedAt
    {
        get => createdAt;
        set => createdAt = value;
    }
}