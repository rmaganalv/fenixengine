
using Microsoft.EntityFrameworkCore;
using FenixEngine.DataBase.Src.Models;
using FenixEngine.DataBase.Src.Context.DTOs;

namespace FenixEngine.DataBase.Src.Services
{
    public class UserService : IUserService
    {
        private readonly DbContext _context; // Reemplaza 'DbContext' por tu DbContext real (ej. FenixEngineDbContext)

        public UserService(DbContext context)
        {
            _context = context;
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            // 1. Crear entidad a partir del DTO
            var newUser = new UserArchitect
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = dto.UserName,
                UserBirth = dto.UserBirth,
                UserCreation = DateTime.UtcNow
            };

            _context.Set<UserArchitect>().Add(newUser);

            // 2. Si el DTO traía correos iniciales, agregarlos asociando el ID
            if (dto.InitialEmails != null && dto.InitialEmails.Any())
            {
                foreach (var email in dto.InitialEmails)
                {
                    var userEmail = new UserEmails
                    {
                        UEmailAddress = email,
                        UEmailCreation = DateTime.UtcNow
                    };
                    _context.Set<UserEmails>().Add(userEmail);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            // 3. Retornar DTO de respuesta
            return MapToResponseDto(newUser);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Set<UserArchitect>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            return user is null ? null : MapToResponseDto(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<UserArchitect>()
                .AsNoTracking()
                .Select(u => new UserResponseDto(
                    u.UserId!,
                    u.UserName!,
                    u.UserBirth,
                    u.UserCreation,
                    u.UserEdit
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Set<UserArchitect>()
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId, cancellationToken);

            if (user is null) return null;

            // Actualizar campos
            user.UserName = dto.UserName;
            user.UserBirth = dto.UserBirth;
            user.UserEdit = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return MapToResponseDto(user);
        }

        public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.Set<UserArchitect>()
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user is null) return false;

            _context.Set<UserArchitect>().Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Método auxiliar para Mapeo manual directo
        private static UserResponseDto MapToResponseDto(UserArchitect entity)
        {
            return new UserResponseDto(
                entity.UserId!,
                entity.UserName!,
                entity.UserBirth,
                entity.UserCreation,
                entity.UserEdit == default ? null : entity.UserEdit
            );
        }
    }
}
