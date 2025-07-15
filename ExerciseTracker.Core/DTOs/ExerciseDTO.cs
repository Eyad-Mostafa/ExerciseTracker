using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Core.DTOs;

public class CreateExerciseDTO
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [MaxLength(400)]
    public string? Comments { get; set; }
}
