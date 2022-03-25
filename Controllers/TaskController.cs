using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todolist.Context;

namespace todolist.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {

        private readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        {
            this._context = context;
        }


        [HttpGet(Name = "GetTask")]
        public IEnumerable<todolist.Models.Task> Get()
        {
            return this._context.Tasks.ToList();
        }

        [HttpPost]
        public IActionResult Post([FromBody] todolist.Models.Task task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Tasks.Add(task);
                    _context.SaveChanges();
                    return Ok(new { message = "task create!." });
                }

                throw new Exception("invalid fields");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] todolist.Models.Task task)
        {
            try
            {
                if (id != task.Id)
                {
                    return NotFound();
                }

                task.Status = !task.Status;
                _context.Entry(task).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new { message = "task updated!!!" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var task = _context.Tasks.Find(id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                _context.SaveChanges();

                return Ok(new { message = " task was deleted!." });

            }
            catch (System.Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}