using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

public class UserFunction
{
    private readonly IUserService _userService;

    public UserFunction(IUserService userService)
    {
        _userService = userService;
    }

    [Function("CreateUser")]
    public async Task<HttpResponseData> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequestData req)
    {
        var user = await req.ReadFromJsonAsync<User>();

        await _userService.AddUserAsync(user);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(user);
        return response;
    }

    [Function("GetUser")]
    public async Task<HttpResponseData> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{id:int}")] HttpRequestData req,
        int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return req.CreateResponse(HttpStatusCode.NotFound);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(user);
        return response;
    }

    [Function("GetAllUsers")]
    public async Task<HttpResponseData> GetAllUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")] HttpRequestData req)
    {
        var users = await _userService.GetAllUsersAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(users);
        return response;
    }
}