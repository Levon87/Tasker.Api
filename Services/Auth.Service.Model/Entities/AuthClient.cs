using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth.Service.Model.Entities
{
    [Table("AuthClients")]
    public class AuthClient
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string ApiKey { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid UserId { get; set; }
    }
}
