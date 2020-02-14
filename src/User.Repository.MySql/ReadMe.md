dotnet-ef migrations add init --startup-project ../User.Grpc.Service

dotnet-ef database update --startup-project ../User.Grpc.Service
