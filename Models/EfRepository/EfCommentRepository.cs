using BookSwap.Models.Abstraction;
using System.Linq;

namespace BookSwap.Models.EfRepository
{
    public class EfCommentRepository : ICommentRepository
    {
        private ApplicationContext db;

        public EfCommentRepository(ApplicationContext db)
        {
            this.db = db;
        }

        public IQueryable<Comment> Comments => db.Comments;
    }
}
