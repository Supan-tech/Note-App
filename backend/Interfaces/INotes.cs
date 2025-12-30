using Backend.DTOs;
using Backend.Entity;


namespace Backend.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllAsync(int uid, QueryNoteDto query);
        Task<Note?> GetByIdAsync(int id);
        // Task<User?> GetByEmailAsync(string Email);
        Task<Note> CreateAsync(int uid,CreateNoteDto note);
        Task<Note> UpdateAsync(int userId, int uid, CreateNoteDto newNote);
        Task<bool> DeleteAsync(int userId, int uid);
    }
}
