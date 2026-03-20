using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

public class ToDoListFunction
{
    private readonly IToDoListService _toDoListService;

    public ToDoListFunction(IToDoListService toDoListService)
    {
        _toDoListService = toDoListService;
    }

    [Function("CreateToDoList")]
    public async Task<HttpResponseData> CreateToDoList(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todolists")] HttpRequestData req)
    {
        var list = await req.ReadFromJsonAsync<ToDoList>();

        await _toDoListService.AddListAsync(list);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(list);
        return response;
    }

    [Function("GetToDoList")]
    public async Task<HttpResponseData> GetToDoList(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todolists/{id:int}")] HttpRequestData req,
        int id)
    {
        var list = await _toDoListService.GetListByIdAsync(id);

        if (list == null)
            return req.CreateResponse(HttpStatusCode.NotFound);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(list);
        return response;
    }
}