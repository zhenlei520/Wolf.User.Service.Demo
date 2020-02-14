dotnet-ef migrations add init --startup-project ../User.GrpcService

dotnet-ef database update --startup-project ../User.GrpcService
