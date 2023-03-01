using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using westcoast_education.web.Models;
using westcoast_education.web.ViewModels;
using System.Net.Mime;
using westcoast_education.web.ViewModels.Course;

namespace westcoast_education.web.Controllers;

[Route("courses")]
public class CoursesController : Controller
{
    private readonly IConfiguration _config;
    private readonly string? _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public CoursesController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<IActionResult> Index()
    {

        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses");
        if (!response.IsSuccessStatusCode) return Content("Oops gick fel!");

        var json = await response.Content.ReadAsStringAsync();

        var courses = JsonSerializer.Deserialize<IList<CourseListViewModel>>(json, _options);

        return View("Index", courses);
    }

    [HttpGet("details/{courseId}")]
    public async Task<IActionResult> Details(Guid courseId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/courses/getbyid/{courseId}");

        if (!response.IsSuccessStatusCode) return Content("Oops gick fel");

        var json = await response.Content.ReadAsStringAsync();
        var course = JsonSerializer.Deserialize<CourseDetailsViewModel>(json, _options);

        return View("Details", course);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new CoursePostViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CoursePostViewModel model)
    {
        if (!ModelState.IsValid) return View("Create", model);

        var course = new CoursePostViewModel
        {
            CourseName = model.CourseName,
            CourseTitle = model.CourseTitle,
            StartDate = model.StartDate,
            CourseLength = model.CourseLength,
            OnSite = model.OnSite,
            CoursePoints = model.CoursePoints
        };

        var json = JsonSerializer.Serialize(course);

        using var client = _httpClient.CreateClient();
        var response = await client.PostAsync($"{_baseUrl}/courses", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json));

        if (response.IsSuccessStatusCode) return RedirectToAction("Index");

        ModelState.AddModelError("", "Failed to create the course.");
        return View("Create");
    }

    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.DeleteAsync($"{_baseUrl}/courses/{id}");

        if (response.IsSuccessStatusCode) return RedirectToAction("Index");

        return Content("Oops gick fel!");
    }

}
