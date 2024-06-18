public interface IAuthorizeUser
{
    public Task<APIResponse> Logout(string token);
    public Task<APIResponse> GetUser(string token);
    public Task<APIResponse> GetLessons(string token);
    public Task<APIResponse> GetLesson(string token, string lessonId);
    public Task<APIResponse> GetTopics(string token);
    public Task<APIResponse> RecordTopic(string token, string topicId);
    public Task<APIResponse> GetTasks(string token, string lessonId);
    public Task<APIResponse> GetTask(string token, string lessonId, string taskId);
    public Task<APIResponse> GetSupplementsLesson(string token, string lessonId);
    public Task<APIResponse> GetSupplementsTask(string token, string lessonId, string taskId);
    public Task<APIResponse> SetFile(string token, string taskId, IFormFile file);
    public Task<APIResponse> RecordTask(string token, string taskId);

    public Task<APIResponse> GetUncheckedUser(string token, string taskId);
    public Task<APIResponse> GetUncheckedTask(string token, string taskId, string userId);
    // public Task<APIResponse> GenerateDiplom(string token, string userId);
}
