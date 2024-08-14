using ActionConfigCore = DrawCurve.Core.Config.ActionConfig;
using ActionConfigModel = DrawCurve.Domen.Models.Core.ActionConfig;

namespace DrawCurve.Domen.DTO.Models
{
    public static class ActionConfigDTO
    {
        public static ActionConfigCore Transfer(this ActionConfigModel cnf)
            => new ActionConfigCore()
            {
                Key = cnf.Key,
                Name = cnf.Name,
                Description = cnf.Description,

                Step = cnf.Step,

                Start = cnf.Start,
                End = cnf.End,
            };

        public static ActionConfigModel Transfer(this ActionConfigCore cnf)
            => new ActionConfigModel()
            {
                Key = cnf.Key,
                Name = cnf.Name,
                Description = cnf.Description,

                Step = cnf.Step,

                Start = cnf.Start,
                End = cnf.End,
            };
    }
}
