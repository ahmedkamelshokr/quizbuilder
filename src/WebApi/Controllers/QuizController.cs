using Application.Quizs.Commands;
using System.ComponentModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Quizs.Queries;
using Application.Common.Models;

namespace WebAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuizController : ApiControllerBase
    {
        private readonly ISender _mediator;

        public QuizController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Description("Create a new quiz.")]

        public async Task<IActionResult> Create(CreateQuizCommand payload)
        {
            var quizCode = await _mediator.Send(payload);

            return Ok($"Quiz Code:{quizCode}");
        }
        [HttpGet]
        [Description("Get quiz for review.")]
        [Route("{quizCode}")]
        public async Task<EQuizDto> GetForEdit(string quizCode)
        {
            return await _mediator.Send(new GetQuizForEditQuery { QuizCode = quizCode });

        }

        [HttpPost]
        [Description("Add Questions to a quiz.")]

        public async Task<IActionResult> AddQuestions(AddQuestionToQuizCommand payload)
        {
            var quizCode = await _mediator.Send(payload);

            return Ok($"Question added to quiz Code:{quizCode}");
        }

        [HttpDelete]
        [Description("Remove Questions to a quiz.")]
        public async Task<IActionResult> RemoveQuestions(RemoveQuestionFromQuizCommand payload)
        {
            var quizCode = await _mediator.Send(payload);

            return Ok($"Question(s) removed from quiz Code:{quizCode}");
        }

        [HttpPut]
        [Description("Update a quiz.")]
        public async Task<IActionResult> Update(UpdateQuizCommand payload)
        {
            await _mediator.Send(payload);

            return NoContent();
        }

        [HttpPut]
        [Description("Publish a quiz.")]
        public async Task<IActionResult> Publish(PublishQuizCommand payload)
        {
            await _mediator.Send(payload);

            return NoContent();
        }

        [HttpPost]
        [Description("Delete a quiz.")]
        public async Task<IActionResult> Delete(DeleteQuizCommand payload)
        {
            await _mediator.Send(payload);

            return NoContent();
        }

        [HttpGet]
        [Description("Get published quizs.")]
        public async Task<IEnumerable<PublishedQuizDto>> GetPublished()
        {
            return await _mediator.Send(new GetPublishedQuizsQuery());

        }

        [HttpPost]
        //in case UI needs to start quiz then get questions
        [Description("Start a quiz.")]
        public async Task<IActionResult> Start(StartQuizCommand payload)
        {
            await _mediator.Send(payload);

            return NoContent();
        }

        [HttpGet]
        [Description("Get Quiz questions For Examiner.")]
        public async Task<QuizDto> GetQuizQuestions(GetQuizForExaminerQuery payload)
        {
            return await _mediator.Send(payload);
        }


        [HttpGet]
        [Description("Start and get quiz for examiner.")]
        [Route("{quizCode}")]
        public async Task<QuizDto> GetQuiz(string quizCode)
        {
            await _mediator.Send(new StartQuizCommand { QuizCode = quizCode });

            return await _mediator.Send(new GetQuizForExaminerQuery { QuizCode = quizCode });

        }

        [HttpPost]
        [Description("Submit a quiz.")]
        public async Task<IActionResult> Submit(SubmitQuizCommand payload)
        {
            var quizCode = await _mediator.Send(payload);

            return Ok($"Quiz:{quizCode} submitted"); ;
        }



        [HttpGet]
        [Description("Get quiz result.")]
        [Route("{quizCode}")]
        public async Task<QuizReultDto> GetResult(string quizCode)
        {

            return await _mediator.Send(new GetExaminerQuizResultQuery { QuizCode = quizCode });

        }

        [HttpGet]
        [Description("Get quiz result stats.")]
        [Route("{quizCode}")]
        public async Task<QuizReultStatistics> GetQuizStats(string quizCode)
        {
            return await _mediator.Send(new GetQuizResultStatsQuery { QuizCode = quizCode });

        }
    }
}
