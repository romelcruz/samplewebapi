using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebApi.Models;
using SampleWebApi.Validators;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly TrainingContext _context;
        public TrainingsController(TrainingContext context)
        {
            _context = context;
        }

        // GET: api/Trainings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainings()
        {
            return await _context.Trainings.ToListAsync();
        }

        // GET: api/Trainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Training>> GetTraining(int id)
        {
            var training = await _context.Trainings.FindAsync(id);

            if (training == null)
            {
                return NotFound();
            }

            return training;
        }

        // PUT: api/Trainings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(int id, Training training)
        {
            if (id != training.Id)
            {
                return BadRequest();
            }

            _context.Entry(training).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Trainings
        [HttpPost]
        public async Task<ActionResult<Training>> PostTraining(Training training)
        {
            ValidatorContext validatorContext = new ValidatorContext();
            validatorContext.AddValidators(new DateOrderValidator(training));

            if (validatorContext.ContainsError())
            {
               Response.Headers.Add("statusText", validatorContext.ErrorMessage);
               return BadRequest("Bad Request");
            }
 
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();
            Response.Headers.Add("statusText", "Resource Created");
            return CreatedAtAction("GetTraining", new { id = training.Id }, training);
        }

        // DELETE: api/Trainings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Training>> DeleteTraining(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            if (training == null)
            {
                Response.Headers.Add("statusText", "Resource not found");
                return NotFound("Resource not found");
            }

            _context.Trainings.Remove(training);
            await _context.SaveChangesAsync();
            Response.Headers.Add("statusText", "Resource Deleted");
            return training;
        }

        private bool TrainingExists(int id)
        {
            return _context.Trainings.Any(e => e.Id == id);
        }
    }
}
