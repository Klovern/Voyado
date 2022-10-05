using IVoyadoManagement.Search;
using VoyadaManagement.Options.Search;
using VoyadaManagement.Search;
using VoyadoMain.Areas.Search.Libraries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Register in IOC

// This should normally be split into a ManagementSpecific WireUp IoC to publish serverlogic to avoid references to uneccessary projects like UI web project
// However the entire "management" part is really up to how you would like to expose and reuse server code logic
// Since we build alot apis and having other people integrating and building solutions based on our servercode
// Its exposed through a nugepackage and therefor although depending on the usecase this could be a necessity or not
builder.Services.AddSingleton<IGoogleSearchManager, GoogleSearchManager>();
builder.Services.AddSingleton<IBingSearchManager, BingSearchManager>();

// Register libraries for the web to use
builder.Services.AddSingleton<ISearchLibrary, SearchLibrary>();

// Instanciate IOptionsPattern, however normally I'd normally at work use a regular XML config published in a folder since i mainly work on-prem solutions
// However IOptions is easier and faster to implement
builder.Services.Configure<GoogleApiSettings>
        (builder.Configuration.GetSection("GoogleApiSettings"));
builder.Services.Configure<BingApiSettings>
        (builder.Configuration.GetSection("BingApiSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

// Set default routing to SearchController
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Search}/{action=Index}/");

app.MapRazorPages();

app.Run();
