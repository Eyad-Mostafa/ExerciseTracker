using ExerciseTracker.Core.DTOs;
using ExerciseTracker.Core.Models;

namespace ExerciseTracker.API.Services;

public interface IExerciseService
{
    IEnumerable<Exercise> GetAllExercises();
    Exercise? GetExerciseById(int id);
    Exercise AddExercise(CreateExerciseDTO exercise);
    Exercise? UpdateExercise(int id, CreateExerciseDTO exercise);
    Exercise? DeleteExercise(int id);
}
