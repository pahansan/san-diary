using SanDiaryApi.Models;

namespace SanDiaryApi.DTOs
{
    public class GetUserByIdRequest(int id)
    {
        public int Id { get; set; } = id;
    }

    public class GetAllUsersRequest()
    {
        // empty
    }

    public class GetUserByEmailRequest(string email)
    {
        public string Email { get; set; } = email;
    }

    public class DeleteUserByIdRequest(int id)
    {
        public int Id { get; set; } = id;
    }
}
