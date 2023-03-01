using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using westcoast_education.web.ViewModels;
using westcoast_education.web.ViewModels.Teacher;

namespace westcoast_education.web.Controllers;

[Route("teachers")]
public class TeachersController : Controller
{
    private readonly IConfiguration _config;
    private readonly string? _baseUrl;
    private readonly JsonSerializerOptions _options;
    private readonly IHttpClientFactory _httpClient;

    public TeachersController(IConfiguration config, IHttpClientFactory httpClient)
    {
        _httpClient = httpClient;
        _config = config;
        _baseUrl = _config.GetSection("apiSettings:baseUrl").Value;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<IActionResult> Index()
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers");
        if (!response.IsSuccessStatusCode) return Content("Oops gick fel!");

        var json = await response.Content.ReadAsStringAsync();

        var teacher = JsonSerializer.Deserialize<IList<TeacherListViewModel>>(json, _options);

        return View("Index", teacher);
    }

    [HttpGet("details/{teacherId}")]
    public async Task<IActionResult> Details(Guid teacherId)
    {
        using var client = _httpClient.CreateClient();
        var response = await client.GetAsync($"{_baseUrl}/teachers/getbyid/{teacherId}");

        if (!response.IsSuccessStatusCode) return Content("Oops gick fel");

        var json = await response.Content.ReadAsStringAsync();
        var teacher = JsonSerializer.Deserialize<TeacherDetailsViewModel>(json, _options);

        return View("Details", teacher);
    }
}
