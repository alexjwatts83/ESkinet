using ESkitNet.API.Accounts.Dtos;
using ESkitNet.API.Extensions;
using ESkitNet.Identity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace ESkitNet.API.Accounts.AddOrUpdateAddress;

public static class Endpoint
{
    public record Request(AddressDto AddressDto);
    public record Response(AddressDto? AddressDto);

    public static async Task<IResult> Handle(Request request, ISender sender)
    {
        var command = request.Adapt<Command>();

        var result = await sender.Send(command);

        var response = result.Adapt<Response>();

        return Results.Ok(response.AddressDto);
    }

    public record Command(AddressDto AddressDto) : ICommand<Result>;
    public record Result(AddressDto? AddressDto);

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.AddressDto.Line1).NotEmpty().WithMessage("Line1 can not be empty");
            RuleFor(x => x.AddressDto.City).NotEmpty().WithMessage("City can not be empty");
            RuleFor(x => x.AddressDto.State).NotEmpty().WithMessage("State can not be empty");
            RuleFor(x => x.AddressDto.PostalCode).NotEmpty().WithMessage("PostalCode can not be empty");
            RuleFor(x => x.AddressDto.Country).NotEmpty().WithMessage("Country can not be empty");
        }
    }

    public class Handler(IServiceProvider sp, IHttpContextAccessor context) : ICommandHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var signInManager = sp.GetRequiredService<SignInManager<AppUser>>();
            var user = await signInManager.UserManager.GetUserByEmail(context.HttpContext!.User, true);

            if (user.Address == null)
            {
                var address = command.AddressDto.Adapt<Address>();
                user.Address = address;
            } else
            {
                user.Address.UpdateFromDto(command.AddressDto);
            }

            var result = await signInManager.UserManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new BadHttpRequestException("There was a problem updating your address", StatusCodes.Status400BadRequest);

            var dto = user.Address.Adapt<AddressDto>();

            return new Result(dto);
        }
    }
}
