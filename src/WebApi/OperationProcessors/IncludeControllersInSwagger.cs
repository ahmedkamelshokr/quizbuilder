using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace WebApi.OperationProcessors
{

    public class IncludeControllersInSwagger : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            bool controllerIsIncluded = TakeADecisionBasedOn(context.ControllerType);
            return controllerIsIncluded;
        }

        private bool TakeADecisionBasedOn(Type controllerType)
        {
            return controllerType.Namespace == "WebAPI.Controllers";
        }
    }
}