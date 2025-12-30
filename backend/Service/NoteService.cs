using Backend.DTOs;
using Backend.Entity;
using Backend.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Service
{
    public class NoteService : INoteService
    {
        private static readonly List<User> _users = new();

        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public NoteService(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }


        public async Task<IEnumerable<Note>> GetAllAsync(int uid, QueryNoteDto query)
        {
            IQueryable<Note> notes = _context.Notes
                            .Where(n => n.UserUID == uid);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                notes = notes.Where(n =>
                    n.Title.Contains(query.Search));
            }

            if (query.CreatedFrom.HasValue)
                notes = notes.Where(n => n.CreatedDate >= query.CreatedFrom.Value);

            if (query.CreatedTo.HasValue)
                notes = notes.Where(n => n.CreatedDate <= query.CreatedTo.Value);

            notes = query.SortBy?.ToLower() switch
            {
                "date-desc" => notes.OrderByDescending(n => n.CreatedDate),

                "date-asc" =>  notes.OrderBy(n => n.CreatedDate),

                "title-asc" =>  notes.OrderBy(n => n.Title),
                "title-desc" =>  notes.OrderByDescending(n => n.Title),

                _ => notes.OrderByDescending(n => n.CreatedDate) 
            };

            // var total = await notes.CountAsync();

            //Couldn't implement pagination in time
            // notes = notes
            //         .Skip((query.Page - 1) * query.PageSize)
            //         .Take(query.PageSize);

            return await notes.ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            var result = await _context.Notes.Select(x => x).FirstOrDefaultAsync(u => u.Uid == id);

            return result;
        }

        // public async Task<User?> GetByEmailAsync(string Email)
        // {
        //     var result = await _context.Users
        //                 .Select(x => x).FirstOrDefaultAsync(u => u.Email == Email);
        //     return result;
        // }
        public async Task<Note> CreateAsync(int uid, CreateNoteDto note)
        {
            var _newNote = new Note
            {
                UserUID = uid,
                Title = note.Title,
                Content = note.Content
            };

            try
            {
                _context.Notes.Add(_newNote);
                var result = await _context.SaveChangesAsync();
                return _newNote;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw new BusinessException(
                    message: "Note creation failed",
                    statusCode: StatusCodes.Status500InternalServerError,
                    errors: ["Something went wrong when inserting into DB"]
                );
            }
        }


        public async Task<Note> UpdateAsync(int userId, int uid, CreateNoteDto newNote)
        {
            var note = await _context.Notes.FindAsync(uid);
            if (note == null)
            {
                throw new BusinessException(
                   message: "Can't retreive Note",
                   statusCode: StatusCodes.Status404NotFound,
                   errors: ["Note Not Found"]
               );
            }

            if (note.UserUID != userId)
            {
                throw new BusinessException(
                   message: "You are not allowed to modify this note",
                   statusCode: StatusCodes.Status403Forbidden,
                   errors: ["Access denied"]
               );
            }

            note.Title = newNote.Title;
            note.Content = newNote.Content;
            note.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<bool> DeleteAsync(int userId, int uid)
        {
            var note = await _context.Notes.FindAsync(uid);
            if (note == null)
            {
                throw new BusinessException(
                   message: "Can't retreive Note",
                   statusCode: StatusCodes.Status404NotFound,
                   errors: ["Note Not Found"]
               );
            }

            if (note.UserUID != userId)
            {
                throw new BusinessException(
                   message: "You are not allowed to modify this note",
                   statusCode: StatusCodes.Status403Forbidden,
                   errors: ["Access denied"]
               );
            }
            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
