using System;

namespace Backend.Interfaces
{
    public interface IAuditable
    {
        string? CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        
        string? UpdatedBy { get; set; }
        DateTime? UpdatedAt { get; set; }
        
        DateTime? DeletedAt { get; set; }
    }
}