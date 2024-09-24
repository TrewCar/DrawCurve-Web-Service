using DrawCurve.Domen.Models;
using DrawCurve.Domen.Responces;
using DrawCurve.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawCurve.Application.Interface
{
    public interface IVideoService
    {
        public string GetVideo(string Key);

        public VideoInfo? GetInfo(string Key);

        public VideoInfo? Publish(VideoResponce responce, int AuthorId);
    }
}
