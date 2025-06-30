using acheesporte_athlete_app.ViewModels;
using Flunt.Extensions.Br;
using Flunt.Extensions.Br.Validations;
using Flunt.Notifications;
using Flunt.Validations;


namespace acheesporte_athlete_app.Validations
{
    class RegisterContract : Contract<RegisterViewModel>
    {
            public RegisterContract(RegisterViewModel vm)
    {
        Requires()
            .IsNotNullOrWhiteSpace(vm.FirstName, "FirstName", "Nome é obrigatório")
            .IsNotNullOrWhiteSpace(vm.LastName, "LastName", "Sobrenome é obrigatório")
            .IsEmail(vm.Email, "Email", "Email inválido")
            .IsCpf(vm.Cpf, "Cpf", "CPF inválido")
            .IsPhoneNumber(vm.Phone, "Phone", "Telefone inválido");
    }
    }
}
