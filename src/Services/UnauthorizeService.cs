using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Repos;

public class UnauthorizeService : IUnauthorize
{
    private readonly ServerContext context;
    readonly IMemoryCache memoryCache;

    public UnauthorizeService(ServerContext context, IMemoryCache memory)
    {
        this.context = context;
        this.memoryCache = memory;
    }

    public async Task<APIResponse> Auth(string email, string password)
    {
        var user = await context.Users.FirstOrDefaultAsync(u =>
            u.Email.ToLower() == email.ToLower() && u.Password == password
        );

        if (user != null)
        {
            user.Token = Guid.NewGuid().ToString();
            memoryCache.Set(user.Token, user);
            await context.SaveChangesAsync();
            return new APIResponse() { code = 1, message = user.Token };
        }
        return new APIResponse() { code = 0, message = "Неверный email или пароль" };
    }

    public async Task<APIResponse> Register(RegUserModel userModel)
    {
        var user = await context.Users.FirstOrDefaultAsync(u =>
            u.Email.ToLower() == userModel.email.ToLower()
        );

        if (user == null)
        {
            user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = userModel.email,
                Password = userModel.password,
                RoleId = 1,
                FullName = $"{userModel.surname} {userModel.name} {userModel.dadname}",
                Token = Guid.NewGuid().ToString(),
                TopicId = null
            };
            memoryCache.Set(user.Token, user);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return new APIResponse() { code = 1, message = user.Token };
        }
        return new APIResponse() { code = 0, message = "Пользователь с таким email уже есть!" };
    }
}
