dotnet ef migrations add Initial \
--project src/Modules/Booking.Modules.Users/Booking.Modules.Users.csproj \
--startup-project src/Api/Booking.Api/Booking.Api.csproj \
--context UsersDbContext \
--configuration Debug \
--output-dir Presistence/Migrations


The project parameter (--project) specifies where to put the migration files (Users module)
The startup-project parameter (--startup-project) specifies which project has the DI container setup (API project)
This way EF Tools can use the API project's Program.cs to build the services and create the DbContex






in integrations tests , background jobs needs to be triggered manually because they are not automatically executed in the test environment  .
------

// Ensure the token is properly encoded for URL usage ( + , / , = .. )
string encodedToken = Uri.EscapeDataString(emailVerificationToken);
string verificationLink = $"{_frontEndOptions.BaseUrl}{_frontEndOptions.EmailVerificationPagePath}?token={encodedToken}&email={emailAdress}";
return verificationLink ?? throw new InvalidOperationException("Failed to generate verification link.");


dotnet ef migrations add SyncModelAndDb --project ../Infrastructure --startup-project .


System.Text.Json is case-insensitive by default, but Newtonsoft.Json is case-sensitive.


When you return your EF Core entities directly, System.Text.Json will serialize every public getter—mapped or not—so your DomainEvents list shows up even though it isn’t a database column.



[NotMapped]                  // EF Core will ignore it (optional, since it’s already not mapped)
[JsonIgnore]                 // JSON serializer will skip it
public List<IDomainEvent> DomainEvents => _domainContainer.DomainEvents;


to retrieve services :

var userContext = context.RequestServices.GetRequiredService<IUserContext>();
or
using var scope = app.Services.CreateScope();
var userContext = scope.ServiceProvider.GetRequiredService<IUserContext>();

or
private readonly IServiceProvider _serviceProvider;
using var scope = _serviceProvider.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();