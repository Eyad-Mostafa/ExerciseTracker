using ExerciseTracker.API.Services;
using ExerciseTracker.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExercisesController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExercisesController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    /// <summary>
    /// Gets all exercises.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ExerciseResponseDTO>), StatusCodes.Status200OK)]
    public ActionResult<List<ExerciseResponseDTO>> GetAll()
    {
        var exercises = _exerciseService.GetAllExercises();

        var result = exercises.Select(e => new ExerciseResponseDTO
        {
            Id = e.Id,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Comments = e.Comments
        }).ToList();

        return Ok(result);
    }

    /// <summary>
    /// Gets an exercise by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ExerciseResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ExerciseResponseDTO> GetById(int id)
    {
        var exercise = _exerciseService.GetExerciseById(id);
        if (exercise == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Exercise with ID {id} not found.",
                Status = 404
            });
        }

        var result = new ExerciseResponseDTO
        {
            Id = exercise.Id,
            StartDate = exercise.StartDate,
            EndDate = exercise.EndDate,
            Comments = exercise.Comments
        };

        return Ok(result);
    }

    /// <summary>
    /// Creates a new exercise.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ExerciseResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public ActionResult<ExerciseResponseDTO> AddExercise([FromBody] CreateExerciseDTO dto)
    {
        try
        {
            var createdExercise = _exerciseService.AddExercise(dto);

            var result = new ExerciseResponseDTO
            {
                Id = createdExercise.Id,
                StartDate = createdExercise.StartDate,
                EndDate = createdExercise.EndDate,
                Comments = createdExercise.Comments
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Input",
                Detail = ex.Message,
                Status = 400
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Server Error",
                Detail = ex.Message,
                Status = 500
            });
        }
    }

    /// <summary>
    /// Updates an existing exercise.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ExerciseResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult UpdateExercise(int id, [FromBody] CreateExerciseDTO dto)
    {
        try
        {
            var updated = _exerciseService.UpdateExercise(id, dto);
            if (updated == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Exercise with ID {id} not found.",
                    Status = 404
                });
            }

            var result = new ExerciseResponseDTO
            {
                Id = updated.Id,
                StartDate = updated.StartDate,
                EndDate = updated.EndDate,
                Comments = updated.Comments
            };

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid Input",
                Detail = ex.Message,
                Status = 400
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Server Error",
                Detail = ex.Message,
                Status = 500
            });
        }
    }

    /// <summary>
    /// Deletes an exercise by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        try
        {
            var deleted = _exerciseService.DeleteExercise(id);
            if (deleted == null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Detail = $"Exercise with ID {id} not found.",
                    Status = 404
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Server Error",
                Detail = ex.Message,
                Status = 500
            });
        }
    }
}
