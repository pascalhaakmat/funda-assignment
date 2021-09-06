using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using funda.Services;
using Microsoft.Extensions.Options;

namespace funda.razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IFundaService _fundaService;
        private readonly FundaServiceOptions _options;

        public IndexModel(ILogger<IndexModel> logger, IFundaService fundaService, IOptions<FundaServiceOptions> options)
        {
            _logger = logger;
            _fundaService = fundaService;
            _options = options.Value;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; } = "/amsterdam";

        public List<KeyValuePair<string, string>> QueryTypes { get; set; } = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("/amsterdam", "Amsterdam"),
            new KeyValuePair<string, string>("/amsterdam/tuin", "Amsterdam met Tuin"),
        };

        public IEnumerable<Object1> AllObjects;
        public IEnumerable<MakelaarGrouping> MakelaarGroupings;

        public async Task OnGetAsync()
        {
            AllObjects = await _fundaService.GetAllObjects(SearchString);
            MakelaarGroupings = AllObjects.GroupByMakelaarId().Take(10);
        }
    }
}
