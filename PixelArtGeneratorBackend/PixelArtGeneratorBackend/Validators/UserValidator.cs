using System.Diagnostics.Eventing.Reader;
using YourPetAPI.Database;
using YourPetAPI.Models;
using YourPetAPI.Repositories;

namespace YourPetBackend.Validators
{

    public interface IUserValidator
    {
        Task<bool> RegisterUserValidate(User user);

        void IsNull(User user);

        bool CheckPassword(string password);

        public bool ChceckPersonalData(string firstName, string lastName, string phone, int role);
    }
    public class UserValidator : IUserValidator
    {
        private readonly IUserRepository userRepository;
        
        
        public UserValidator(IUserRepository userRepository) 
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> RegisterUserValidate(User user)
        { 
            //czy przyszły dane, poprawność maila i czy juz ktoś ma konto na tego maila
            ////, czy jest hasło i ma wiecej niz 6 znaków, imie, nazwisko, miasto, rola, telefon
            IsNull(user);
            bool emailexist = await userRepository.EMailExist(user);
            if(emailexist) 
            { 
                throw new Exception(); 
            }
            CheckPassword(user.PasswordHash);
           // ChceckPersonalData(user.FirstName, user.LastName, user.Phone, user.RoleId);
            return true;

        }

        public void IsNull(User user)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
                // pasuje kiedyś zrobić lepszą obśługę błędu 
                // throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public bool CheckPassword(string password)
        {
            if(password.Length < 6 || password == null)
            {
                throw new Exception();
            }
            return true;
            //
        }

        public bool ChceckPersonalData(string firstName, string lastName, string phone, int role)
        {
            if (firstName == null || lastName == null || phone == null) {
                throw new Exception();
            }
            if (role == 1 || role == 2 || role ==3) { 
               
            } else
            {
                throw new Exception();
            }
        
            return true;
        }
    }
}
