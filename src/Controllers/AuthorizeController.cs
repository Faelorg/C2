using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("{token}")]
public class AuthorizeController : ControllerBase
{
    readonly IAuthorizeUser service;

    public AuthorizeController(IAuthorizeUser service)
    {
        this.service = service;
    }

    [HttpGet("tasks/{taskId}")]
    public async Task<ActionResult> RecordTask([FromRoute] string token, string taskId)
    {
        var response = await service.RecordTask(token, taskId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpPost("tasks/{taskId}/file")]
    public async Task<ActionResult> SetFile([FromRoute] string token, string taskId, IFormFile file)
    {
        var response = await service.SetFile(token, taskId, file);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons/{lessonId}/tasks/{taskId}")]
    public async Task<ActionResult> GetTask(
        [FromRoute] string token,
        string lessonId,
        string taskId
    )
    {
        var response = await service.GetTask(token, lessonId, taskId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons/{lessonId}/supplements")]
    public async Task<ActionResult> GetLessonSupplements([FromRoute] string token, string lessonId)
    {
        var response = await service.GetSupplementsLesson(token, lessonId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons/{lessonId}/tasks/{taskId}/supplements")]
    public async Task<ActionResult> GetTaskSupplements(
        [FromRoute] string token,
        string lessonId,
        string taskId
    )
    {
        var response = await service.GetSupplementsTask(token, lessonId, taskId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons/{lessonId}")]
    public async Task<ActionResult> GetLesson([FromRoute] string token, string lessonId)
    {
        var response = await service.GetLesson(token, lessonId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons/{lessonId}/tasks")]
    public async Task<ActionResult> GetTasks([FromRoute] string token, string lessonId)
    {
        var response = await service.GetTasks(token, lessonId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("topics/{topicId}")]
    public async Task<ActionResult> RecordTopic([FromRoute] string token, string topicId)
    {
        var response = await service.RecordTopic(token, topicId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("user")]
    public async Task<ActionResult> GetUser([FromRoute] string token)
    {
        var response = await service.GetUser(token);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("topics")]
    public async Task<ActionResult> GetTopics([FromRoute] string token)
    {
        var response = await service.GetTopics(token);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("lessons")]
    public async Task<ActionResult> GetLessons([FromRoute] string token)
    {
        var response = await service.GetLessons(token);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }

    [HttpGet("logout")]
    public async Task<ActionResult> LogOut([FromRoute] string token)
    {
        return Ok(await service.Logout(token));
    }

    // [HttpGet("teacher/{userId}/diplom/new")]
    // public async Task<ActionResult> GenerateDiplom([FromRoute] string token, string userId)
    // {
    //     var response = await service.GenerateDiplom(token, userId);

    //     if (response.code == 1)
    //     {
    //         return Ok(response.message);
    //     }

    //     return Unauthorized(response.message);
    // }

    [HttpGet("teacher/uncheked/{taskId}")]
    public async Task<ActionResult> GetUnchekedUser([FromRoute] string token, string taskId)
    {
        var response = await service.GetUncheckedUser(token, taskId);

        if (response.code == 1)
        {
            return Ok(response.message);
        }

        return Unauthorized(response.message);
    }
}
