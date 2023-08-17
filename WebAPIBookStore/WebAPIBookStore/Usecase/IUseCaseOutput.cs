using WebAPIBookStore.Dto;

namespace WebAPIBookStore.Usecase
{
    public interface IUseCaseOutput
    {
        public Output NotFound(string msg);

        public Output UnprocessableEntity(string msg);

        public Output InternalServer(string msg);

        public Output Success(Object data);
    }
}
