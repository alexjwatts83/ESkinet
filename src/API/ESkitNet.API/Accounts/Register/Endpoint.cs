using ESkitNet.API.Accounts.Dtos;
using ESkitNet.Identity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ESkitNet.API.Accounts.Register;

public static class Endpoint
{
    public record Request(RegisterDto RegisterDto);
    public record Response(bool Succeeded, IEnumerable<IdentityError> Errors);

    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        //// TODO figure out later if this can acutally happen
        //if (identityResult == null)
        //    return Results.BadRequest("Failure During registering the user");
        return (response.Succeeded)
            ? Results.Ok(new
            {
                response.Succeeded,
                response.Errors
            })
            : Results.ValidationProblem(response.Errors!.ToDictionary(x => x.Code, x => new string[] { x.Description }));
    }

    public record Command(RegisterDto RegisterDto) : ICommand<Result>;
    public record Result(bool Succeeded, IEnumerable<IdentityError> Errors);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.RegisterDto.FirstName).NotEmpty().WithMessage("First Name can not be empty");
            RuleFor(x => x.RegisterDto.LastName).NotEmpty().WithMessage("Last Name can not be empty");
            RuleFor(x => x.RegisterDto.Email).NotEmpty().WithMessage("Email can not be empty");
            RuleFor(x => x.RegisterDto.Password).NotEmpty().WithMessage("Password can not be empty");
        }
    }

    public class Handler(IServiceProvider sp) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var dto = command.RegisterDto;
            var newUser = new AppUser()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email.ToUpper(),
            };

            var signInManager = sp.GetRequiredService<SignInManager<AppUser>>();

            var result = await signInManager.UserManager.CreateAsync(newUser, dto.Password);

            // for some reason when I returned just result, map couldn't figure out how to
            // map the errors and if there was an error then the errors returned would return an
            // empty array
            return new Result(result.Succeeded, result.Errors);
        }
    }
}
