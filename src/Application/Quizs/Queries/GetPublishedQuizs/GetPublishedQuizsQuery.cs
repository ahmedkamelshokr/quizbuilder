namespace Application.Quizs.Queries
{
    public class GetPublishedQuizsQuery : IRequest<IEnumerable<PublishedQuizDto>>
    {
    }

    public class GetPublishedQuizsQueryHandler : IRequestHandler<GetPublishedQuizsQuery, IEnumerable<PublishedQuizDto>>
    {
        private readonly IApplicationRepository _repository;

        public GetPublishedQuizsQueryHandler(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PublishedQuizDto>> Handle(GetPublishedQuizsQuery request, CancellationToken cancellationToken)
        {

            var quizs = await _repository.Quizs
               .Where(q => q.Published && !q.Deleted)
               .Include(q => q.User).ToListAsync();

            if (quizs == null)
                return Enumerable.Empty<PublishedQuizDto>();

            var result = quizs.Select(q => new PublishedQuizDto
            { Code = q.Code, Title = q.Title, PublisheDate = q.PublisheDate.GetValueOrDefault(), Publisher = $"{q.User.FirstName} {q.User.LastName}" });

            return result;
        }
    }
}