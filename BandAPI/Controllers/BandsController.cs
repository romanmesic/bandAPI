﻿using BandAPI.Models;
using BandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BandAPI.Helpers;
using AutoMapper;
using System.Text.Json;

namespace BandAPI.Controllers
{
  
    
    [ApiController]
    [Route("api/bands")]
    public class BandsController:ControllerBase
    {

        private readonly IBandAlbumRepository _bandAlbumRepository;
        private readonly IMapper _mapper;

        public BandsController(IBandAlbumRepository bandAlbumRepository, IMapper mapper)
        {

            _bandAlbumRepository = bandAlbumRepository ?? 
                throw new ArgumentNullException(nameof (bandAlbumRepository));
            
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        
        [HttpGet(Name = "GetBands")]
        [HttpHead]
        public ActionResult<IEnumerable<BandDto>> GetBands([FromQuery] BandsResourceParameters bandsResourceParameters)
        {

           
            var bandsFromRepo = _bandAlbumRepository.GetBands(bandsResourceParameters);


            var previousPageLink = bandsFromRepo.HasPrevious ?
                CreateBandsUri(bandsResourceParameters, UriType.PreviousPage) : null;

            var nextPageLink = bandsFromRepo.HasNext ?
                CreateBandsUri(bandsResourceParameters, UriType.NextPage) : null;

            var metaData = new
            {
                totalCount = bandsFromRepo.TotalCount,
                pageSize = bandsFromRepo.PageSize,
                currentPage = bandsFromRepo.CurrentPage,
                totalPages = bandsFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink

            };

            Response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData));


            return Ok(_mapper.Map<IEnumerable<BandDto>>(bandsFromRepo));
        
        }

        [HttpGet("{bandId}", Name ="GetBand")]
        public IActionResult GetBand(Guid bandId)
        {
            
            var bandFromRepo = _bandAlbumRepository.GetBand(bandId);
            if (bandFromRepo == null)
                return NotFound();
            return Ok(bandFromRepo);


        }

        [HttpPost]

        public ActionResult<BandDto> CreateBand([FromBody] BandForCreatingDto band)
        {
            var bandEntity = _mapper.Map<Entities.Band>(band);
            _bandAlbumRepository.AddBand(bandEntity);
            _bandAlbumRepository.Save();

            var bandToReturn = _mapper.Map<BandDto>(bandEntity);

            return CreatedAtRoute("GetBand", new { bandId = bandToReturn.Id }, bandToReturn);


        }

        public IActionResult GetBandsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,DELETE,HEAD,OPTIONS");
            return Ok();

        }

        [HttpDelete("{bandId}")]

        public ActionResult DeleteBand( Guid bandId)
        {

            var bandFromRepo = _bandAlbumRepository.GetBand(bandId);

            if (bandFromRepo == null)
                return NotFound();

            _bandAlbumRepository.DeleteBand(bandFromRepo);
            _bandAlbumRepository.Save();
            return NoContent();
        
        }
        
        private string CreateBandsUri(BandsResourceParameters bandsResourceParameters, UriType uriType) 
        {
            switch (uriType)
            {

                case UriType.PreviousPage:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber - 1,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery

                    });

                case UriType.NextPage:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber + 1,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery

                    });
                default:
                    return Url.Link("GetBands", new
                    {
                        pageNumber = bandsResourceParameters.PageNumber,
                        pageSize = bandsResourceParameters.PageSize,
                        mainGenre = bandsResourceParameters.MainGenre,
                        searchQuery = bandsResourceParameters.SearchQuery

                    });


            }

        }


    }
}
