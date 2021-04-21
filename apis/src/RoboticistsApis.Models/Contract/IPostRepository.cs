using System.Net;
using System.Threading.Tasks;
using RoboticistsApis.Models.Domain;

namespace RoboticistsApis.Models.Contract
{
    public interface IPostRepository
    {
        Task<(HttpStatusCode result, string message)> Save(BlogPost blogPost);
    }
}