using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wescoast_education.api.ViewModels.Students;
using westcoast_education.api.Data;
using westcoast_education.api.Models;

namespace wescoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/students")]
    public class StudentsController : ControllerBase
    {
        private readonly WestcoastEducationsContext _context;
        public StudentsController(WestcoastEducationsContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<IActionResult> List()
        {
            var result = await _context.Students
                .Select(s => new
                {
                    StudentId = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Adress = s.Adress,
                    ZipCode = s.ZipCode,
                    City = s.City,
                    ProfileAvatar = s.ProfileAvatar,
                    Title = s.Title
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _context.Students
                .Select(s => new
                {
                    StudentId = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    Adress = s.Adress,
                    ZipCode = s.ZipCode,
                    City = s.City,
                    ProfileAvatar = s.ProfileAvatar,
                    Title = s.Title,
                    Courses = s.StudentCourses!.Select(c => new
                    {
                        CourseId = c.CourseId,
                        CourseName = c.Course!.CourseName,
                        CourseTitle = c.Course!.CourseTitle,
                        StartDate = c.Course!.StartDate,
                        CourseLength = c.Course!.CourseLength,
                        OnSite = c.Course!.OnSite,
                        CoursePoints = c.Course!.CoursePoints,
                        Status = ((CourseStatusEnum)c.Status).ToString()
                    }).ToList(),
                })
                .SingleOrDefaultAsync(c => c.StudentId == id);

            return Ok(result);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(StudentPostViewModel model)
        {
            var exists = await _context.Students.SingleOrDefaultAsync(c => c.Email.ToLower().Trim() == model.Email.ToUpper().Trim());

            if (exists is not null) return BadRequest($"A student with the email of {model.Email} already exist in the system");

            var student = new StudentModel
            {
                Id = Guid.NewGuid(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Adress = model.Adress,
                City = model.City
            };

            await _context.Students.AddAsync(student);

            foreach (var item in model.Courses)
            {
                var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == item);

                if (course is not null)
                {
                    if (student.StudentCourses is null) student.StudentCourses = new List<StudentCourse>();
                    student.StudentCourses.Add(new StudentCourse { Course = course, Student = student, Status = CourseStatusEnum.NoStatus });
                }
            }


            if (await _context.SaveChangesAsync() > 0)
            {
                return StatusCode(201);
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addcourse/{studentId}/{courseId}")]
        public async Task<IActionResult> AddStudentToCourse(Guid studentId, Guid courseId)
        {

            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);
            if (student is null) return NotFound("The student could not be found");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            if (course is null) return NotFound("The course could not be found");

            await _context.StudentCourse.AddAsync(new StudentCourse { Course = course, Student = student, Status = CourseStatusEnum.NoStatus });

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("changestatus/{studentId}/{courseId}/{status}")]
        public async Task<IActionResult> ChangeStatus(Guid studentId, Guid courseId, CourseStatusEnum status)
        {

            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);
            if (student is null) return NotFound("The student could not be found");

            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            if (course is null) return NotFound("The course could not be found");

            var studentCourse = await _context.StudentCourse.SingleOrDefaultAsync(c => c.StudentId == studentId && c.CourseId == courseId);
            if (studentCourse is null) return NotFound("The course for the selected student could not be found");

            studentCourse.Status = status;

            _context.StudentCourse.Update(studentCourse);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var student = await _context.Students.FirstOrDefaultAsync(c => c.Id == id);
            if (student is null) return BadRequest("Student could not be found");

            _context.Students.Update(student);
            _context.Students.Remove(student);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Student has been removed");
            }
            return BadRequest("An issue occured when removing a student");
        }
    }
}