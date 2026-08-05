// Copyright (C) 2026 Ruben Magaña Alvarado
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
using FenixEngine.DataBase.Src.Models;

namespace FenixEngine.DataBase.Src.Context.DTOs;

public class UserService
{
    public UserResponseDto MapToDto(UserArchitect entity)
    {
        return new UserResponseDto(
            entity.UserId,
            entity.UserName ?? string.Empty,
            entity.UserBirth,
            entity.UserCreation,
            entity.UserEdit == default ? null : entity.UserEdit
        );
    }

    public UserArchitect MapToEntity(CreateUserDto dto)
    {
        return new UserArchitect
        {
            UserName = dto.UserName,
            UserBirth = dto.UserBirth,
            UserCreation = DateTime.UtcNow
        };
    }
}
