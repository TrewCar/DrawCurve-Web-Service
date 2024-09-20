using MusicObjectCore = DrawCurve.Core.Objects.MusicObject;
using MusicObjectModel = DrawCurve.Domen.Models.Core.Objects.MusicObject;

namespace DrawCurve.Domen.DTO.Models.Objects
{
    public static class MusicObjectDTO
    {
        public static MusicObjectCore Transfer(this MusicObjectModel cnf)
            => new MusicObjectCore() { PathToMusic = cnf.PathToMusic };
        public static MusicObjectModel Transfer(this MusicObjectCore cnf)
            => new MusicObjectModel() { PathToMusic = cnf.PathToMusic };
    }
}
