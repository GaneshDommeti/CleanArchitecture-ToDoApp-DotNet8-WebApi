using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Services;
using TodoApp.Domain.Entities;

public  class ToDoItemFunction
{
    private  readonly IToDoItemService _toDoItemService;

    public  ToDoItemFunction(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
    }

    [Function("GetAllItems")]
    public async Task<HttpResponseData> GetAllItems(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todoitems")] HttpRequestData req)
    {
        var items = await _toDoItemService.GetAllItemsAsync();

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(items);
        return response;
    }

    [Function("GetItemById")]
    public async Task<HttpResponseData> GetItemById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todoitems/{id:int}")] HttpRequestData req,
        int id)
    {
        var item = await _toDoItemService.GetItemByIdAsync(id);

        if (item == null)
            return req.CreateResponse(HttpStatusCode.NotFound);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(item);
        return response;
    }

    [Function("CreateItem")]
    public async Task<HttpResponseData> CreateItem(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todoitems")] HttpRequestData req)
    {
        var item = await req.ReadFromJsonAsync<ToDoItem>();

        await _toDoItemService.AddItemAsync(item);

        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(item);
        return response;
    }

    [Function("UpdateItem")]
    public async Task<HttpResponseData> UpdateItem(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todoitems/{id:int}")] HttpRequestData req,
        int id)
    {
        var item = await req.ReadFromJsonAsync<ToDoItem>();

        if (id != item.ToDoItemId)
            return req.CreateResponse(HttpStatusCode.BadRequest);

        await _toDoItemService.UpdateItemAsync(item);

        return req.CreateResponse(HttpStatusCode.NoContent);
    }

    [Function("DeleteItem")]
    public async Task<HttpResponseData> DeleteItem(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todoitems/{id:int}")] HttpRequestData req,
        int id)
    {
        await _toDoItemService.DeleteItemAsync(id);

        return req.CreateResponse(HttpStatusCode.NoContent);
    }
}