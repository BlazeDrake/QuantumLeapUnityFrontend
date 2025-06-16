using System;

public record NavigationState : StandardSystemBaseState
{
    public RequestedCourseCalculation[] RequestedCourseCalculations { get; set; } = Array.Empty<RequestedCourseCalculation>();
    public CalculatedCourse[] CalculatedCourses { get; set; } = Array.Empty<CalculatedCourse>();
    public CurrentCourse CurrentCourse { get; set; }
}

public record RequestedCourseCalculation
{
    public string CourseId { get; set; }
    public string Destination { get; set; }
    public DateTimeOffset RequestedAt { get; set; } = DateTimeOffset.UtcNow;
}

public record CalculatedCourse
{
    public string CourseId { get; set; }
    public string Destination { get; set; }
    public Point Coordinates { get; set; }
    public Eta Eta { get; set; }
    public DateTimeOffset CalculatedAt { get; set; }
}

public record CurrentCourse
{
    public string CourseId { get; set; }
    public string Destination { get; set; }
    public Point Coordinates { get; set; }
    public Eta Eta { get; set; }
    public DateTimeOffset CourseSetAt { get; set; }
}

public record Eta
{
    public string EngineSystem { get; set; }
    public TravelTime[] TravelTimes { get; set; }
}

public record TravelTime
{
    public int Speed { get; set; }
    public int ArriveInMilliseconds { get; set; }
}
