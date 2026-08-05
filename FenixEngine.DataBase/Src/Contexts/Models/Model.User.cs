using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FenixEngine.DataBase.Src.Models
{
    public class UserArchitect
    {
        [Key]
        public string UserId { get; init; } = Guid.NewGuid().ToString();
        public required string UserName { get; set; } 
        public DateTime UserBirth { get; set; }
        public DateTime UserCreation { get; init; } = DateTime.UtcNow;
        public DateTime? UserEdit { get; set; }
    }

    public class UserEmails
    {
        [Key]
        public string UEmailId { get; init; } = Guid.NewGuid().ToString();
        
        // FK opcional si quieres vincularlo al usuario
        public string? UserId { get; set; } 
        
        public required string UEmailAddress { get; set; }
        public DateTime UEmailCreation { get; init; } = DateTime.UtcNow;
        public DateTime? UEmailEdit { get; set; }
        public DateTime? UEmailDelete { get; set; }
    }

    public class ComandTerminal
    {
        [Key]
        public string CTerminalId { get; init; } = Guid.NewGuid().ToString();
        public required string CTerminalComand { get; set; }
        public DateTime CTerminalCreation { get; init; } = DateTime.UtcNow;
        public DateTime? CTerminalEdit { get; set; }
        public DateTime? CTerminalDelete { get; set; }
    }

    public class AgentRole
    {
        [Key]
        public string ARoleId { get; init; } = Guid.NewGuid().ToString();
        public required string ARoleName { get; set; }
        public DateTime ARoleCreation { get; init; } = DateTime.UtcNow;
        public DateTime? ARoleEdit { get; set; }
        public DateTime? ARoleDelete { get; set; }         
    }

    public class CollectAgents
    {
        [Key]
        public string CAgentId { get; init; } = Guid.NewGuid().ToString();
        
        [ForeignKey(nameof(AgentRole))]
        public required string ARoleId { get; set; }
        public AgentRole? AgentRole { get; set; }

        public required string CAgentName { get; set; }
        public DateTime CAgentCreation { get; init; } = DateTime.UtcNow;
        public DateTime? CAgentEdit { get; set; }
        public DateTime? CAgentDelete { get; set; }       
    }

    public class LLMModels
    {
        [Key]
        public string LModelId { get; init; } = Guid.NewGuid().ToString();
        
        [ForeignKey(nameof(CollectAgent))]
        public required string CAgentId { get; set; }
        public CollectAgents? CollectAgent { get; set; }

        public required string LModelName { get; set; }
        public DateTime LModelCreation { get; init; } = DateTime.UtcNow;
        public DateTime? LModelEdit { get; set; }
        public DateTime? LModelDelete { get; set; }
    }

    public class GeneralProyects
    {
        [Key]
        public string GProyectId { get; init; } = Guid.NewGuid().ToString();
        
        [ForeignKey(nameof(ProyectType))]
        public string? GPtypeId { get; set; }
        public ProyectType? ProyectType { get; set; }

        public required string GProyectName { get; set; }
        public string? GProyectComment { get; set; }
        public DateTime GProyectCreate { get; init; } = DateTime.UtcNow;
        public DateTime? GProyectEdit { get; set; }
        public DateTime? GProyectClose { get; set; }
    }

    public class ProyectType
    {
        [Key]
        public string PTypeId { get; init; } = Guid.NewGuid().ToString();
        public required string PTypeName { get; set; }
        public DateTime PTypeCreation { get; init; } = DateTime.UtcNow;
        public DateTime? PTypeEdit { get; set; }
        public DateTime? PTypeDelete { get; set; }
    }

    public class UserActivity
    {
        [Key] // O definir clave compuesta en DbContext
        public string ActivityId { get; init; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public required string UserId { get; set; }
        public UserArchitect? User { get; set; }

        [ForeignKey(nameof(Proyect))]
        public required string ProyectId { get; set; }
        public GeneralProyects? Proyect { get; set; }

        public DateTime UActivityCreation { get; init; } = DateTime.UtcNow;
    }

}
