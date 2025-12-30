using Backend.DTOs;
using Backend.Entity;
using Backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class UserService : IUserService
    {
        private static readonly List<User> _users = new();

        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }


        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var result = _context.Users.Select(
                x => new UserResponse
                {
                    UUid = x.UUid,
                    Role = x.Role,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                });

            return await result.ToListAsync();
        }

        public async Task<UserResponse?> GetByIdAsync(Guid uid)
        {
            var result = await _context.Users
                        .Select(x => new UserResponse
                        {
                            UUid = x.UUid,
                            Role = x.Role,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email
                        }).FirstOrDefaultAsync(u => u.UUid == uid);
            return result;
        }

        public async Task<User?> GetByEmailAsync(string Email)
        {
            var result = await _context.Users
                        .Select(x => x).FirstOrDefaultAsync(u => u.Email == Email);
            return result;
        }

        public async Task<User?> GetByUUIDAsync(Guid uuid)
        {
            var result = await _context.Users
                        .Select(x => x).FirstOrDefaultAsync(u => u.UUid == uuid);
            return result;
        }


        public async Task<UserResponse> CreateAsync(RegisterUserDTO user)
        {
            bool emailExists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());

            if (emailExists)
            {
                throw new BusinessException(
                    message: "Registration failed",
                    statusCode: StatusCodes.Status409Conflict,
                    errors: ["Email Already Exist"]
                );
            }

            var hasher = new PasswordHasher<User>();

            var _newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
            };

            _newUser.Password = hasher.HashPassword(_newUser, user.Password);

            try
            {
                _context.Users.Add(_newUser);
                var result = await _context.SaveChangesAsync();
                return new UserResponse
                {
                    UUid = _newUser.UUid,
                    Role = _newUser.Role,
                    FirstName = _newUser.FirstName,
                    LastName = _newUser.LastName,
                    Email = _newUser.Email
                };
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw new BusinessException(
                    message: "Registration failed",
                    statusCode: StatusCodes.Status500InternalServerError,
                    errors: ["Something went wrong when inserting into DB"]
                );
            }
        }
    }
}
