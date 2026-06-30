using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class UpdateGameDto(

    // Introducing the properties that we'll be receiving in this endpoint
    [Required][StringLength(50)] string Name, // Required: makes it to where it throws an error if there is no value input for Name
                                              // StringLength: Throws error if name length is longer than the specified amount of
                                              // characters
    [Range(1, 50)] int GenreId,
    [Range(1, 150)] decimal Price, // Range: sets range of allowed values for Price variable - Throws error if not in range
    DateOnly ReleaseDate
);