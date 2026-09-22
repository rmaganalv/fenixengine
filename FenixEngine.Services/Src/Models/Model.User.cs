using System.ComponentModel.DataAnnotations;

namespace FenixEngine.Services.Src.Models
{
    public class UserArchitect
    {
        [Key]
        public string? UserId { get; init; } = Guid.NewGuid().ToString();
        public required string? UserName { get; set; } 
        public DateTime UserBirth { get; set; }
        public DateTime UserCreation { get; init; } = DateTime.UtcNow;
        public DateTime UserEdit { get; set; }
    }

    public class UserEmails
    {
        public string? UEmailId { get; init; } = Guid.NewGuid().ToString();
        public required string? UEmailAdress { get; set; }
        public DateTime UEmailCreation { get; init; } = DateTime.UtcNow;
        public DateTime UEmailEdit { get; set; }
        public DateTime UEmailDelete { get; set; }
    }


    public class ComandTerminal
    {
        public string? CTerminalId { get; init;} = Guid.NewGuid().ToString();
        public required string? CTerminalComand { get; set; }
        public DateTime CTerminalCreation { get; init; } = DateTime.UtcNow;
        public DateTime CTerminalEdit { get; set; }
        public DateTime CTerminalDelete { get; set; }
    }

    public class LLMModels
    {
        
    }
    

    public class GeneralProyects
    {
        [Key]
        public string? GProyectId { get; init; } = Guid.NewGuid().ToString();
        public string? GPtypeId { get; set; } 
        public required string? GProyectName {get;set;}
        public string? GProyectComment{ get; set; }
        public DateTime GProyectCreate { get; init; } = DateTime.UtcNow;
        public DateTime GProyectEdit {get; set;}
        public DateTime GProyectClose {get;set;}
    }

    public class ProyectType
    {
        [Key]
        public string? PTypeId{ get; init; } = Guid.NewGuid().ToString();
        public string? PTypeName{get;set;}
        public DateTime PTypeCreation { get; init; } = DateTime.UtcNow;
        public DateTime PTypeEdit { get; set; }
        public DateTime PtypeDelete { get; set; }
    }

    public class UserActivity
    {
        public required string? UserId { get; set; }
        public required string? ProyectId { get; set; }
        public DateTime UActivityCreation { get; init; } = DateTime.UtcNow;
    }

}
