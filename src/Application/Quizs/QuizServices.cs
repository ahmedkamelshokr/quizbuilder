namespace Application.Quizs
{
    public interface IQuizServices
    {
        Task<bool> IsExist(string quizCode, CancellationToken token);
        Task<bool> Published(string quizCode, CancellationToken token);
    }
    public class QuizServices : IQuizServices
    {
        private readonly IApplicationRepository _repository;

        public QuizServices(IApplicationRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> IsExist(string quizCode, CancellationToken token)
        {
            return await _repository.Quizs.AnyAsync(q => q.Code == quizCode, token);

        }
        public async Task<bool> Published(string quizCode, CancellationToken token)
        {
            var quiz = await _repository.Quizs.GetByCode(quizCode);
            return quiz != null && quiz.Published;
        }
    }
}
