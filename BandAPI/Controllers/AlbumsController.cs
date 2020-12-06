using AutoMapper;
using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Controllers
{
    [ApiController]
    [Route("api/bands/{bandId}/albums")]
    public class AlbumsController : ControllerBase
    {

        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;

        public AlbumsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {

            _bandAlbumRepository = bandAlbumRepository ??
                throw new ArgumentNullException(nameof(bandAlbumRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<AlbumsDto>> GetAlbumsForBand(Guid bandId)
        {

            if (!_bandAlbumRepository.BandExists(bandId))
                return NotFound();

            var albumsFromRepo = _bandAlbumRepository.GetAlbums(bandId);
            return Ok(_mapper.Map<IEnumerable<AlbumsDto>>(albumsFromRepo));
        
        
        }




    }
}
