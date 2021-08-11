using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Models.Abstraction
{
    public interface ICommentRepository
    {
        public IQueryable<Comment> Comments { get;}

        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
    }
}
