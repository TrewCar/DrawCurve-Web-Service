using DrawCurve.Application.Interface;
using DrawCurve.Core.Window;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application.Menedgers.Renders
{
    public abstract class MenedgerRender<TValDictionary>
    {
        protected TypeStatus search;
        protected TypeStatus proccess;
        protected TypeStatus end;
        protected IServiceProvider _serviceProvider;
        protected ILogger _logger;
        protected ISendTickRender _resiveMsg;

        public Dictionary<string, (int Author, TValDictionary Value)> Renders { get; protected set; } = new();
        protected List<string> KeyRenderByEnd = new();

        public MenedgerRender(IServiceProvider serviceProvider, ILogger logger,
            TypeStatus search, TypeStatus proccess, TypeStatus end)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
            //this._resiveMsg = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ISendTickRender>();

            this.search = search;
            this.proccess = proccess;
            this.end = end;

            using var scope = _serviceProvider.CreateScope();
            var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();

            var renders = queue.GetQueue(proccess);
            foreach (var item in renders)
            {
                _logger.LogInformation($"Add break proccess - {item.KEY}");
                Add(item.AuthorId, item.KEY);
            }

            _ = Task.Run(CheckEndAsync);
        }
        public abstract string Add(int Author, string Key);
        private async Task CheckEndAsync()
        {
            while (true)
            {
                _logger.LogInformation("Start search queue");
                using var scope = _serviceProvider.CreateScope();
                var queue = scope.ServiceProvider.GetRequiredService<IRenderQueue>();
                var tempList = KeyRenderByEnd.ToList();

                foreach (var key in tempList)
                {
                    _logger.LogInformation($"END - {key}");
                    var render = Renders[key];

                    // No need to call thread.Join(), the task should have already completed
                    Renders.Remove(key);

                    var renderInfo = queue.GetRender(key);
                    queue.UpdateState(renderInfo, end);
                    KeyRenderByEnd.Remove(key);
                }

                if (Renders.Count < 10)
                {
                    var items = queue.GetQueue(search);

                    for (int i = 0; i < items.Count - Renders.Count; i++)
                    {
                        _logger.LogInformation($"START - {items[i].KEY}");
                        queue.UpdateState(items[i], proccess);
                        Add(items[i].AuthorId, items[i].KEY);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1)); // wait 1 minute to next iteration
            }
        }

        public void SendTick(RenderTick tick)
        {

        }
    }
}
