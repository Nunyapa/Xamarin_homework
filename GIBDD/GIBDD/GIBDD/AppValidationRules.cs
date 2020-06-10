using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace GIBDD
{
    class ProfileValidator : AbstractValidator<Profile>
    {

        public ProfileValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).Must(name => name.All(c => char.IsLetter(c) || c == '-')).WithMessage("Name should contain letters only");
            RuleFor(x => x.Sername).NotNull();
            RuleFor(x => x.Sername).Must(sername => sername.All(c => char.IsLetter(c))).WithMessage("Sername should contain letters only");
            RuleFor(x => x.Email).NotNull().EmailAddress();
            RuleFor(x => x.SelectedRegion).NotNull();
            RuleFor(x => x.SelectedDiv).NotNull();
        }
    }

    class EmailValidator : AbstractValidator<string>
    {

        public EmailValidator()
        {
            RuleFor(str => str).NotNull().EmailAddress();
        }
    }
}
