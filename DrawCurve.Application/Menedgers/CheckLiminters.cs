using DrawCurve.Application.Config;
using DrawCurve.Domen.Models;
using Microsoft.Extensions.Options;

namespace DrawCurve.Application.Menedgers
{
    public class CheckLiminters
    {
        private LimiterConfig config;
        public CheckLiminters(IOptions<RenderApplicationConfig> renderConfig)
        {
            this.config = renderConfig.Value.Limiter;
        }


        /// <summary>
        /// Проверка конфигурационных файлов на соответствие лимитам
        /// </summary>
        /// <param name="cnf"></param>
        /// <returns></returns>
        public List<string> CheckConfig(ref RenderInfo cnf)
        {
            List<string> errors = new();

            // Проверка на превышение лимитов FPS
            if (cnf.RenderConfig.FPS < config.Video.MinFPS)
            {
                errors.Add("FPS - превышиает минимальное значение в " + config.Video.MaxFPS);
                cnf.RenderConfig.FPS = config.Video.MinFPS;
            }
            else if (cnf.RenderConfig.FPS > config.Video.MaxFPS)
            {
                errors.Add("FPS - превышает максимально значение в " + config.Video.MaxFPS);
                cnf.RenderConfig.FPS = config.Video.MaxFPS;
            }

            // Проверка на превышение лимитов ширины
            if (cnf.RenderConfig.Width < config.Video.MinWidth)
            {
                errors.Add("Ширина - превышает минимальное значение в " + config.Video.MinWidth);
                cnf.RenderConfig.Width = config.Video.MinWidth;
            }
            else if (cnf.RenderConfig.Width > config.Video.MaxWidth)
            {
                errors.Add("Ширина - превышает максимальное значение в " + config.Video.MaxWidth);
                cnf.RenderConfig.Width = config.Video.MaxWidth;
            }

            // Проверка на превышение лимитов высоты
            if (cnf.RenderConfig.Height < config.Video.MinHeight)
            {
                errors.Add("Высота - превышает минимальное значение в " + config.Video.MinHeight);
                cnf.RenderConfig.Height = config.Video.MinHeight;
            }
            else if (cnf.RenderConfig.Height > config.Video.MaxHeight)
            {
                errors.Add("Высота - превышает максимальное значение в " + config.Video.MaxHeight);
                cnf.RenderConfig.Height = config.Video.MaxHeight;
            }

            // Проверка на превышение лимитов времени
            if (cnf.RenderConfig.Time < config.Video.MinTime)
            {
                errors.Add("Время - превышает минимальное значение в " + config.Video.MinTime);
                cnf.RenderConfig.Time = config.Video.MinTime;
            }
            else if (cnf.RenderConfig.Time > config.Video.MaxTime)
            {
                errors.Add("Время - превышает максимальное значение в " + config.Video.MaxTime);
                cnf.RenderConfig.Time = config.Video.MaxTime;
            }

            return errors;
        }
    }
}
