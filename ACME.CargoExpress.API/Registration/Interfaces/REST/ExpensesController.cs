using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;


namespace ACME.CargoExpress.API.Registration.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
public class ExpensesController(IExpenseCommandService expenseCommandService, IExpenseQueryService expenseQueryService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseResource createExpenseResource)
    {
        try
        {
            var createExpenseCommand = CreateExpenseCommandFromResourceAssembler.ToCommandFromResource(createExpenseResource);
            var expense = await expenseCommandService.Handle(createExpenseCommand);
            if (expense is null) return BadRequest(new { message = "No se pudo crear el gasto." });
            var resource = ExpenseResourceFromEntityAssembler.ToResourceFromEntity(expense);
            return CreatedAtAction(nameof(GetExpenseById), new { expenseId = resource.Id }, resource);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{expenseId}")]
    public async Task<IActionResult> UpdateExpense([FromBody] UpdateExpenseResource updateExpenseResource, [FromRoute] int expenseId)
    {
        try
        {
            var updateExpenseCommand = UpdateExpenseCommandFromResourceAssembler.ToCommandFromResource(updateExpenseResource, expenseId);
            var expense = await expenseCommandService.Handle(updateExpenseCommand);
            if (expense is null) return NotFound(new { message = "No se ha encontrado el gasto." });
            var resource = ExpenseResourceFromEntityAssembler.ToResourceFromEntity(expense);
            return Ok(resource);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllExpenses()
    {
        var getAllExpensesQuery = new GetAllExpensesQuery();
        var expenses = await expenseQueryService.Handle(getAllExpensesQuery);
        var resources = expenses.Select(ExpenseResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{expenseId}")]
    public async Task<IActionResult> GetExpenseById([FromRoute] int expenseId)
    {
        var expense = await expenseQueryService.Handle(new GetExpenseByIdQuery(expenseId));
        if (expense == null) return NotFound(new { message = "No se ha encontrado el gasto." });
        var resource = ExpenseResourceFromEntityAssembler.ToResourceFromEntity(expense);
        return Ok(resource);
    }

    [HttpPut("{expenseId}/state")]
    public async Task<IActionResult> UpdateExpenseState([FromBody] UpdateExpenseStateResource updateExpenseStateResource, [FromRoute] int expenseId)
    {
        try
        {
            if (updateExpenseStateResource.State == null)
                return BadRequest(new { message = "El campo 'state' es requerido y solo acepta true o false." });

            var command = new UpdateExpenseStateCommand(expenseId, updateExpenseStateResource.State.Value);
            var expense = await expenseCommandService.Handle(command);
            if (expense == null) return NotFound(new { message = "No se ha encontrado el gasto." });
            return Ok(new { id = expense.Id, state = expense.State });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
