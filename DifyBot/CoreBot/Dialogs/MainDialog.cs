using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ApiService _apiService;
        private readonly ILogger _logger;

        public MainDialog(ApiService apiService, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _apiService = apiService;
            _logger = logger;

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ProcessInputStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ProcessInputStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Context.Activity.Type == ActivityTypes.Message)
            {
                var userInput = stepContext.Context.Activity.Text;
                var apiResult = await _apiService.MakeApiCall("Dify_SHG", userInput);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(apiResult), cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}