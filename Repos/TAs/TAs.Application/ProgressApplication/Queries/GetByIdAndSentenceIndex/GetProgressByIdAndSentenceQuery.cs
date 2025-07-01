using MediatR;
using TAs.Application.ProgressApplication.DTOs.Queries.GetByIdAndSentence;
using TAs.Domain.Result;

namespace TAs.Application.ProgressApplication.Queries.GetByIdAndSentenceIndex
{
    public class GetProgressByIdAndSentenceQuery(Guid progressId, int sentenceIndex)
     : IRequest<Result<GetProgressByIdAndSentenceIndexDTO>>
    {
        public Guid ProgressId = progressId;
        public int SentenceIndex = sentenceIndex;
    }
}