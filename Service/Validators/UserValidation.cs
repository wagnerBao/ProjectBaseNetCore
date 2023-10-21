using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Validators
{
    public class UserValidation : AbstractValidator<Domain.Entities.UserEntities.User>
    {
        public UserValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome e obrigatorio")
                .NotNull().WithMessage("Nome e obrigatorio");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email e obrigatorio")
                .NotNull().WithMessage("Email e obrigatorio");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password e obrigatorio")
                .NotNull().WithMessage("Password e obrigatorio");
        }
    }
}
