using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using SwiftMT103ApiTask.Models;
using SwiftMT103ApiTask.Services.MT103;

namespace SwiftMT103ApiTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MT103Controller : ControllerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly MT103Service _mt103Service;
        
        public MT103Controller(MT103Service mt103Service)
        {
            this._mt103Service = mt103Service;
        }

        /// <summary>
        ///     Gets all records from the database.
        /// </summary>
        /// <returns>A list of MT103Message or an empty list if no data is present in the db.</returns>
        [HttpGet]
        public async Task<ActionResult<List<MT103Message>>> GetAllMessages()
        {
            List<MT103Message> messages = await _mt103Service.GetAllAsync();

            return messages;
        }


        /// <summary>
        /// Returns a message by id
        /// </summary>
        /// <param name="id">The message id</param>
        /// <returns>MT103Message object if it is found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MT103Message), 200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MT103Message?>> GetMessageById(long id)
        {
            MT103Message? msg = await _mt103Service.GetByIdAsync(id);
            if (msg == null)
            {
                _logger.Warn("MT103 message with Id {id} not found.", id);
                return NotFound("Message with the given Id does not exist.");
            }

            _logger.Info("Successfully retreived message with id {id}.", id);
            return Ok(msg);
        }
        /// <summary>
        /// Uploads and proccesses an MT103 message text file
        /// </summary>
        /// <param name="file">The MT103 message text file</param>
        /// <returns>An HTTP response with a status message and the data if success.</returns>
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if(file == null || file.Length == 0) {
                    // No file uploaded OR empty
                    _logger.Warn("Request received without a file uploaded.");
                    return BadRequest(new { Error = "No file uploaded."});
                }

                if(!file.FileName.EndsWith(".txt"))
                {
                    _logger.Warn("Received a file upload with a wrong format. File name: {FileName}", file.FileName);
                    return BadRequest(new { Error = "Uploaded file must be a text file." });
                }

                // After file is successfully uploaded and it is of type .txt
                _logger.Info($"Proccessing MT103 message file: {file.FileName}");

                MT103Message msg = await _mt103Service.UploadFile(file);

                return Ok(new
                {
                    message = $"Successfully saved a MT103 Swift message with reference id: {msg.ReferenceNumber}",
                    data = msg
                });
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "There was an error proccessing the file. FileName: {FileName}", file?.FileName);
                return BadRequest(new { Error = "Something went wrong while processing your file."});
            }
        } 
    }
}
