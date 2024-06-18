using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Repos;

public class AuthorizeUserService : IAuthorizeUser
{
    readonly ServerContext context;
    readonly IMemoryCache memoryCache;
    readonly IWebHostEnvironment appEnv;

    public AuthorizeUserService(
        ServerContext context,
        IMemoryCache memory,
        IWebHostEnvironment appEnvironment
    )
    {
        this.context = context;
        this.memoryCache = memory;
        appEnv = appEnvironment;
    }

    public async Task<APIResponse> RecordTask(string token, string taskId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.RoleId != 1)
            {
                return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
            }
            var userTask = await context.UserTasks.FirstOrDefaultAsync(ut =>
                ut.TaskId == taskId && ut.UserId == user.Id
            );

            if (userTask == null)
            {
                userTask = new UserTask()
                {
                    TaskId = taskId,
                    UserId = user.Id,
                    Status = 1,
                    Estimation = null
                };
                context.UserTasks.Add(userTask);

                await context.SaveChangesAsync();

                return new APIResponse() { code = 1, message = new { statusCode = 1 } };
            }
            return new APIResponse() { code = 0, message = "Пользователь уже выполнял задание" };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetLesson(string token, string lessonId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.TopicId != null)
            {
                var lesson = await context.Lessons.FindAsync(lessonId);
                if (lesson.TopicId == user.TopicId)
                {
                    return new APIResponse() { code = 1, message = lesson };
                }
                return new APIResponse()
                {
                    code = 0,
                    message = "Такой лекции нет в курсе пользователя"
                };
            }
            return new APIResponse() { code = 0, message = "Пользователь не проходит обучение" };
        }
        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetLessons(string token)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.TopicId == null)
            {
                return new APIResponse() { code = 0, message = "Сначала запишитесь на курсы" };
            }
            var lessons = await context.Lessons.Where(l => l.TopicId == user.TopicId).ToListAsync();
            var resLessons = new List<object>();

            foreach (var l in lessons)
            {
                resLessons.Add(
                    new
                    {
                        id = l.Id,
                        title = l.Tittle,
                        content = l.Content,
                        author = l.Autor,
                    }
                );
            }
            ;

            return new APIResponse()
            {
                code = 1,
                message = new
                {
                    topics = await context.Topics.FindAsync(user.TopicId),
                    lessons = resLessons
                }
            };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetSupplementsLesson(string token, string lessonId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.TopicId != null)
            {
                var lesson = await context.Lessons.FindAsync(lessonId);
                try
                {
                    if (lesson.TopicId == user.TopicId)
                    {
                        var supplements = await context
                            .SupplementLessons.Where(s => s.LessonId == lessonId)
                            .ToListAsync();

                        return new APIResponse() { code = 1, message = supplements };
                    }
                }
                catch { }
                return new APIResponse()
                {
                    code = 0,
                    message = "Такой лекции нет в курсе пользователя"
                };
            }
            return new APIResponse() { code = 0, message = "Пользователь не проходит обучение" };
        }
        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetSupplementsTask(string token, string lessonId, string taskId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.TopicId != null)
            {
                var lesson = await context.Lessons.FindAsync(lessonId);
                if (lesson.TopicId == user.TopicId)
                {
                    var task = await context
                        .LessonTasks.Where(lt => lt.LessonId == lessonId && lt.TaskId == taskId)
                        .ToListAsync();

                    if (task != null)
                    {
                        var supplements = await context
                            .SupplementTasks.Where(s => s.TaskId == taskId)
                            .ToListAsync();
                        return new APIResponse() { code = 1, message = supplements };
                    }
                    return new APIResponse()
                    {
                        code = 0,
                        message = "Такого задания нет в этой лекции"
                    };
                }
                return new APIResponse()
                {
                    code = 0,
                    message = "Такой лекции нет в курсе пользователя"
                };
            }
            return new APIResponse() { code = 0, message = "Пользователь не проходит обучение" };
        }
        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetTask(string token, string lessonId, string taskId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.TopicId != null)
            {
                var topic = await context.Topics.FindAsync(user.TopicId);

                var lesson = await context.Lessons.FirstOrDefaultAsync(l =>
                    l.Id == lessonId && l.TopicId == topic.Id
                );
                if (lesson != null)
                {
                    var lessonTask = await context.LessonTasks.FirstOrDefaultAsync(t =>
                        t.LessonId == lessonId && t.TaskId == taskId
                    );
                    if (lessonTask != null)
                    {
                        var task = await context.Tasks.FindAsync(taskId);
                        int? statusCode = 0;
                        string? estimation = null;
                        var userTask = await context.UserTasks.FirstOrDefaultAsync(ut =>
                            ut.TaskId == taskId && ut.UserId == user.Id
                        );
                        if (userTask != null)
                        {
                            statusCode = userTask.Status;
                            estimation = userTask.Estimation;
                        }
                        return new APIResponse()
                        {
                            code = 1,
                            message = new
                            {
                                id = task.Id,
                                content = task.Content,
                                tittle = task.Tittle,
                                difficalty = task.Difficalty,
                                time = task.Time,
                                statusCode = statusCode,
                                estimation = estimation
                            }
                        };
                    }
                    return new APIResponse()
                    {
                        code = 0,
                        message = "Такого задания нет в этой лекции"
                    };
                }
                return new APIResponse() { code = 0, message = "Такой лекции нет в этом курсе" };
            }

            return new APIResponse() { code = 0, message = "Пользователь не проходит обучение" };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetTasks(string token, string lessonId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (await context.Lessons.FirstOrDefaultAsync(l => l.TopicId == user.TopicId) != null)
            {
                var lessonTasks = await context
                    .LessonTasks.Where(t => t.LessonId == lessonId)
                    .ToListAsync();

                if (lessonTasks.Count > 0)
                {
                    var resTasks = new List<object>();
                    foreach (var t in lessonTasks)
                    {
                        var task = await context.Tasks.FindAsync(t.TaskId);
                        var usersTasks = await context.UserTasks.FirstOrDefaultAsync(ut =>
                            ut.TaskId == t.TaskId
                        );
                        var estimation = "";
                        int? statusCode = 0;

                        if (usersTasks != null)
                        {
                            estimation = usersTasks.Estimation;
                            statusCode = usersTasks.Status;
                        }

                        resTasks.Add(
                            new
                            {
                                id = task.Id,
                                content = task.Content,
                                tittle = task.Tittle,
                                dificalty = task.Difficalty,
                                time = task.Time,
                                lessonId = t.LessonId,
                                mark = estimation,
                                statusCode = statusCode
                            }
                        );
                    }

                    return new APIResponse() { code = 1, message = new { tasks = resTasks } };
                }

                return new APIResponse() { code = 0, message = "В данной лекции нету заданий" };
            }

            return new APIResponse() { code = 0, message = "Этой лекции нету в выбранном курсе" };
        }
        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetTopics(string token)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            var topics = await context.Topics.ToListAsync();
            var userResult = await context.Results.Where(r => r.UserId == user.Id).ToListAsync();
            var resTopics = new List<object>();
            foreach (var t in topics)
            {
                if (userResult.FirstOrDefault(x => x.TopicId == t.Id) != null)
                {
                    continue;
                }
                var lessonInTopic = await context
                    .Lessons.Where(l => l.TopicId == t.Id)
                    .ToListAsync();

                resTopics.Add(
                    new
                    {
                        code = t.Id,
                        title = t.Tittle,
                        description = t.Description,
                        lesson = lessonInTopic,
                    }
                );
            }

            return new APIResponse() { code = 1, message = resTopics };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetUser(string token)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            var authUser = await context.Users.FindAsync(user.Id);
            return new APIResponse()
            {
                code = 1,
                message = new
                {
                    fullname = authUser.FullName,
                    email = authUser.Email,
                    password = authUser.Password,
                    idRole = authUser.RoleId,
                    topicId = authUser.TopicId
                }
            };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> Logout(string token)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            memoryCache.Remove(token);
            await context.SaveChangesAsync();

            return new APIResponse() { code = 1, message = "Успех" };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> RecordTopic(string token, string topicId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.RoleId != 1)
            {
                return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
            }
            if (await context.Topics.FindAsync(topicId) != null)
            {
                if (user.TopicId == null)
                {
                    var result = await context.Results.FirstOrDefaultAsync(r =>
                        r.UserId == user.Id && r.TopicId == topicId
                    );
                    if (result == null)
                    {
                        var authUser = await context.Users.FindAsync(user.Id);
                        authUser.TopicId = topicId;
                        await context.SaveChangesAsync();
                        memoryCache.Set(token, authUser);
                        return new APIResponse()
                        {
                            code = 1,
                            message = "Пользователь успешно записался"
                        };
                    }
                    return new APIResponse()
                    {
                        code = 0,
                        message = "Пользователь уже прошел обучение"
                    };
                }
                return new APIResponse()
                {
                    code = 0,
                    message = "Пользователь уже проходит обучение"
                };
            }
            return new APIResponse() { code = 0, message = "Такого курса нет" };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> SetFile(string token, string taskId, IFormFile file)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.RoleId != 1)
            {
                return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
            }
            if (file != null)
            {
                var task = await context.UserTasks.FirstOrDefaultAsync();
                if (task != null)
                {
                    if (task.Status != 1)
                    {
                        Directory.CreateDirectory(appEnv.WebRootPath + $"/{user.Id}");
                        var filepath = $"/{user.Id}/{taskId}.png";
                        using (
                            var fs = new FileStream(
                                appEnv.WebRootPath + filepath,
                                FileMode.OpenOrCreate
                            )
                        )
                        {
                            await file.CopyToAsync(fs);
                        }
                        task.Status = 2;
                        await context.SaveChangesAsync();
                        return new APIResponse() { code = 1, message = "Успешно добавлено" };
                    }

                    return new APIResponse()
                    {
                        code = 0,
                        message = "В данный момент прикрепить файлы нельзя"
                    };
                }
                return new APIResponse() { code = 0, message = "Пользователь не брал задание" };
            }
            return new APIResponse() { code = 0, message = "Файл не пришел" };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    // public async Task<APIResponse> GenerateDiplom(string token, string userId)
    // {
    //     var user = memoryCache.Get<User>(token);

    //     if (user != null)
    //     {
    //         if (user.RoleId != 2)
    //         {
    //             return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
    //         }
    //         var student = await context.Users.FindAsync(userId);
    //         if (student != null)
    //         {
    //             var lessons = await context
    //                 .Lessons.Where(l => l.TopicId == student.TopicId)
    //                 .ToListAsync();
    //             var userTasks = await context
    //                 .UserTasks.Where(ut => ut.UserId == userId)
    //                 .ToListAsync();

    //             var tasks = new List<Server.Repos.Task>();

    //             lessons.ForEach(async l =>
    //             {
    //                 tasks.AddRange(
    //                     await context.Tasks.Where(t => t.LessonId == l.Id).ToListAsync()
    //                 );
    //             });
    //         }
    //         return new APIResponse() { code = 0, message = "Такого студента нет" };
    //     }
    //     return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    // }

    public async Task<APIResponse> GetUncheckedUser(string token, string taskId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.RoleId != 2)
            {
                return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
            }

            var userTasks = await context
                .UserTasks.Where(ut => ut.Status == 2 && ut.TaskId == taskId)
                .ToListAsync();

            var users = new List<object>();

            userTasks.ForEach(async ut =>
            {
                var uncheckUser = await context.Users.FindAsync(ut.UserId);

                users.Add(
                    new
                    {
                        fullname = uncheckUser.FullName,
                        email = uncheckUser.Email,
                        password = uncheckUser.Password,
                        idRole = uncheckUser.RoleId,
                        topicId = uncheckUser.TopicId
                    }
                );
            });
            return new APIResponse() { code = 1, message = users };
        }

        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }

    public async Task<APIResponse> GetUncheckedTask(string token, string taskId, string userId)
    {
        var user = memoryCache.Get<User>(token);

        if (user != null)
        {
            if (user.RoleId != 2)
            {
                return new APIResponse() { code = 0, message = "Ошибка отказано в доступе" };
            }

            // var userTask =
        }
        return new APIResponse() { code = 0, message = "Пользователь не авторизован" };
    }
}
