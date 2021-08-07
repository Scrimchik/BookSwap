using System.Linq;

namespace BookSwap.Models.Abstraction
{
    public interface ICommentRepository
    {
        public IQueryable<Comment> Comments { get;}
    }
}
