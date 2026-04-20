using NLog;
using SwiftMT103ApiTask.Models;
using SwiftMT103ApiTask.Repositories;

namespace SwiftMT103ApiTask.Services.MT103
{
    public class MT103Service
    {
        private IMT103Repository _repository;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public MT103Service(IMT103Repository repository)
        {
            _repository = repository;
        }

        public async Task<MT103Message> UploadFile(IFormFile file)
        {
            string content;
            
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                content = await reader.ReadToEndAsync();
            }

            MT103Message parsedMessage = MT103Parser.ParseMessage(content);
            _logger.Info("Successfully parsed message");


            long msgId = await _repository.SaveAsync(parsedMessage);
            parsedMessage.Id = msgId;
            _logger.Info("Successfully saved a parsed MT103 Swift message record.");

            return parsedMessage;
        }

        public async Task<List<MT103Message>> GetAllAsync()
        {
            List<MT103Message> messages = await _repository.GetAllAsync();

            return messages;
        }

        public async Task<MT103Message?> GetByIdAsync(long id)
        {
            MT103Message? msg = await _repository.GetByIdAsync(id);

            return msg;
        }

    }
}
