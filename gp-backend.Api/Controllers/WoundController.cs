using gp_backend.Api.Dtos;
using gp_backend.EF.MySql.Repositories.Interfaces;
using gp_backend.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using gp_backend.Api.Dtos.Wound;
using Mscc.GenerativeAI;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Identity.Client;

namespace gp_backend.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WoundController : ControllerBase
    {
        private readonly IGenericRepo<Wound> _woundRepo;
        private readonly ILogger<WoundController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepo<Disease> _diseaseRepo;
        public WoundController(IGenericRepo<Wound> woundRepo, ILogger<WoundController> logger,
        IGenericRepo<Disease> diseaseRepo, UserManager<ApplicationUser> userManager)
        {
            _woundRepo = woundRepo;
            _logger = logger;
            _userManager = userManager;
            _diseaseRepo = diseaseRepo;
        }

        // add
        [HttpPost("upload-type")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                var fileDescription = GetDescription(file);

                //if(!(await checkInjury(fileDescription.Content.Content)))
                //{
                //    return Ok(new BaseResponse(true, ["You are health"], null));
                //}

                var response = await CallFlaskEndPoint(file, "type");
                string mask = await GetTheMask(file);
                if (response == null)
                    return BadRequest();
                Disease diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(x => x.Name.Contains(response[0]));


                var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

                var user = await _userManager.FindByIdAsync(uid);

                var wound = new Wound
                {
                    UploadDate = DateTime.Now.Date,
                    User = user,
                    ApplicationUserId = user.Id,
                    Image = fileDescription
                };
                string diseaseName = "";
                string description = "";
                string risk = "";
                var prevention = new List<string>();

                if (diseasse == null)
                {
                    return NoContent();
                }

                wound.Disease.Add(diseasse);
                diseaseName += diseasse.Name;
                description += diseasse.Description + "\n\n";
                prevention.AddRange(diseasse.Preventions);
                risk += diseasse.Risk;

                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetWoundDetailsDto
                {
                    Id = wound.Id,
                    Name = diseaseName,
                    Description = description,
                    Image = mask,
                    Preventions = prevention,
                    Risk = risk,
                    UploadDate = wound.UploadDate
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { ex.Message }, null));
            }
        }

        [HttpPost("upload-burn")]
        public async Task<IActionResult> UploadBurn(IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                var fileDescription = GetDescription(file);

                //if (!(await checkInjury(fileDescription.Content.Content)))
                //{
                //    return Ok(new BaseResponse(true, ["You are health"], null));
                //}

                var response = await CallFlaskEndPoint(file, "burn");

                // call the mask
                string mask = await GetTheMask(file);

                if (response == null)
                    return BadRequest();

                Disease diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(x => x.Name.Contains(response[0]));

                var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

                var user = await _userManager.FindByIdAsync(uid);

                var wound = new Wound
                {
                    UploadDate = DateTime.Now.Date,
                    User = user,
                    ApplicationUserId = user.Id,
                    Image = fileDescription
                };
                string diseaseName = "";
                string description = "";
                string risk = "";
                var prevention = new List<string>();
                if (diseasse == null)
                    return NoContent();

                wound.Disease.Add(diseasse);
                diseaseName += diseasse.Name;
                description += diseasse.Description + "\n\n";
                prevention.AddRange(diseasse.Preventions);
                risk += diseasse.Risk;


                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetWoundDetailsDto
                {
                    Id = wound.Id,
                    Name = diseaseName,
                    Description = description,
                    Image = mask,
                    Preventions = prevention,
                    Risk = risk,
                    UploadDate = wound.UploadDate
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { ex.Message }, null));
            }
        }

        [HttpPost("upload-skin")]
        public async Task<IActionResult> UploadSkin(IFormFile file)
        {
            try
            {
                // check the properties validation
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList(), null));
                }

                var fileDescription = GetDescription(file);

                //if (!(await checkInjury(fileDescription.Content.Content)))
                //{
                //    return Ok(new BaseResponse(true, ["You are health"], null));
                //}

                var response = await CallFlaskEndPoint(file, "skin");
                string mask = await GetTheMask(file);
                if (response == null)
                    return BadRequest();

                Disease diseasse = (await _diseaseRepo.GetAllAsync("")).FirstOrDefault(x => x.Name.Contains(response[0]));

                if (diseasse == null)
                    return NoContent();

                var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;

                var user = await _userManager.FindByIdAsync(uid);

                var wound = new Wound
                {
                    UploadDate = DateTime.Now.Date,
                    User = user,
                    ApplicationUserId = user.Id,
                    Image = fileDescription
                };
                string diseaseName = "";
                string description = "";
                string risk = "";
                var prevention = new List<string>();

                wound.Disease.Add(diseasse);
                diseaseName += diseasse.Name;
                description += diseasse.Description + "\n\n";
                prevention.AddRange(diseasse.Preventions);
                risk += diseasse.Risk;


                var result = await _woundRepo.InsertAsync(wound);
                await _woundRepo.SaveAsync();

                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, new GetWoundDetailsDto
                {
                    Id = wound.Id,
                    Name = diseaseName,
                    Description = description,
                    Image =mask,
                    Preventions = prevention,
                    Risk = risk,
                    UploadDate = wound.UploadDate
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "an error occurred while uploading the file.");
                return StatusCode(500, new BaseResponse(false, new List<string> { ex.Message }, null));
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var uid = User.Claims.FirstOrDefault(x => x.Type == "uid").Value;
            var wounds = await _woundRepo.GetAllAsync(uid);

            var resultList = new List<GetWoundDto>();
            foreach(var wound in wounds)
            {
                string name = "";
                foreach (var item in wound.Disease)
                    name += item.Name + " / ";
                resultList.Add(new GetWoundDto
                {
                    Id = wound.Id,
                    file = Convert.ToBase64String( wound?.Image?.Content?.Content),
                    Name = name,
                    AddedDate = wound.UploadDate.ToShortDateString(),
                });
            }
            if(resultList.Count > 0)
                return Ok(new BaseResponse(true, new List<string> { "Uploaded Successfuly" }, resultList));
            else
                return Ok(new BaseResponse(true, new List<string> { "History Empty" }, resultList));
        }

        [HttpGet("get-id")]
        public async Task<IActionResult> GetById(int id)
        {
            // check the properties validation
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse(state: false, message: ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList(), null));
            }

            var result = new Wound();
            if (id > 0)
            {
                result = await _woundRepo.GetByIdAsync(id);
                if (result != null)
                {
                    string diseaseName = "";
                    string description = "";
                    string risk = "";
                    var prevention = new List<string>();
                    foreach(var item in result.Disease)
                    {
                        diseaseName += item.Name + " / ";
                        description += $"{item.Name}: \n\n";
                        description += item.Description + "\n\n";
                        prevention.AddRange(item.Preventions);
                        risk += item.Risk + "\n\n";
                    }

                    return Ok(new BaseResponse(true, new List<string> { "Success" }, new GetWoundDetailsDto
                    {
                        Id = result.Id,
                        Description = description,
                        Name = diseaseName,
                        Preventions = prevention,
                        UploadDate = result.UploadDate,
                        Image = Convert.ToBase64String(result?.Image?.Content?.Content),
                        Risk = risk
                    }));
                }
                else
                    return NotFound();
            }
            return BadRequest(new BaseResponse(false, new List<string> { "id not valid" }, null));
        }    
        /*
         Methods
         */
        private FileDescription GetDescription(IFormFile file)
        {
            byte[] fileBytes;

            using (var fs = file.OpenReadStream())
            {
                using (var sr = new BinaryReader(fs))
                {
                    fileBytes = sr.ReadBytes((int)file.Length);
                }
            }
            var fileContent = new FileContent
            {
                Content = fileBytes
            };
            return new FileDescription
            {
                Content = fileContent,
                ContentType = file.ContentType,
                ContentDisposition = file.ContentDisposition,
            };
        }
        private async Task<List<string>> CallFlaskEndPoint(IFormFile file, string endpoint)
        {
            using(var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://9160-105-197-94-90.ngrok-free.app");
                using(var content = new MultipartFormDataContent())
                {
                    var fileStream = file.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "file", file.FileName);

                    var response = await client.PostAsync($"https://39b7-102-189-121-121.ngrok-free.app/{endpoint}", content);
                    if (!response.IsSuccessStatusCode)
                        return [""];

                    var result = await response.Content.ReadFromJsonAsync<WoundIdDto>();

                    return result.Output;
                }
            }
        }

        private async Task<string> GetTheMask(IFormFile file)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("https://9160-105-197-94-90.ngrok-free.app");
                using (var content = new MultipartFormDataContent())
                {
                    var fileStream = file.OpenReadStream();
                    var fileContent = new StreamContent(fileStream);

                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                    content.Add(fileContent, "file", file.FileName);

                    var response = await client.PostAsync($" https://39b7-102-189-121-121.ngrok-free.app/location", content);
                    if (!response.IsSuccessStatusCode)
                        return "";

                    var result = await response.Content.ReadFromJsonAsync<MaskDto>();

                    return result.Output;
                }
            }
        }

        private async Task<bool> checkInjury(byte[] content)
        {
            string apiKey = "AIzaSyBmVnnwiEoKcJTGMBXZptF1Y1cUU7fat0g";
            var googleAI = new GoogleAI(apiKey: apiKey);
            var model = googleAI.GenerativeModel(model: Model.Gemini15FlashLatest);
            var prompt = "can you tell me if there is injury or skin disease in this image? answer only with yes or no. if you can not answer with no only";

            var request = new GenerateContentRequest(prompt);

            // save the image in the folder

            var filePath = Path.Combine("wwwroot", "images", "image.png");
            await System.IO.File.WriteAllBytesAsync(filePath, content);

            if (!System.IO.File.Exists(filePath))
                return false;

            await request.AddMedia("http://localhost:5093/images/image.png");

            int retries = 3;
            while (retries > 0)
            {
                try
                {
                    var response = await model.GenerateContent(request);
                    string answer = response.Text.Substring(0, 3).ToLower();

                    // Delete the image
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    return answer.Contains("yes");
                }
                catch (Exception ex)
                {
                    if (retries == 0 || !ex.Message.Contains("overloaded"))
                    {
                        // Log the error or handle the exception
                        return false;
                    }

                    retries--;
                    await Task.Delay(TimeSpan.FromSeconds(5 * (3 - retries)));  // Exponential backoff
                }
            }

            // Delete the image if the request failed
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return false;
        }

    }
}
