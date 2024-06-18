
    public interface IUnauthorize{
        public Task<APIResponse> Auth(string email, string password);
        public Task<APIResponse> Register(RegUserModel userModel);
    }
    