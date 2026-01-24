using Agecanonix.Application.DTOs.FacilityCategory;
using Agecanonix.Application.Features.FacilityCategories.Commands;
using Agecanonix.Application.Features.FacilityCategories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class FacilityCategoryEndpoints
{
    public static void MapFacilityCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facility-categories")
            .WithTags("FacilityCategories");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var categories = await mediator.Send(new GetAllFacilityCategoriesQuery());
            return Results.Ok(categories);
        })
        .WithName("GetAllFacilityCategories")
        .Produces<IEnumerable<FacilityCategoryDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var category = await mediator.Send(new GetFacilityCategoryByIdQuery(id));
            return category is not null ? Results.Ok(category) : Results.NotFound();
        })
        .WithName("GetFacilityCategoryById")
        .Produces<FacilityCategoryDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateFacilityCategoryDto dto, IMediator mediator) =>
        {
            try
            {
                var category = await mediator.Send(new CreateFacilityCategoryCommand(dto));
                return Results.Created($"/api/facility-categories/{category.Id}", category);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateFacilityCategory")
        .Produces<FacilityCategoryDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateFacilityCategoryDto dto, IMediator mediator) =>
        {
            try
            {
                var category = await mediator.Send(new UpdateFacilityCategoryCommand(id, dto));
                return Results.Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("UpdateFacilityCategory")
        .Produces<FacilityCategoryDto>(200)
        .Produces(404)
        .Produces(400);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var deleted = await mediator.Send(new DeleteFacilityCategoryCommand(id));
                return deleted ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteFacilityCategory")
        .Produces(204)
        .Produces(404)
        .Produces(400);
    }
}
