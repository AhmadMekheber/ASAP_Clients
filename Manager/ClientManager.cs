using ASAP_Clients.Base;
using ASAP_Clients.Dto;
using ASAP_Clients.Entities;
using ASAP_Clients.Exceptions;
using ASAP_Clients.Manager.IManager;
using ASAP_Clients.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ASAP_Clients.Manager
{
    public class ClientManager : IClientManager
    {
        private readonly IClientsUnitOfWork _unitOfWork;

        private readonly IClientRepository _clientRepository;

        public ClientManager(IClientsUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _clientRepository = unitOfWork.ClientRepository;
        }

        public async Task<IEnumerable<ClientDto>> GetAll() 
        {
            var clients = _clientRepository.GetNotDeleted();

            return await MapClientsToDto(clients)
                .ToListAsync();
        }

        private IQueryable<ClientDto> MapClientsToDto(IQueryable<Client> clients) 
        {
            return clients.Select(client => new ClientDto(client.ID, client.FirstName, client.LastName, client.Email, client.PhoneNumber));
        }

        public async Task<ClientDto?> GetByID(long clientID)
        {
            ClientDto? clientDto = null;
            Client? client = await _clientRepository.GetByID(clientID);

            if (client != null)
            {
                clientDto = MapClientToDto(client);
            }

            return clientDto;
        }

        private ClientDto MapClientToDto(Client client)
        {
            return new ClientDto(client.ID, client.FirstName, client.LastName, client.Email, client.PhoneNumber);
        }

        public async Task<ClientDto> CreateClient(UpdateClientDto updateClientDto)
        {
            bool emailExists = await _clientRepository.EmailExists(updateClientDto.Email, null);

            if (emailExists)
            {
                //Email already exists
                throw new DuplicateEmailException();
            }

            Client client = MapClientFromUpdateClientDto(updateClientDto);

            _clientRepository.Add(client);

            await _unitOfWork.SaveChangesAsync();

            return MapClientToDto(client);
        }

        private Client MapClientFromUpdateClientDto(UpdateClientDto clientDto)
        {
            return new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                PhoneNumber = clientDto.PhoneNumber
            };
        }

        public async Task UpdateClient(long clientID, UpdateClientDto updateClientDto)
        {
            Client? client = await _clientRepository.GetByID(clientID);

            if (client == null) 
            {
                //Invalid client ID to update
                throw new RecordNotFoundException();
            }

            bool emailExists = await _clientRepository.EmailExists(updateClientDto.Email, clientID);

            if (emailExists)
            {
                //Email already exists
                throw new DuplicateEmailException();
            }

            MapClientFromUpdateClientDto(client, updateClientDto);

            await _unitOfWork.SaveChangesAsync();
        }

        private void MapClientFromUpdateClientDto(Client client, UpdateClientDto clientDto)
        {
            client.FirstName = clientDto.FirstName;
            client.LastName = clientDto.LastName;
            client.Email = clientDto.Email;
            client.PhoneNumber = clientDto.PhoneNumber;
        }

        public async Task DeleteClient(long clientID)
        {
            Client? client = await _clientRepository.GetByID(clientID);

            if (client == null) 
            {
                //Invalid client ID to delete
                throw new RecordNotFoundException();
            }

            client.IsDeleted = true;

            await _unitOfWork.SaveChangesAsync();
        }
    }
}