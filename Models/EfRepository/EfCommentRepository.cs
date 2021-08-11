using BookSwap.Models.Abstraction;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task AddCommentAsync(Comment comment)
        {
            db.Comments.Add(comment);
            await db.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
        }
    }
}
