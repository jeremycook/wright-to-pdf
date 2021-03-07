using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlaywrightSharp;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WrightToPdf.Controllers
{
    [ApiController]
    [Route("pdf")]
    public class PdfController : ControllerBase
    {
        private readonly ILogger<PdfController> logger;

        public PdfController(ILogger<PdfController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult Get() => Ok();

        [HttpPost]
        public async Task<ActionResult> GetAsync(PdfRequest input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            logger.LogInformation("Printing {Url}", input.Url);

            using var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GoToAsync(input.Url);
            var pdf = await page.GetPdfAsync(
                displayHeaderFooter: input.DisplayHeaderFooter,
                printBackground: input.PrintBackground,
                width: input.Width,
                height: input.Height,
                margin: input.Margin);

            return File(pdf, "application/pdf");
        }

        public class PdfRequest
        {
            [Required]
            public string Url { get; set; } = null!;
            public bool DisplayHeaderFooter { get; set; } = false;
            public bool PrintBackground { get; set; } = true;
            public string? Width { get; set; } = null!;
            public string? Height { get; set; } = null!;
            public Margin? Margin { get; set; } = null!;
        }
    }
}
