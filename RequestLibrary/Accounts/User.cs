using RequestLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace RequestLibrary
{
    public class User
    {
        public int id;
        public string seshID;
        public string username;
        public string password;
        public StarSystem position;
        public int positionID;

        public int galacticCredits;
        public int diplomaticWeight;

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
        }

        public User()
        {

        }

        public User(int galacticCredits, int diplomaticWeight)
        {
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
        }

        public User(string username, string password, int positionID, int galacticCredits, int diplomaticWeight)
        {
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
            this.positionID = positionID;
        }

        public User(int id, string username, string password, int positionID, int galacticCredits, int diplomaticWeight)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.galacticCredits = 1000;
            this.diplomaticWeight = 0;
            this.galacticCredits = galacticCredits;
            this.diplomaticWeight = diplomaticWeight;
            this.positionID = positionID;
        }
    }
}
