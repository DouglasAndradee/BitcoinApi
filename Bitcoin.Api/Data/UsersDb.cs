using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using Bitcoin.Api.Data.Configuration;
using Bitcoin.Api.Models;

namespace Bitcoin.Api.Data
{
    public class UsersDb
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UsersDb(IUsersDatabaseSettings settings)
        {
            var client = new MongoClient("mongodb+srv://douglasA:921026@sandbox.c6fz6.mongodb.net/");
            var database = client.GetDatabase("UsersApiDb");

            _usersCollection = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public List<User> Get()
        {
            return _usersCollection.Find(cli => true).ToList();
        }

        public User GetById(string id)
        {
            return _usersCollection.Find<User>(user => user.Id == id).FirstOrDefault();
        }

        public User GetByEmail(string email)
        {
            return _usersCollection.Find<User>(user => user.Email == email).FirstOrDefault();
        }

        public User Create(User usr)
        {
            _usersCollection.InsertOne(usr);
            return usr;
        }

        public void Update(string id, User usr)
        {
            _usersCollection.ReplaceOne(user => user.Id == id, usr);
        }

        public void Delete(User usr)
        {
            _usersCollection.DeleteOne(user => user.Id == usr.Id);
        }

        public void DeleteById(string id)
        {
            _usersCollection.DeleteOne(user => user.Id == id);
        }
    }

}
