using EmsiSoft.Models;
using EmsiSoft.RabbitMQ.Interface;
using EmsiSoft.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmsiSoft.Controllers
{
    [ApiController]
    [Route("hashes")]
    public class HashesController : ControllerBase
    {
        private readonly ILogger<HashesController> _logger;
        private readonly IHashesService _hashesService;
        private readonly IRabbitMQProducer _rabbitMQProducer;

        public HashesController(ILogger<HashesController> logger, IHashesService hashesService, IRabbitMQProducer rabbitMQProducer)
        {
            _logger = logger;
            _hashesService = hashesService;
            _rabbitMQProducer = rabbitMQProducer;
        }

        [HttpGet]
        public HashResponseModel Get()
        {
            try
            {
                var hashList = _hashesService.ListGroupedByDate();

                if (hashList.Any())
                    return new HashResponseModel() { Hashes = hashList };

                return new HashResponseModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return new HashResponseModel();
            }
        }

        [HttpPost]
        public ResponseModel Post()
        {
            try
            {
                var hashList = _hashesService.GenerateRandomHash(40000);

                if (hashList.Any())
                {
                    var itemsPerPage = 10000;
                    var pageCount = (hashList.Count() / itemsPerPage);

                    Parallel.For(0, pageCount, pc =>
                    {
                        var queue = new HashModel { Date = DateTime.Now, Hashes = hashList.OrderBy(m => m).Skip(itemsPerPage * pc).Take(itemsPerPage) };

                        _rabbitMQProducer.SendProductMessage(queue);
                    });

                    return new ResponseModel() { Message = "Successfully queued 40,000 random SHA1 hashes to RabbitMQ for further processing.", Success = true };
                }

                return new ResponseModel() { Message = "No data to process", Success = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message.ToString());
                return new ResponseModel() { Message = ex.Message.ToString(), Success = false };
            }
        }
    }
}