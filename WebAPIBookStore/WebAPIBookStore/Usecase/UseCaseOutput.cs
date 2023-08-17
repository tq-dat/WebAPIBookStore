using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;

namespace WebAPIBookStore.Usecase
{
    public class UseCaseOutput : IUseCaseOutput
    {
        public Output InternalServer(string msg)
        {
            var output = new Output();
            output.Success = false;
            output.Error = StatusCodeAPI.InternalServer;
            output.Message = msg;
            return output;
        }

        public Output NotFound(string msg)
        {
            var output = new Output();
            output.Success = false;
            output.Error = StatusCodeAPI.NotFound;
            output.Message = msg;
            return output;
        }

        public Output Success(Object data)
        {
            var output = new Output();
            output.Success = true;
            output.Data = data;
            return output;
        }

        public Output UnprocessableEntity(string msg)
        {
            var output = new Output();
            output.Success = false;
            output.Error = StatusCodeAPI.UnprocessableEntity;
            output.Message = msg;
            return output;
        }
    }
}
