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

namespace FenixEngine.DataBase.Src.Context.DTOs;

/// <summary>
/// DTO utilizado para recibir los datos al registrar o crear un nuevo usuario.
/// </summary>
public record CreateUserDto(
    string UserName,
    DateTime UserBirth,
    List<string>? InitialEmails = null
);

/// <summary>
/// DTO utilizado cuando el usuario solicita actualizar su información personal.
/// </summary>
public record UpdateUserDto(
    string UserId,
    string UserName,
    DateTime UserBirth
);

/// <summary>
/// DTO de respuesta para la interfaz de usuario (IDE) o servicios externos.
/// Expone únicamente la información necesaria sin acoplarse a las restricciones de EF Core.
/// </summary>
public record UserResponseDto(
    string UserId,
    string UserName,
    DateTime UserBirth,
    DateTime UserCreation,
    DateTime? UserEdit
);

