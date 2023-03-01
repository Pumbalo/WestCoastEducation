using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Models;
using westcoast_education.api.ViewModels;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly WestcoastEducationsContext _context;
        public TeacherController(WestcoastEducationsContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _context.Teachers
                .Select(t => new
                {
                    TeacherId = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    Adress = t.Adress,
                    ZipCode = t.ZipCode,
                    City = t.City,
                    ProfileAvatar = t.ProfileAvatar,
                    Title = t.Title
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Teachers
                .Select(t => new
                {
                    TeacherId = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    PhoneNumber = t.PhoneNumber,
                    Adress = t.Adress,
                    ZipCode = t.ZipCode,
                    City = t.City,
                    ProfileAvatar = t.ProfileAvatar,
                    Title = t.Title,
                    Courses = t.Course!.Select(c => new
                    {
                        CourseId = c.CourseId,
                        CourseName = c.CourseName,
                        CourseTitle = c.CourseTitle,
                        StartDate = c.StartDate,
                        CourseLength = c.CourseLength,
                        OnSite = c.OnSite,
                        CoursePoints = c.CoursePoints,
                    }).ToList(),
                    Competence = t.Competence!.Select(s => new
                    {
                        CompetenceId = s.Id,
                        CompetenceName = s.CompetenceName
                    }).ToList()
                })
                .SingleOrDefaultAsync(c => c.TeacherId == id);

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(TeacherPostViewModel model)
        {
            var exists = await _context.Teachers.SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToUpper().Trim());

            if (exists is not null) return BadRequest($"A teacher with the email of {model.Email} already exist in the system");

            var teacher = new TeacherModel
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Adress = model.Adress,
                City = model.City,
                ZipCode = model.ZipCode
            };

            await _context.Teachers.AddAsync(teacher);

            foreach (var competence in model.Competence)
            {
                var s = await _context.Competence.SingleOrDefaultAsync(c => c.CompetenceName!.ToLower().Trim() == competence.ToLower().Trim());
                if (s is null)
                {
                    s = new CompetenceModel
                    {
                        CompetenceName = competence.Trim()
                    };
                    await _context.Competence.AddAsync(s);
                }
                if (teacher.Competence is null) teacher.Competence = new List<CompetenceModel>();
                teacher.Competence.Add(s);
            }


            if (await _context.SaveChangesAsync() > 0)
            {
                return StatusCode(201);
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addcompetence/{teacherId}/{competenceId}")]
        public async Task<IActionResult> AddSkill(Guid teacherId, Guid competenceId)
        {
            var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Id == teacherId);
            if (teacher is null) return NotFound("The teacher could not be found");

            var competence = await _context.Competence.SingleOrDefaultAsync(c => c.Id == competenceId);
            if (competence is null) return NotFound("The competence could not be found");

            if (teacher.Competence is null) teacher.Competence = new List<CompetenceModel>();

            teacher.Competence.Add(competence);

            try
            {
                _context.Teachers.Update(teacher);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ErrorMessage", ex.Message + " " + ex.InnerException != null ? ex.InnerException!.Message : "");
                return ValidationProblem();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var teacher = await _context.Teachers.FirstOrDefaultAsync(c => c.Id == id);
            if (teacher is null) return BadRequest("Teacher could not be found");

            _context.Teachers.Update(teacher);
            _context.Teachers.Remove(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Teacher has been removed");
            }
            return BadRequest("An issue occured when removing a teacher");
        }

        [HttpGet("competence/listall")]
        public async Task<IActionResult> ListCompetence()
        {
            var result = await _context.Competence
                .Select(c => new
                {
                    CompetenceId = c.Id,
                    CompetenceName = c.CompetenceName
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpDelete("competence/{id}")]
        public async Task<ActionResult> DeleteCompetence(Guid id)
        {
            var competence = await _context.Competence.FirstOrDefaultAsync(c => c.Id == id);
            if (competence is null) return BadRequest("Competence could not be found");

            _context.Competence.Update(competence);
            _context.Competence.Remove(competence);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Competence has been removed");
            }
            return BadRequest("An issue occured when removing a competence");
        }

        [HttpPatch("removecompetence/{teacherId}/{competenceId}")]
        public async Task<ActionResult> RemoveCompetence(Guid teacherId, Guid competenceId)
        {
            var competence = await _context.Competence.FindAsync(competenceId);
            if (competence is null) return NotFound($"Competence {competenceId} could not be found");

            var teacher = await _context.Teachers.Include(t => t.Competence).FirstOrDefaultAsync(t => t.Id == teacherId); ;
            if (teacher is null) return NotFound($"Teacher {teacherId} could not be found");

            competence.Teacher?.Remove(teacher);
            _context.Competence.Update(competence);
            _context.Teachers.Update(teacher);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Competence from teacher has been removed");
            }
            return BadRequest("An issue occured when removing a competence from a teacher");
        }
    }
}