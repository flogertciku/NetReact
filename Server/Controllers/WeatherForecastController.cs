using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers;


[Route("api/TodoItems")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private bool TodoItemExists(long id)
{
    return _context.TodoItems.Any(e => e.Id == id);
}
    private TodoContext _context;
    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    {
        return await _context.TodoItems.ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    {
        // Find the item in question
        var todoItem = await _context.TodoItems.FindAsync(id);
        // Check to see if we got back null, in which case return NotFound
        if (todoItem == null)
        {
            return NotFound();
        }
        // Otherwise, return the item
        return todoItem;
    }
    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItem todoItem)
    {
        System.Console.WriteLine(todoItem);
        if (ModelState.IsValid)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            // This uses the GetTodoItem route we wrote above
            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                todoItem);
        }
        else
        {
            // This is what will allow us to get error messages for our front end
            return BadRequest(ModelState);
        }
    }

    [HttpPost("update/{id}")]
public async Task<IActionResult> UpdateTodoItem(long id, [FromBody] TodoItem todoItem)
{
    System.Console.WriteLine("ne fillim");
    // If the id from the route doesn't match the id of the item we passed along, throw a bad request
    if (id != todoItem.Id)
    {
        System.Console.WriteLine("f the id from the route doesn't match the id");
        return BadRequest();
    }
    // Find the original item
    var originalTodoItem = await _context.TodoItems.FindAsync(id);
    // Verify the original item exists
    if (originalTodoItem == null)
    {
        System.Console.WriteLine("Verify the original item exists");
        return NotFound();
    }
    // Added to update item
    originalTodoItem.Name = todoItem.Name;
    originalTodoItem.IsComplete = todoItem.IsComplete;
        
    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
    {
        System.Console.WriteLine(" return NotFound();");
        return NotFound();
    }
    return NoContent();
}


    
}
