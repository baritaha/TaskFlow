using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITodoRepository repository, ILogger<TodoController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var todoItems = await _repository.GetAllAsync();
            var responseList = todoItems.Select(item => new TodoResponseDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                IsCompleted = item.IsCompleted
            }).ToList();
            return Ok(responseList);
        }
        [HttpGet("{id}", Name = "GetByIdAsync")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var todoItem = await _repository.GetByIdAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }
            var response = new TodoResponseDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> AddAsync(CreateTodoDto item)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newItem = new TodoItem
                {
                    Title = item.Title,
                    Description = item.Description,
                    
                };
                await _repository.AddAsync(newItem);
                /*  return Ok(newItem);*/
                var response = new TodoResponseDto
                {
                    Id = newItem.Id,
                    Title = newItem.Title,
                    Description = newItem.Description,
                    IsCompleted = newItem.IsCompleted
                };
                return CreatedAtRoute(nameof(GetByIdAsync), new { id = newItem.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying add item");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new todo item");
            }
          
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateTodoDto todoDto)
        {
           
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                existingItem.Title = todoDto.Title;
                existingItem.Description = todoDto.Description;
                existingItem.IsCompleted = todoDto.IsCompleted;

                await _repository.UpdateAsync(existingItem);

                var response = new TodoResponseDto
                {
                    Id = existingItem.Id,
                    Title = existingItem.Title,
                    Description = existingItem.Description,
                    IsCompleted = existingItem.IsCompleted
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying update item related ID : {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating todo item");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingItem = await _repository.GetByIdAsync(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while trying delete item related ID : {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting todo item");
            }

        }
    }
}
