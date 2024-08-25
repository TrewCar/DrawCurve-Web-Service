using DrawCurve.Application.Menedgers.Renders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Menedgers
{
    public class MenedgerRenderHostedService : IHostedService
    {
        private readonly MenedgerGenerateFrames _menedgerRender;
        private readonly MenedgerConcatFrame _menedgerConcatFrames;
        private readonly MenedgerVideoConcatAudio _menedgerVideoConcatAudio;

        public MenedgerRenderHostedService(MenedgerGenerateFrames menedgerRender, MenedgerConcatFrame menedgerConcatFrame, MenedgerVideoConcatAudio menedgerVideoConcatAudio)
        {
            _menedgerRender = menedgerRender;
            _menedgerConcatFrames = menedgerConcatFrame;
            _menedgerVideoConcatAudio = menedgerVideoConcatAudio;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Здесь вы можете вызвать необходимые методы или инициализировать ресурсы
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Логика остановки сервиса, если требуется
            return Task.CompletedTask;
        }
    }

}
