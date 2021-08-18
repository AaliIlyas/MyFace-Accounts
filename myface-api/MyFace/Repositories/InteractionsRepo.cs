using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFace.Models.Database;
using MyFace.Models.Request;

namespace MyFace.Repositories
{
    public interface IInteractionsRepo
    {
        IEnumerable<Interaction> Search(SearchRequest search);
        int Count(SearchRequest search);
        Interaction GetById(int id);
        Interaction Create(CreateInteractionRequest create, string authHeader);
        void Delete(int id);
    }

    public class InteractionsRepo : IInteractionsRepo
    {
        private readonly MyFaceDbContext _context;

        public InteractionsRepo(MyFaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Interaction> Search(SearchRequest search)
        {
            return _context.Interactions
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(SearchRequest search)
        {
            return _context.Interactions.Count();
        }

        public Interaction GetById(int id)
        {
            return _context.Interactions.Single(i => i.Id == id);
        }

        public Interaction Create(CreateInteractionRequest create, string authHeader)
        {
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            var seperatorIndex = usernamePassword.IndexOf(':');
            var username = usernamePassword.Substring(0, seperatorIndex);

            var userId = _context.Users
                .Where(u => u.Username == username)
                .Single()
                .Id;

            var insertResult = _context.Interactions.Add(new Interaction
            {
                Date = DateTime.Now,
                Type = create.InteractionType,
                PostId = create.PostId,
                UserId = userId,
            });
            _context.SaveChanges();
            return insertResult.Entity;
        }

        public void Delete(int id)
        {
            var interaction = GetById(id);
            _context.Interactions.Remove(interaction);
            _context.SaveChanges();
        }
    }
}