namespace Model;

public class Booking : BaseEntity {
    private int userId;
    private int teacherId;
    private DateTime lessonDateTime;
    private string? status;
    private double durationHours;
    private DateTime createdAt;

    public int UserId
    {
        get => userId;
        set => userId = value;
    }

    public int TeacherId
    {
        get => teacherId;
        set => teacherId = value;
    }

    public DateTime LessonDateTime
    {
        get => lessonDateTime;
        set => lessonDateTime = value;
    }

    public string? Status
    {
        get => status;
        set => status = value ?? throw new ArgumentNullException(nameof(value));
    }

    public double DurationHours
    {
        get => durationHours;
        set => durationHours = value;
    }
    
    public DateTime CreatedAt
    {
        get => createdAt;
        set => createdAt = value;
    }
}