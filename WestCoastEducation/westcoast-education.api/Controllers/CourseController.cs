using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.api.Data;
using westcoast_education.api.Models;
using westcoast_education.api.ViewModels;

namespace westcoast_education.api.Controllers
{
    [ApiController]
    [Route("api/v1/courses")]
    [Produces("application/json")]
    public class CourseController : Controller
    {
        private readonly WestcoastEducationsContext _context;
        public CourseController(WestcoastEducationsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Listar alla kurser i systemet
        /// </summary>
        [HttpGet()]
        public async Task<ActionResult> ListAll()
        {
            var result = await _context.Courses
            .Select(
                c => new
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseTitle = c.CourseTitle,
                    StartDate = c.StartDate,
                    CourseLength = c.CourseLength,
                    OnSite = c.OnSite,
                    CoursePoints = c.CoursePoints,
                    Teacher = c.Teacher != null ? c.Teacher.FirstName + " " + c.Teacher.LastName : "Saknas",
                    Students = c.StudentCourse!.Select(s => new
                    {
                        StudentId = s.StudentId,
                        name = s.Student!.FirstName + " " + s.Student.LastName
                    }).ToList()
                }
            ).ToListAsync();
            return Ok(result);
        }

        /// <summary>
        /// Hämtar en kurs baserad på kurs id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Kurs information om sökt kurs och dess lärare samt studenter
        /// </returns>
        /// <response code="200">Returnerar en kurs med information om sökt kurs och dess lärare samt studenter</response>
        [HttpGet("getbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetById(Guid id)
        {
            var result = await _context.Courses
            .Select(
                c => new
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseTitle = c.CourseTitle,
                    StartDate = c.StartDate,
                    CourseLength = c.CourseLength,
                    OnSite = c.OnSite,
                    CoursePoints = c.CoursePoints,
                    Teachers = c.Teacher != null ? new
                    {
                        teacherId = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName,
                        Email = c.Teacher.Email,
                        PhoneNumber = c.Teacher.PhoneNumber,
                        Adress = c.Teacher.Adress,
                        ZipCode = c.Teacher.ZipCode,
                        City = c.Teacher.City,
                        ProfileAvatar = c.Teacher.ProfileAvatar,
                    } : null,
                    Students = c.StudentCourse!.Select(s => new
                    {
                        studentId = s.Student!.Id,
                        FirstName = s.Student.FirstName,
                        LastName = s.Student.LastName,
                        Email = s.Student.Email,
                        PhoneNumber = s.Student.PhoneNumber,
                        Adress = s.Student.Adress,
                        ZipCode = s.Student.ZipCode,
                        City = s.Student.City,
                        ProfileAvatar = s.Student.ProfileAvatar,
                        Status = ((CourseStatusEnum)s.Status).ToString()
                    })
                }
            ).SingleOrDefaultAsync(c => c.CourseId == id);
            return Ok(result);
        }

        [HttpGet("getbycoursename/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetByCourseName(string name)
        {
            var result = await _context.Courses
            .Select(
                c => new
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseTitle = c.CourseTitle,
                    StartDate = c.StartDate,
                    CourseLength = c.CourseLength,
                    OnSite = c.OnSite,
                    CoursePoints = c.CoursePoints,
                    Teacher = c.Teacher != null ? new
                    {
                        teacherId = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName,
                        Email = c.Teacher.Email,
                        PhoneNumber = c.Teacher.PhoneNumber
                    } : null,
                    Students = c.StudentCourse!.Select(s => new
                    {
                        studentId = s.Student!.Id,
                        FirstName = s.Student.FirstName,
                        LastName = s.Student.LastName,
                        Email = s.Student.Email,
                        PhoneNumber = s.Student.PhoneNumber,
                        Status = ((CourseStatusEnum)s.Status).ToString()
                    })
                }
            ).SingleOrDefaultAsync(c => c.CourseName.Trim().ToLower() == name.Trim().ToLower());
            return Ok(result);
        }

        [HttpGet("getbystartdate/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetByStartDate(string date)
        {
            var result = await _context.Courses
            .Select(
                c => new
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseTitle = c.CourseTitle,
                    StartDate = c.StartDate,
                    CourseLength = c.CourseLength,
                    OnSite = c.OnSite,
                    CoursePoints = c.CoursePoints,
                    Teacher = c.Teacher != null ? new
                    {
                        teacherId = c.Teacher.Id,
                        FirstName = c.Teacher.FirstName,
                        LastName = c.Teacher.LastName,
                        Email = c.Teacher.Email,
                        PhoneNumber = c.Teacher.PhoneNumber
                    } : null,
                    Students = c.StudentCourse!.Select(s => new
                    {
                        studentId = s.Student!.Id,
                        FirstName = s.Student.FirstName,
                        LastName = s.Student.LastName,
                        Email = s.Student.Email,
                        PhoneNumber = s.Student.PhoneNumber,
                        Status = ((CourseStatusEnum)s.Status).ToString()
                    })
                }
            ).SingleOrDefaultAsync(c => c.StartDate.Trim().ToLower() == date.Trim().ToLower());
            return Ok(result);
        }

        /// <summary>
        /// Skapar och lägger till en ny kurs i systemet
        /// </summary>
        /// <param name="model"></param>
        /// <returns>En länk till den nya kursen och ett object med kursens information</returns>
        /// <remarks>
        /// Sample Request:
        ///
        /// POST /api/v1/courses
        /// {
        ///     "title": "Kurstitle",
        ///     "courseNumber": 12345,
        ///     "duration": 5,
        ///     "startDate": "2023-09-11"
        /// }
        /// </remarks>
        /// <response code="201">Returnerar den tillagda kursen</response>
        /// <response code="400">Om kursen redan existerar eller om det saknas information i anropet</response>
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add(CoursePostViewModel model)
        {
            var exists = await _context.Courses.SingleOrDefaultAsync(
                c => c.CourseName == model.CourseName &&
                c.StartDate == model.StartDate
            );

            if (exists is not null) return BadRequest($"Course with the name of {model.CourseName} that starts on {model.StartDate} already exists!");

            var course = new CourseModel
            {
                CourseId = Guid.NewGuid(),
                CourseName = model.CourseName,
                CourseTitle = model.CourseTitle,
                StartDate = model.StartDate,
                CourseLength = model.CourseLength,
                OnSite = model.OnSite,
                CoursePoints = model.CoursePoints
            };

            await _context.Courses.AddAsync(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                var result = new
                {
                    CourseId = course.CourseId,
                    CourseTitle = course.CourseTitle,
                    StartDate = course.StartDate
                };
                return CreatedAtAction(nameof(GetById), new { Id = course.CourseId }, result);
            }
            return StatusCode(500, "Internal Server Error");
        }

        /// <summary>
        /// Lägger tille n ny student till en befintlig kurs.
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="studentId"></param>
        /// <returns>Inget</returns>
        /// <response code="204"></response>
        /// <response code="404">Om kurs eller student inte finns i systemet</response>
        [HttpPatch("addstudent/{courseId}/{studentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> AddStudent(Guid courseId, Guid studentId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);

            if (course is null) return NotFound("Couldn't find the course");
            if (student is null) return NotFound("Couldn't find the student");

            var studentCourse = new StudentCourse
            {
                Course = course,
                Student = student
            };

            await _context.StudentCourse.AddAsync(studentCourse);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("removestudent/{courseId}/{studentId}")]
        public async Task<ActionResult> RemoveStudent(Guid courseId, Guid studentId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            var student = await _context.Students.SingleOrDefaultAsync(c => c.Id == studentId);

            if (course is null) return NotFound("Couldn't find the course");
            if (student is null) return NotFound("Couldn't find the student");

            var studentCourse = await _context.StudentCourse.SingleOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == studentId);

            if (studentCourse is not null) _context.StudentCourse.Remove(studentCourse);

            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpPatch("addteacher/{courseId}/{teacherId}")]
        public async Task<ActionResult> AddTeacher(Guid courseId, Guid teacherId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);
            var teacher = await _context.Teachers.SingleOrDefaultAsync(c => c.Id == teacherId);

            if (course is null) return NotFound("Couldn't find the course");
            if (teacher is null) return NotFound("Couldn't find the teacher");

            course.TeacherId = teacher.Id;


            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }
            return StatusCode(500, "Internal Server Error");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);
            if (course is null) return BadRequest("Course could not be found");

            _context.Courses.Update(course);
            _context.Courses.Remove(course);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Course has been removed");
            }
            return BadRequest("An issue occured when removing a course");
        }

        [HttpPatch("removeteacher/{courseId}")]
        public async Task<ActionResult> RemoveTeacher(Guid courseId)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseId == courseId);

            if (course is null) return NotFound("Couldn't find the course");

            course.TeacherId = null;

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Teacher has been removed");
            }
            return BadRequest("An issue occured when removing the teacher from the course");
        }
    }
}